using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceScript : MonoBehaviour
{

    Rigidbody2D rbody;
    [Header("�л緮 ����")]
    [SerializeField][Range(1f, 100f)] float Force = 1.0f;

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();   // �̵��� ���� ���� ��Ҹ� �����´�.
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
            rbody.AddForce(transform.up * Force);
    }

}
