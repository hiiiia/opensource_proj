using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    public GameObject subScreen;        // ������ũ���� ����

    public bool isForceScrollX = false;     // x�� ���� ��ũ�� ����
    public float forceScrollSpeedX = 0.5f;  // x�� �̵� �ӵ�
    public bool isForceScrollY = false;     // y�� ���� ��ũ�� ����
    public float forceScrollSpeedY = 0.5f;  // y�� �̵� �ӵ�

    void Update()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");     // �÷��̾ ã�´�.
        if (player != null) // �÷��̾ ã������ ī�޶� ����
        {
            // ī�޶� �÷��̾ ���󰡰� �Ѵ�.
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;
            // �¿�� ����
            if (isForceScrollX)
            {
                // ������ ���� ��ũ��
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            }
            // �¿� �̵� ����  x = Mathf.Clamp(x, leftLimit, rightLimit);
            if (x < leftLimit)
            {
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }
            // ���Ϸ� ����
            if (isForceScrollY)
            {
                // �Ʒ��� ���� ��ũ��
                y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }
            // ���� �̵� ����
            if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }
            // ī�޶� ��ġ ����
            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;

            // ���� ��ũ���� �ٸ� �ӵ��� �̵��ϰ� �Ѵ�.
            if (subScreen != null)
            {
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                Vector3 v = new Vector3(x / 2.0f, y, z);
                subScreen.transform.position = v;
            }
        }
    }
}