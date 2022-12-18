using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{


    void OnCollisionEnter(Collision col)
    {
 
        // 충돌한 물체의 Tag를 확인한다.
        if (col.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            return;
        }
        GameManager.instance.DestroyBrick();
        Destroy(gameObject);


    }
}
