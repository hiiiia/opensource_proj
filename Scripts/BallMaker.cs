using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMaker : MonoBehaviour
{
    public GameObject line;
    public GameObject fireBall;
    public GameObject fastBall;// ��ź ������Ʈ ����
    public GameObject holeObj1;          // ���� ��ġ�� ������Ʈ
    public GameObject holeObj2;          // ���� ��ġ�� ������Ʈ
    public GameObject holeObj3;          // ���� ��ġ�� ������Ʈ
    public GameObject holeObj4;          // ���� ��ġ�� ������Ʈ
    public float delayTime = 3.0f;      // ��� �ð�
    public float delayTime1 = 10.0f;      // ��� �ð�
    public float fireSpeedX = -5.0f;    // x ������ �ӵ�
    public float fireSpeedY = 0.0f;     // y ������ �ӵ�
    public float length = 8.0f;         // ĳ���Ϳ��� ���� �Ÿ�

    GameObject player;                  // ĳ���� ������Ʈ�� ����
    Vector3 holePos1;
    Vector3 holePos2;
    Vector3 holePos3;
    Vector3 holePos4;  // ���� ��ġ ������ ����
    Vector3 playerPos;
    
    float passedTimes = 0.0f;    // ������ �ð� (��� �ð���ŭ ������ �߻�)
    float passedTimes1 = 0.0f;
    float passedTimes2 = 3.0f;// ������ �ð� (��� �ð���ŭ ������ �߻�)

    void Start()
    {
        // ���� ��ġ ȹ��
        holePos1 = holeObj1.transform.position;
        holePos2 = holeObj2.transform.position;
        holePos3 = holeObj3.transform.position;
        holePos4 = holeObj4.transform.position;

        // �÷��̾� ������Ʈ ȹ�� 
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // �ð� ����
     
        passedTimes += Time.deltaTime;
        passedTimes1 += Time.deltaTime;
        passedTimes2 += Time.deltaTime;
        // ĳ���Ϳ� �Ÿ� Ȯ��

        if (passedTimes > delayTime)
        {
            holePos1 = holeObj1.transform.position;
            holePos2 = holeObj2.transform.position;
            holePos3 = holeObj3.transform.position;
            holePos4 = holeObj4.transform.position;
            // �ð� �ʱ�ȭ
            passedTimes = 0;
            int number = Random.Range(0, 3);
            // ��ź�� ����
            switch (number)
            {
                case 0:
                    pattern1(); break;
                case 1:
                    pattern2(); break;
                case 2:
                    pattern3(); break;

                    // ��ź�� ���� ���Ͽ� ���ư��� �Ѵ�.
            }
        }
        if (passedTimes2 > delayTime1)
        {
            playerPos = player.transform.position;
            GameObject obj = Instantiate(line, playerPos, Quaternion.identity);
            obj.transform.Rotate(0, 0, -90);
            Destroy(obj, 3.0f);
            passedTimes2 = 0;
        }
        if (passedTimes1 > delayTime1)
        {
            playerPos = playerPos + new Vector3(0f, 20f, 0f);

            GameObject fb = Instantiate(fastBall, playerPos, Quaternion.identity);
            fb.transform.Rotate(0, 0, -90);
            Rigidbody2D rbody10 = fb.GetComponent<Rigidbody2D>();
            Vector2 v1 = new Vector2(fireSpeedX, fireSpeedY * 10.0f);
            rbody10.AddForce(v1, ForceMode2D.Impulse);
            Destroy(fb, 4.0f);
            passedTimes1 = 0;
            if(delayTime1>4f)
            delayTime1 = delayTime1 / 1.3f;
        }


    }


    void pattern1()
    {
        GameObject obj1 = Instantiate(fireBall, holePos1, Quaternion.identity);
        GameObject obj2 = Instantiate(fireBall, holePos3, Quaternion.identity);
        GameObject obj3 = Instantiate(fireBall, holePos4, Quaternion.identity);
        Rigidbody2D rbody = obj1.GetComponent<Rigidbody2D>();
        Rigidbody2D rbody2 = obj2.GetComponent<Rigidbody2D>();
        Rigidbody2D rbody3 = obj3.GetComponent<Rigidbody2D>();
        Vector2 v = new Vector2(fireSpeedX, fireSpeedY);
        rbody.AddForce(v, ForceMode2D.Impulse);
        rbody2.AddForce(v, ForceMode2D.Impulse);
        rbody3.AddForce(v, ForceMode2D.Impulse);
    }
    void pattern2()
    {
        GameObject obj1 = Instantiate(fireBall, holePos2, Quaternion.identity);
        GameObject obj2 = Instantiate(fireBall, holePos3, Quaternion.identity);
        GameObject obj3 = Instantiate(fireBall, holePos4, Quaternion.identity);
        Rigidbody2D rbody = obj1.GetComponent<Rigidbody2D>();
        Rigidbody2D rbody2 = obj2.GetComponent<Rigidbody2D>();
        Rigidbody2D rbody3 = obj3.GetComponent<Rigidbody2D>();
        Vector2 v = new Vector2(fireSpeedX, fireSpeedY);
        rbody.AddForce(v, ForceMode2D.Impulse);
        rbody2.AddForce(v, ForceMode2D.Impulse);
        rbody3.AddForce(v, ForceMode2D.Impulse);
    }
    void pattern3()
    {
        GameObject obj1 = Instantiate(fireBall, holePos1, Quaternion.identity);
        GameObject obj2 = Instantiate(fireBall, holePos2, Quaternion.identity);
        GameObject obj3 = Instantiate(fireBall, holePos3, Quaternion.identity);
        Rigidbody2D rbody = obj1.GetComponent<Rigidbody2D>();
        Rigidbody2D rbody2 = obj2.GetComponent<Rigidbody2D>();
        Rigidbody2D rbody3 = obj3.GetComponent<Rigidbody2D>();
        Vector2 v = new Vector2(fireSpeedX, fireSpeedY);
        rbody.AddForce(v, ForceMode2D.Impulse);
        rbody2.AddForce(v, ForceMode2D.Impulse);
        rbody3.AddForce(v, ForceMode2D.Impulse);
    }
}