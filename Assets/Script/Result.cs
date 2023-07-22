using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Result : MonoBehaviour
{
    enum Character
    {
        m_en_Rank1,
        m_en_Rank2,
        m_en_Rank3,
        m_en_Rank4,
    }

    private const int MAX_CHARACTER_NUMBER = 4;
    // Start is called before the first frame update
    //リザルトのスコア
    private int[] m_resultScore = new int[MAX_CHARACTER_NUMBER];

    public TextMeshProUGUI[] m_scoreTMP=new TextMeshProUGUI[MAX_CHARACTER_NUMBER];

    //挿入する画像
    public Sprite m_sheepSprite;
    public Sprite m_catSprite;
    public Sprite m_dugSprite;
    public Sprite m_penguinSprite;

    private GameData m_gameData;

    public GameObject[] m_resultCharacter = new GameObject[MAX_CHARACTER_NUMBER];

    [SerializeField]
    AudioClip m_enterSE;

    void Start()
    {
        m_gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        GameData.ActorData[] actorData = new GameData.ActorData[MAX_CHARACTER_NUMBER];
        actorData = m_gameData.GetActorData();
        GameData.ActorData swapData;

        for(int j=0;j< MAX_CHARACTER_NUMBER;j++)
        {
            for (int i = 0; i < MAX_CHARACTER_NUMBER-1; i++)
            {
                if (m_gameData.GetActorData()[i].Score < m_gameData.GetActorData()[i + 1].Score)
                {
                    swapData = m_gameData.GetActorData()[i + 1];
                    m_gameData.GetActorData()[i + 1] = m_gameData.GetActorData()[i];
                    m_gameData.GetActorData()[i] = swapData;
                }
            }
        }
  
        for(int i=0;i<MAX_CHARACTER_NUMBER;i++)
        {
            switch (m_gameData.GetActorData()[i].actorCharacter)
            {
                case GameData.ActorCharacter.enActorCharacter_Sheep:
                    m_resultCharacter[i].GetComponent<Image>().sprite = m_sheepSprite;
                    break;
                case GameData.ActorCharacter.enActorCharacter_Duck:
                    m_resultCharacter[i].GetComponent<Image>().sprite = m_dugSprite;
                    break;
                case GameData.ActorCharacter.enActorCharacter_Penguin:
                    m_resultCharacter[i].GetComponent<Image>().sprite = m_penguinSprite;
                    break;
                case GameData.ActorCharacter.enActorCharacter_Cat:
                    m_resultCharacter[i].GetComponent<Image>().sprite = m_catSprite;
                    break;
            }
        }
       
        int hoge = 0;
        for(int i=0;i<MAX_CHARACTER_NUMBER-1;i++)
        {
            for(int j=i+1;j<MAX_CHARACTER_NUMBER;j++)
            {
                if(actorData[i].Score<actorData[j].Score)
                {
                    hoge = actorData[i].Score;
                    actorData[i].Score = actorData[j].Score;
                    actorData[j].Score = hoge;
                }
            }
        }

        for(int i=0;i< MAX_CHARACTER_NUMBER;i++)
        {
            m_scoreTMP[i].text = ""+actorData[i].Score;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space))
        {
            // シーン切り替え
            GameManager.PlaySE(m_enterSE);
            Destroy(GameObject.FindGameObjectWithTag("GameData"));
            GameManager.SceneChange("Title");
        }
    }
}
