using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;            // 그림 표시용 변수
    public GameObject panel;                // 패널 표시용 변수
   
    public Text point_count;            // 점수용 텍스트
    // 시간 제한 추가
    public GameObject scoreBar;              // 시간 표시 이미지
    public GameObject scoreText;             // 시간 표시 텍스트
    public int score=0;
    public static GameManager instance = null;   // 게임 매니저용 변수

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }
    void Start()
    {
        scoreBar.SetActive(true);
        scoreText.SetActive(true);
    }
    void Update()
    {
            scoreText.GetComponent<Text>().text =score.ToString();

    }
    public void DestroyBrick()
    {
        score++;        // 점수 올리기
       
    }

}