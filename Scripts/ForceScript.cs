using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceScript : MonoBehaviour
{

    Rigidbody2D rbody;
    [Header("분사량 조절")]
    [SerializeField][Range(1f, 100f)] float Force = 1.0f;

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();   // 이동용 물리 설정 요소를 가져온다.
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
            rbody.AddForce(transform.up * Force);
    }

}
