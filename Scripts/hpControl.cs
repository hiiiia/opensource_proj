using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class hpControl : MonoBehaviour
{
    int hp = 3;
    public GameObject hpImage;
    public Sprite life3Image;
    public Sprite life2Image;
    public Sprite life1Image;
    public Sprite life0Image;


    // Update is called once per frame
    void Update()
    {
        Debug.Log(hp);// 현재 애니메이션 재생
        UpdateHP();
    }
    void UpdateHP()
    {
        if(PlayerControll.gameState =="playing")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player!=null)
            {
                if(PlayerControll.hp!=hp)
                {
                    hp = PlayerControll.hp;
                    if(hp<=0)
                    {
                        hpImage.GetComponent<Image>().sprite = life0Image;
                    }
                    else if(hp ==1)
                    {
                        hpImage.GetComponent<Image>().sprite = life1Image;
                    }
                    else if (hp == 2)
                    {
                        
                        hpImage.GetComponent<Image>().sprite = life2Image;
                    }
                    else if (hp == 3)
                    {
                        
                        hpImage.GetComponent<Image>().sprite = life3Image;
                    }
                }
            }
        }    
    }
}
