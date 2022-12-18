using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float deleteTime = 15.0f;     // 삭제될때까지 시간
    public float fireSpeedX = 0.0f;    // x 방향의 속도
    public float fireSpeedY = -5.0f;     // y 방향의 속도

    void Start()
    {
        transform.Rotate(0, 0, -90);

        Destroy(gameObject, deleteTime);        // 일정 시간 후 삭제    
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}