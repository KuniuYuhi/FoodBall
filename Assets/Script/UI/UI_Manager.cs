using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_timeText;
    [SerializeField]
    TextMeshProUGUI m_rankNumText, m_rankText;
    [SerializeField]
    TextMeshProUGUI m_scoreText;

    [Header("------------------")]
    // �~�jUI
    [SerializeField, Tooltip("Cat->Duck->Penguin\nAI��ʗp�񋓌^�̏���")]
    GameObject[] m_enemyUI = new GameObject[3];

    GameManager m_gameManager;
    Camera m_mainCamera;
    GameObject[] m_enemys;
    Player m_player;

    void Awake()
    {
        // �Q�[���}�l�[�W���[��T��
        m_gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        // ���C���J�������擾
        m_mainCamera = Camera.main;
        // �v���C���[���擾
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // �G���܂Ƃ߂Ď擾
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy");

        // �ŏ���UI�𓧖���
        foreach (GameObject uiObj in m_enemyUI)
        {
            uiObj.SetActive(false);
        }

    }

    void Update()
    {
        // ���݂̎��Ԃ�\��
        int Minit = m_gameManager.GetMinit();
        float second = m_gameManager.GetSecond();
        if (m_gameManager.GetSecond() >= 60.0f)
        {
            Minit++;
            second = 0.0f;
        }

        string timeString = Minit.ToString("00");
        timeString += ":";
        timeString += Mathf.Floor(second).ToString("00");

        m_timeText.text = timeString;

        // �X�R�A�X�V
        m_scoreText.text = "" + m_player.GetEatFoods() + " pt";
        
        // �X�e�[�^�X�X�V
        StatusUpdate();
    }

    void StatusUpdate()
    {
        for(int i = 0; i < m_enemyUI.Length; i++)
        {
            // �^�[�Q�b�g�̍��W��T��
            Vector3 targetPos = Vector3.zero;
            AI targetAI = null;

            foreach(GameObject target in m_enemys)
            {
                targetAI = target.GetComponent<AI>();

                if ((int)targetAI.GetAICharactor() == i)
                {
                    // 1�Ԗڂ̎q���̍��W
                    targetPos = target.transform.GetChild(1).transform.position;
                    break;
                }
            }

            // ���������グ��
            targetPos.y += 14.0f;

            // �J��������^�[�Q�b�g�ւ̃x�N�g��
            Vector3 targetDir = targetPos - m_mainCamera.transform.position;

            // ���ς��g���ăJ�����O�����ǂ����𔻒�
            if(Vector3.Dot(m_mainCamera.transform.forward, targetDir) < 0)
            {
                m_enemyUI[i].SetActive(false);
            }
            else
            {
                m_enemyUI[i].SetActive(true);

                // �����ł��\���ؑ�
                if (targetDir.sqrMagnitude >= 80000.0f)
                {
                    m_enemyUI[i].SetActive(false);
                }
            }

            // ���W�X�V
            m_enemyUI[i].transform.position
                = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPos);

            // �\���X�V
            m_enemyUI[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "" + targetAI.GetEatFoods() + "pt";

        }

    }

}
