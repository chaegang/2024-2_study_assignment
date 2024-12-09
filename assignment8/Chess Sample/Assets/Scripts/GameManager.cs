using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 프리팹들
    public GameObject TilePrefab;
    public GameObject[] PiecePrefabs;   // King, Queen, Bishop, Knight, Rook, Pawn 순
    public GameObject EffectPrefab;

    // 오브젝트의 parent들
    private Transform TileParent;
    private Transform PieceParent;
    private Transform EffectParent;
    
    private MovementManager movementManager;
    private UIManager uiManager;
    
    public int CurrentTurn = 1; // 현재 턴 1 - 백, 2 - 흑
    public Tile[,] Tiles = new Tile[Utils.FieldWidth, Utils.FieldHeight];   // Tile들
    public Piece[,] Pieces = new Piece[Utils.FieldWidth, Utils.FieldHeight];    // Piece들

    void Awake()
    {
        TileParent = GameObject.Find("TileParent").transform;   
        PieceParent = GameObject.Find("PieceParent").transform;
        EffectParent = GameObject.Find("EffectParent").transform;
        
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        movementManager = gameObject.AddComponent<MovementManager>();
        movementManager.Initialize(this, EffectPrefab, EffectParent);
        
        InitializeBoard();
    }

    void InitializeBoard()
    {
        // 8x8로 타일들을 배치
        // TilePrefab을 TileParent의 자식으로 생성하고, 배치함
        // Tiles를 채움
        // --- TODO ---
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                GameObject NewTile = Instantiate(TilePrefab, new Vector3(0, 0, 1), Quaternion.identity, TileParent);
                Tile tile = NewTile.GetComponent<Tile>();
                
                Tiles[x, y] = tile;

                tile.Set((x, y));
            }
        }
        // ------

        PlacePieces(1);
        PlacePieces(-1);
    }

    void PlacePieces(int direction)
    {
        // PlacePiece를 사용하여 Piece들을 적절한 모양으로 배치
        // --- TODO ---
        if (direction == 1)
        {
            PlacePiece(0, (4, 0), direction);//킹
            PlacePiece(1, (3, 0), direction);//퀸
            PlacePiece(2, (2, 0), direction);
            PlacePiece(2, (5, 0), direction);//비숍
            PlacePiece(3, (1, 0), direction);
            PlacePiece(3, (6, 0), direction);//나이트
            PlacePiece(4, (0, 0), direction);
            PlacePiece(4, (7, 0), direction);//룩
            for (int x = 0; x <= 7; x++)
            {
                PlacePiece(5, (x, 1), direction);
            }
        }
        else
        {
            PlacePiece(0, (4, 7), direction);//킹
            PlacePiece(1, (3, 7), direction);//퀸
            PlacePiece(2, (2, 7), direction);
            PlacePiece(2, (5, 7), direction);//비숍
            PlacePiece(3, (1, 7), direction);
            PlacePiece(3, (6, 7), direction);//나이트
            PlacePiece(4, (0, 7), direction);
            PlacePiece(4, (7, 7), direction);//룩
            for (int x = 0; x <= 7; x++)
            {
                PlacePiece(5, (x, 6), direction);
            }
        }
        // ------
    }

    Piece PlacePiece(int pieceType, (int, int) pos, int direction)
    {
        // Piece를 배치 후, initialize
        // PiecePrefabs의 원소를 사용하여 배치, PieceParent의 자식으로 생성
        // Pieces를 채움
        // 배치한 Piece를 리턴
        // --- TODO ---
        GameObject NewPiece = Instantiate(PiecePrefabs[pieceType], new Vector3(0, 0, 0), Quaternion.identity, PieceParent);
        
        Piece piece = NewPiece.GetComponent<Piece>();
        Pieces[pos.Item1, pos.Item2] = piece;

        piece.initialize(pos, direction);

        return piece;
        // ------
    }

    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        return movementManager.IsValidMove(piece, targetPos);
    }

    public void ShowPossibleMoves(Piece piece)
    {
        movementManager.ShowPossibleMoves(piece);
    }

    public void ClearEffects()
    {
        movementManager.ClearEffects();
    }


    public void Move(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMove(piece, targetPos)) return;
        
        // 해당 위치에 다른 Piece가 있다면 삭제
        // Piece를 이동시킴
        // --- TODO ---
        if (!IsValidMove(piece, targetPos)) return;
    
        Piece TargetPiece = Pieces[targetPos.Item1, targetPos.Item2];
        if (TargetPiece != null)
        {
            Destroy(TargetPiece.gameObject);
        }

        //기존 위치를 비움
        (int, int) originalPos = piece.MyPos;

        // Piece를 이동시킴
        piece.MoveTo(targetPos);
        Pieces[originalPos.Item1, originalPos.Item2] = null; //올바른 이전 위치를 null로 설정
        Pieces[targetPos.Item1, targetPos.Item2] = piece;

        ChangeTurn();
        // ------
    }

    void ChangeTurn()
    {
        // 턴을 변경하고, UI에 표시
        // --- TODO ---
        if (CurrentTurn == 1) CurrentTurn = -1;
        else CurrentTurn = 1;

        uiManager.UpdateTurn(CurrentTurn);
        // ------
    }
}
