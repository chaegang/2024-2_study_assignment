using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject effectPrefab;
    private Transform effectParent;
    private List<GameObject> currentEffects = new List<GameObject>();   // 현재 effect들을 저장할 리스트

    public void Initialize(GameManager gameManager, GameObject effectPrefab, Transform effectParent)
    {
        this.gameManager = gameManager;
        this.effectPrefab = effectPrefab;
        this.effectParent = effectParent;
    }

    private bool TryMove(Piece piece, (int, int) targetPos, MoveInfo moveInfo)
    {
        // moveInfo의 distance만큼 direction을 이동시키며 이동이 가능한지를 체크
        // 보드에 있는지, 다른 piece에 의해 막히는지 등을 체크
        // 폰에 대한 예외 처리를 적용
        // --- TODO ---
        int sx = piece.MyPos.Item1;
        int sy = piece.MyPos.Item2;
        int dx = moveInfo.dirX;
        int dy = moveInfo.dirY;

        for (int step = 1; step <= moveInfo.distance; step++)
        {
            int tox = sx + dx * step;
            int toy = sy + dy * step;

            if (!Utils.IsInBoard((tox, toy))) return false;

            Piece encounteredPiece = gameManager.Pieces[tox, toy];

            if ((tox, toy) == (targetPos.Item1, targetPos.Item2))
            {
                if (piece is Pawn)
                {
                    return dx == 0 ? encounteredPiece == null :
                        encounteredPiece != null && encounteredPiece.PlayerDirection != piece.PlayerDirection;
                }
                return encounteredPiece == null || encounteredPiece.PlayerDirection != piece.PlayerDirection;
            }

            if (encounteredPiece != null) return false;
        }

        return false;
        // ------
    }

    // 체크를 제외한 상황에서 가능한 움직임인지를 검증
    private bool IsValidMoveWithoutCheck(Piece piece, (int, int) targetPos)
    {
        if (!Utils.IsInBoard(targetPos) || targetPos == piece.MyPos) return false; //보드를 넘어가는 상황과 현재 위치일때 false

        foreach (var moveInfo in piece.GetMoves())
        {
            if (TryMove(piece, targetPos, moveInfo))
                return true;
        }

        return false;
    }

    // 체크를 포함한 상황에서 가능한 움직임인지를 검증 : 움직였을때, 그 곳이 체크가 되는 위치라면, flase를 반환함
    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMoveWithoutCheck(piece, targetPos)) return false;

        // 체크 상태 검증을 위한 임시 이동
        var originalPiece = gameManager.Pieces[targetPos.Item1, targetPos.Item2];
        var originalPos = piece.MyPos;

        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = piece;
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = null;
        piece.MyPos = targetPos;

        bool isValid = !IsInCheck(piece.PlayerDirection);

        // 원상 복구
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = piece;
        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = originalPiece;
        piece.MyPos = originalPos;

        return isValid;
    }

    // 체크인지를 확인
    private bool IsInCheck(int playerDirection)
    {
        (int, int) kingPos = (-1, -1); // 왕의 위치
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var piece = gameManager.Pieces[x, y];
                if (piece is King && piece.PlayerDirection == playerDirection)
                {
                    kingPos = (x, y);
                    break;
                }
            }
            if (kingPos.Item1 != -1 && kingPos.Item2 != -1) break;
        } //Pieces를 탐색하여, 왕의 위치를 얻음
        // 왕이 지금 체크 상태인지를 리턴
        // gameManager.Pieces에서 Piece들을 참조하여 움직임을 확인
        // --- TODO ---
        bool isCheck = false;

        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var piece = gameManager.Pieces[x, y];
                if (piece != null && piece.PlayerDirection != playerDirection)
                {
                    if (IsValidMoveWithoutCheck(piece, kingPos))
                    {
                        isCheck = true;
                        break;
                    }
                }
            }
            if (isCheck) break;
        }

        return isCheck;
        // ------
    }

    public void ShowPossibleMoves(Piece piece)
    {
        ClearEffects();

        // 가능한 움직임을 표시
        // IsValidMove를 사용
        // effectPrefab을 effectParent의 자식으로 생성하고 위치를 적절히 설정
        // currentEffects에 effectPrefab을 추가
        // --- TODO ---
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                if (IsValidMove(piece, (x, y)))
                {
                    Vector2 realXY = Utils.ToRealPos((x, y));
                    GameObject newEffect = Instantiate(effectPrefab, realXY, Quaternion.identity, effectParent);
                    currentEffects.Add(newEffect);
                }
            }
        }
        // ------
    }

    // 효과 비우기
    public void ClearEffects()
    {
        foreach (var effect in currentEffects)
        {
            if (effect != null) Destroy(effect);
        }
        currentEffects.Clear();
    }
}