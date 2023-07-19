using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System;
using System.Linq;

public class NavMeshAI : MonoBehaviour
{
    [SerializeField]
    GameObject m_targetEnemy;
    NavMeshAgent m_navMeshAgent;
    AI m_targetAI;

    Vector3 m_nowTargetPosition = Vector3.zero;
    public Vector3 GetTargetPosition()
    {
        return m_nowTargetPosition;
    }

    GameObject[] m_food;

    [SerializeField, Header("�^�[�Q�b�g�̍��W")]
    Vector3 m_targetposition = Vector3.zero;

    [Header("��������^�C�~���O�̕b��")]
    public int m_findTiming = 5;

    //�]���l
    int[] m_eval;

    int nearPosNumber = 0;

    int m_maxEvalNumber = 0;

    //List<int>[] eval = new List<int>();

    //nearPosNumber�p�ӂ���

    void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_targetAI = m_targetEnemy.GetComponent<AI>();
        m_targetAI.SetNavMeshAgent(m_navMeshAgent);
        m_targetAI.SetNavMeshAI(this);

        transform.position = m_targetEnemy.transform.position;
        m_navMeshAgent.transform.position= m_targetEnemy.transform.position;

        //�H�ו�����������^�C�~���O��ݒ�
        FindTiming();

        FindFoods();
        DicideTarget();
    }

    //private void FixedUpdate()
    //{
     
    //}

    void Update()
    {
        

        m_navMeshAgent.nextPosition = m_targetEnemy.transform.position;
    }

    /// <summary>
    /// ���Ԋu���ƂɐH�ו����������A�^�[�Q�b�g��ݒ肷��
    /// </summary>
    void FindTiming()
    {
        Observable.Interval(TimeSpan.FromSeconds(m_findTiming))
          .Subscribe(_ => DicideTarget()).AddTo(this);
    }

    /// <summary>
    /// �X�e�[�W��̐H�ו�����������
    /// </summary>
    bool FindFoods()
    {
        m_food = GameObject.FindGameObjectsWithTag("Food");

        //�H�ו����Ȃ�������
        if (m_food.Length == 0)
        {
            return false;
        }



        //�H�ו��̐������z���p�ӂ���
        int size = m_food.Length;
        m_eval = new int[size];

        Debug.Log(m_eval.Length);

        //�H�ו���������
        return true;
    }

    /// <summary>
    /// �^�[�Q�b�g������
    /// </summary>
    void DicideTarget()
    {
        //�H�ו�������
        //�H�ו�����������
        if (FindFoods() == true)
        {
            //���g�����ԋ߂��H�ו������߂�
            DecideNearPosition();
            //�]���l�̍ő�l
            int Max = m_eval.Max();
            m_maxEvalNumber = 0;
            for(int amount=0;amount<m_eval.Length;amount++)
            {
                if(m_eval[amount]== Max)
                {
                    //���ꂪ��ԑ傫���]���l�̔z��̔ԍ�
                    m_maxEvalNumber = amount;
                    break;
                }
            }

           
            //�^�[�Q�b�g�̍��W���擾
            m_targetposition = m_food[m_maxEvalNumber].transform.position;
            //�^�[�Q�b�g��ݒ�
            m_navMeshAgent.destination = m_targetposition;
        }
        else
        {
            //�����_���ɍ��W���擾
            m_targetposition = DecideRamdomPosition();
            //�^�[�Q�b�g��ݒ�
            m_navMeshAgent.destination = m_targetposition;
        }

        if (m_nowTargetPosition != m_targetposition)
        {
            // ���Z�b�g
            m_targetAI.SetNowIndex(0);
        }
        m_nowTargetPosition = m_targetposition;
    }

    /// <summary>
    /// ��ԋ߂��H�ו��̍��W��Ԃ�
    /// </summary>
    /// <returns></returns>
    Vector3 DecideNearPosition()
    {
        //�͈͓��̐H�ו��𒲂ׂ�

        //�H�ו��̍��W���擾
        Vector3 foodpos = m_food[0].transform.position;
        //���g����H�ו��Ɍ������x�N�g�����v�Z
        Vector3 diff = foodpos - transform.position;
        //�x�N�g���𒷂��ɕϊ�
        float nearLength = diff.magnitude;

        //��ԋ߂��H�ו��̔z��ԍ�
        nearPosNumber = 0;

        for (int amount = 1; amount < m_food.Length; amount++)
        {
            //�H�ו��̍��W���擾
            Vector3 FoodPos = m_food[amount].transform.position;
            //���g����H�ו��Ɍ������x�N�g�����v�Z
            Vector3 Diff = FoodPos - transform.position;
            //�x�N�g���𒷂��ɕϊ�
            float Length = Diff.magnitude;

            //�t�[�h�̃|�C���g
            int foodPoint = m_food[amount].GetComponent<Food>().GetPoint();

            m_eval[amount] = 50 * foodPoint;

           
            //�������g����ł��߂��Ȃ�
            if (nearLength > Length)
            {
                //���Ɉ�ԋ߂��H�ו��̕]���l�������Ă���Ȃ�
                if (m_eval[nearPosNumber] > 0)
                {
                    //�܂���ԋ߂��H�ו��̕]���l������
                    m_eval[amount] += m_eval[nearPosNumber];
                }

                //��ԋ߂��H�ו��̕]���l���グ�Ă���
                //m_eval[amount] += 100;

                //��ԋ߂��H�ו������ւ���
                nearLength = Length;

                nearPosNumber = amount;


            }
            else
            {
                //��ԋ߂��H�ו��̕]���l���グ�Ă���
                m_eval[nearPosNumber] += 100;
            }
        }

        Debug.Log(m_eval[nearPosNumber]);

        return m_food[nearPosNumber].transform.position;
    }

    /// <summary>
    /// �����_���ɍ��W��Ԃ�
    /// </summary>
    /// <returns></returns>
    Vector3 DecideRamdomPosition()
    {
        Vector3 ramdompos = UnityEngine.Random.insideUnitSphere;

        ramdompos *= 20.0f;

        return ramdompos;
    }

    //��ԋ߂��H�ו���菭����Ƀ|�C���g�������H�ו�������
    //�S�Ă̐H�ו��ɕ]���l
    //�߂��H�ו��قǕ]���l������


}
