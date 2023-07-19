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

    [Header("------------------")]
    // �~�jUI
    [SerializeField, Tooltip("Cat->Duck->Penguin\nAI��ʗp�񋓌^�̏���")]
    GameObject[] m_enemyUI = new GameObject[3];
    [SerializeField]
    RectTransform m_parentUI;

    GameManager m_gameManager;
    Camera m_mainCamera;
    GameObject[] m_enemys;

    void Awake()
    {
        // �Q�[���}�l�[�W���[��T��
        m_gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        // ���C���J�������擾
        m_mainCamera = Camera.main;
        // �G���܂Ƃ߂Ď擾
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy");

        // �ŏ���UI�𓧖���
        foreach(GameObject uiObj in m_enemyUI)
        {
            uiObj.SetActive(false);
        }

    }

    void Update()
    {
        // ���݂̎��Ԃ�\��
        string timeString = m_gameManager.GetMinit().ToString("00");
        timeString += ":";
        timeString += Mathf.Floor(m_gameManager.GetSecond()).ToString("00");

        m_timeText.text = timeString;

        // �X�e�[�^�X�X�V
        StatusUpdate();
    }

    void StatusUpdate()
    {
        // �Ȃ��������Ȃ��̂Ŏ~�߂�
        return;

        for(int i = 0; i < m_enemyUI.Length; i++)
        {
            // �^�[�Q�b�g�̍��W��T��
            Vector3 targetPos = Vector3.zero;

            foreach(GameObject target in m_enemys)
            {
                if ((int)target.GetComponent<AI>().GetAICharactor() == i)
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
            bool isFront = Vector3.Dot(m_mainCamera.transform.forward, targetDir) > 0;

            // �����ł��\���ؑ�
            if (targetDir.sqrMagnitude >= 80000.0f)
            {
                isFront = false;
            }

            // �J�����O���Ȃ�UI�\���A����Ȃ��\��
            m_enemyUI[i].SetActive(isFront);

            if (isFront == false)
            {
                return;
            }

            // �I�u�W�F�N�g�̃��[���h���W���X�N���[�����W�ϊ�
            Vector3 targetScreenPos = m_mainCamera.WorldToScreenPoint(targetPos);

            // �X�N���[�����W�ϊ���UI���[�J�����W�ϊ�
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_parentUI,
                targetScreenPos,
                null,
                out Vector2 uiLocalPos
            );

            // RectTransform�̃��[�J�����W���X�V
            m_enemyUI[i].GetComponent<RectTransform>().localPosition = targetScreenPos;

        }

    }

}
