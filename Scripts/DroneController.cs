using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    // 애니메이션 대응
    Animator animator;      // 애니메니터 호출용 변수
    public string stopAnime = "PlayerStop";
    public string leftAnime = "PlayerLeft";
    public string RightAnime = "PlayerRight";
    public string deadAnime = "PlayerOver";
    public string goalAnime = "PlayerGoal";


    [Header("회전속도 조절")]
    [SerializeField][Range(1f, 200f)] float rotateSpeed = 50f;

    Rigidbody2D rbody;
    [Header("분사량 조절")]
    [SerializeField][Range(1f, 100f)] float Force = 1.0f;

    string nowAnime = "";       // 현재 애니메이션 변수
    string oldAnime = "";       // 기존 단계 변수

    public static string gameState = "playing"; // 게임 상태

    void Start()
    {

        // Animator 요소를 가져온다.
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing";  // 게임 중
        rbody = this.GetComponent<Rigidbody2D>();

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (gameState != "playing") // 플레이 상태가 아니면 중단
        {
            return;
        }


        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);    // 현재 애니메이션 재생
        }

        //회전
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(0, 0, Time.deltaTime * rotateSpeed, Space.Self);
            nowAnime = leftAnime;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            transform.Rotate(0, 0, -Time.deltaTime * rotateSpeed, Space.Self);
            nowAnime = RightAnime;
        }


        //분사
        if (Input.GetKey(KeyCode.L))
        {
            rbody.AddForce(transform.up * Force);
        }

    }


    // 충돌처리
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Goal")
        {
            Goal();// 골인
        }
        else if (col.gameObject.tag == "Dead")
        {
            GameOver();// 게임 오버
        }
    }

    // 골인
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameClear";
        GameStop(); // 게임 정지
    }

    // 게임 오버
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameOver";
        GameStop(); // 게임 정지
        //
        //
        //
        // 충돌체 삭제
        GetComponent<CapsuleCollider2D>().enabled = false;

    }

    // 게임 정지
    void GameStop()
    {
        // 이동 요소를 가져온다. 
        rbody.velocity = new Vector2(0, 0); // 속도를 0으로 변경
    }
}