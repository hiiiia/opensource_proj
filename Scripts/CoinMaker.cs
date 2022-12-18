using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinMaker : MonoBehaviour
{
    public GameObject coin;

    int x;
    float yPos;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<15;i++)
        {
            x = Random.Range(0, 3);
            switch (x)
            {
                case 0:
                    yPos = -4.2f; break;
                case 1:
                    yPos = -0.5f; break;
                case 2:
                    yPos = 4f; break;
            }
            Vector3 holePos1 = new Vector3(i*3.0f, yPos, 0f);
      ;
            GameObject obj1 = Instantiate(coin, holePos1, Quaternion.identity);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
