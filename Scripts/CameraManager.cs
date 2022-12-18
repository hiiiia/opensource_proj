using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    public GameObject subScreen;        // 보조스크린용 변수

    public bool isForceScrollX = false;     // x쪽 강제 스크롤 여부
    public float forceScrollSpeedX = 0.5f;  // x쪽 이동 속도
    public bool isForceScrollY = false;     // y쪽 강제 스크롤 여부
    public float forceScrollSpeedY = 0.5f;  // y쪽 이동 속도

    void Update()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");     // 플레이어를 찾는다.
        if (player != null) // 플레이어를 찾았을때 카메라 설정
        {
            // 카메라가 플레이어를 따라가게 한다.
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;
            // 좌우로 동기
            if (isForceScrollX)
            {
                // 옆으로 강제 스크롤
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            }
            // 좌우 이동 제한  x = Mathf.Clamp(x, leftLimit, rightLimit);
            if (x < leftLimit)
            {
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }
            // 상하로 동기
            if (isForceScrollY)
            {
                // 아래로 강제 스크롤
                y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }
            // 상하 이동 제한
            if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }
            // 카메라 위치 설정
            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;

            // 보조 스크린을 다른 속도로 이동하게 한다.
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