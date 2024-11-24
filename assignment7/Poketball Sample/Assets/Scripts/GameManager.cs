using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        MyUIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        CamObj = GameObject.Find("Main Camera");
        PlayerBall = GameObject.Find("PlayerBall");
        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ---------- 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ShootBallTo(hit.point);
            }
        }
        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ---------- 
        Vector3 startPosition = StartPosition;
        float ballSpace = 2 * BallRadius + RowSpacing;
        int totalRows = 5;
        int index = 1; 

        for (int row = 0; row < totalRows; row++)
        {
            int ballCountInRow = row + 1;

            float zOffset = row * (BallRadius * 2 + RowSpacing);
            float xOffset = (ballCountInRow - 1) * (ballSpace / 2); 

            for (int ballIndex = 0; ballIndex < ballCountInRow; ballIndex++)
            {
  
                Vector3 ballPosition = startPosition - new Vector3(xOffset - ballIndex * ballSpace, 0, zOffset);
                GameObject newBall = Instantiate(BallPrefab, ballPosition, StartRotation);
                newBall.GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/ball_{index}");
                newBall.name = index.ToString();
                index++; 
            }
        }
        // -------------------- 
    }

    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다. Unity Lerp
        // ---------- TODO ---------- 
        Vector3 targetPosition = new Vector3(PlayerBall.transform.position.x, CamObj.transform.position.y, PlayerBall.transform.position.z);
        CamObj.transform.position = Vector3.MoveTowards(CamObj.transform.position, targetPosition, CamSpeed * Time.deltaTime);
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ---------- 
        Rigidbody rb = PlayerBall.GetComponent<Rigidbody>();
        Vector3 direction = (targetPos - PlayerBall.transform.position);
        float power = CalcPower(direction);

        rb.velocity = new Vector3(direction.x, 0, direction.z).normalized * power;
        // -------------------- 
    }

    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        MyUIManager.DisplayText($"{ballName} falls", 1f);
        // -------------------- 
    }
}