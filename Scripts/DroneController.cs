using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    // �ִϸ��̼� ����
    Animator animator;      // �ִϸ޴��� ȣ��� ����
    public string stopAnime = "PlayerStop";
    public string leftAnime = "PlayerLeft";
    public string RightAnime = "PlayerRight";
    public string deadAnime = "PlayerOver";
    public string goalAnime = "PlayerGoal";


    [Header("ȸ���ӵ� ����")]
    [SerializeField][Range(1f, 200f)] float rotateSpeed = 50f;

    Rigidbody2D rbody;
    [Header("�л緮 ����")]
    [SerializeField][Range(1f, 100f)] float Force = 1.0f;

    string nowAnime = "";       // ���� �ִϸ��̼� ����
    string oldAnime = "";       // ���� �ܰ� ����

    public static string gameState = "playing"; // ���� ����

    void Start()
    {

        // Animator ��Ҹ� �����´�.
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing";  // ���� ��
        rbody = this.GetComponent<Rigidbody2D>();

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (gameState != "playing") // �÷��� ���°� �ƴϸ� �ߴ�
        {
            return;
        }


        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);    // ���� �ִϸ��̼� ���
        }

        //ȸ��
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


        //�л�
        if (Input.GetKey(KeyCode.L))
        {
            rbody.AddForce(transform.up * Force);
        }

    }


    // �浹ó��
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Goal")
        {
            Goal();// ����
        }
        else if (col.gameObject.tag == "Dead")
        {
            GameOver();// ���� ����
        }
    }

    // ����
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameClear";
        GameStop(); // ���� ����
    }

    // ���� ����
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameOver";
        GameStop(); // ���� ����
        //
        //
        //
        // �浹ü ����
        GetComponent<CapsuleCollider2D>().enabled = false;

    }

    // ���� ����
    void GameStop()
    {
        // �̵� ��Ҹ� �����´�. 
        rbody.velocity = new Vector2(0, 0); // �ӵ��� 0���� ����
    }
}