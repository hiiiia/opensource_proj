using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dead : MonoBehaviour
{

    private void OnCollision(Collider col)
    {
        Destroy(gameObject);
    }

}
