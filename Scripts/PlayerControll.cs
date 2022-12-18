using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    Animator animator;

    public string moveAnime = "player";
    public string damageAnime ="playerdamage";
    public string goalAnime = "playergoal";
    public string deadAnime = "playerdead";
    public string transferAnime = "transfer";

    string nowAnime = "";       // 현재 애니메이션 변수
    string oldAnime = "";       // 기존 단계 변수

    Rigidbody rbody;      // 물리 변수    
    public float speed = 3.0f;  // 이동 속도 변수
    float yPos;

    public float delaytime = 3f;
    float passedTimes = 0.0f;
    public static string gameState = "playing";

    public static int hp = 3;

    void Start()
    {
        animator = GetComponent<Animator>();
        nowAnime = moveAnime;
        oldAnime = moveAnime;
        rbody = this.GetComponent<Rigidbody>();  // 이동용 물리 설정 요소를 가져온다.
        Vector3 v1 = new Vector3(speed, 0, 0);

        rbody.velocity = new Vector3(speed, 0, 0);

    }

    void FixedUpdate()
    {
        if (gameState != "playing") // 플레이 상태가 아니면 중단
        {
            return;
        }
        
        passedTimes += Time.deltaTime;

        if (passedTimes > delaytime)
        {
            nowAnime = moveAnime;
            if (Input.GetKey(KeyCode.Z))
            {
                passedTimes = 0;
                yPos = -4.6f;
                nowAnime = transferAnime;
            }
            else if (Input.GetKey(KeyCode.X))
            {
                passedTimes = 0;
                yPos = -1.0f;
                nowAnime = transferAnime;
            }
            else if (Input.GetKey(KeyCode.C))
            {
                passedTimes = 0;
                yPos = 3.2f;
                nowAnime = transferAnime;
            }
        }
       
        rbody.velocity = new Vector3(speed, 0, 0);
        float xPos = transform.position.x;
        transform.position = new Vector3(xPos, yPos, 0f);

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
            Debug.Log(nowAnime);// 현재 애니메이션 재생
        }

    }
    // 충돌처리
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "goal")
        {
            Goal();// 골인
        }
        else if (col.gameObject.tag == "damage")
        {
            hp--;
            if(hp<=0)
            {
                GameOver();// 게임 오버
            }
         
        }
        
    }

    // 골인
    public void Goal()
    {
        gameState = "gameClear";
        animator.Play(goalAnime);
        GameStop(); // 게임 정지

    }

    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameover";
        GameStop(); // 게임 정지

        //
        //
        //
        // 충돌체 삭제
        GetComponent<SphereCollider>().enabled = false;
        // 플레이어를 위로 뛰어오르게 함

    }
    void GameStop()
    {
        // 이동 요소를 가져온다. 
        Rigidbody rbody = GetComponent<Rigidbody>();
        rbody.velocity = new Vector3(0,0, 0); // 속도를 0으로 변경
    }


}