using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;            // �׸� ǥ�ÿ� ����
    public GameObject panel;                // �г� ǥ�ÿ� ����
   
    public Text point_count;            // ������ �ؽ�Ʈ
    // �ð� ���� �߰�
    public GameObject scoreBar;              // �ð� ǥ�� �̹���
    public GameObject scoreText;             // �ð� ǥ�� �ؽ�Ʈ
    public int score=0;
    public static GameManager instance = null;   // ���� �Ŵ����� ����

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
        score++;        // ���� �ø���
       
    }

}