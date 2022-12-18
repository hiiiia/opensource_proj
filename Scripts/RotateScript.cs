using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript  : MonoBehaviour
{
    [Header("ȸ���ӵ� ����")]
    [SerializeField] [Range(1f, 100f)] float rotateSpeed = 50f;
    
    void Update()
    {
        //ȸ��
        if (Input.GetKey(KeyCode.Z))
            transform.Rotate(0, 0, Time.deltaTime * rotateSpeed, Space.Self);
        if (Input.GetKey(KeyCode.X))
            transform.Rotate(0, 0, -Time.deltaTime * rotateSpeed, Space.Self);

    }
}
