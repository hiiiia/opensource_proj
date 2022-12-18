using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float deleteTime = 15.0f;     // �����ɶ����� �ð�
    public float fireSpeedX = 0.0f;    // x ������ �ӵ�
    public float fireSpeedY = -5.0f;     // y ������ �ӵ�

    void Start()
    {
        transform.Rotate(0, 0, -90);

        Destroy(gameObject, deleteTime);        // ���� �ð� �� ����    
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}