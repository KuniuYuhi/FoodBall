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
   
    float m_counter = 0.0f;

    GameObject targetObject;
    
    [Header("�^�[�Q�b�g����������̃J�E���g�̏��")]
    public const int m_maxEvalCountLimit = 3;

    int m_addEvalValue = 100;

    //���ׂ�͈�
    public float m_range = 200.0f;

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

    void Update()
    {
        
        m_navMeshAgent.nextPosition = m_targetEnemy.transform.position;
    }

    public void SetTarget()
    {
         FindFoods();
         DicideTarget();
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
        //���΂炭���̏�ɂ����Ȃ�
        Vector3 a = transform.position - m_targetAI.GetNextPosition();
        float A = a.magnitude;

        if (A < 5.0f)
        {
            m_counter += Time.deltaTime;
            if (m_counter > 3.0f)
            {
                SetRamdomTarget();
            }
        }
        else
        {
            m_counter = 0.0f;
        }

        //�H�ו�������
        //�H�ו�����������
        if (FindFoods() == true)
        {
            //�^�[�Q�b�g�̍��W���擾
            GameObject target = DecideNearPosition();

            // �����ꏊ
            if(targetObject == target)
            {
                return;
            }

            targetObject = target;
            m_targetposition = targetObject.transform.position;
            m_navMeshAgent.enabled = true;
            m_navMeshAgent.SetDestination(m_targetposition);

            ResetIndex();

            Debug.Log(m_navMeshAgent.destination);

            
        }
        else
        {
            //�����_���ɍ��W���擾
            SetRamdomTarget();
        }
    }

    void ResetIndex()
    {
        // ���Z�b�g
        m_targetAI.SetNowIndex(0);

        m_nowTargetPosition = m_targetposition;
    }

    void SetRamdomTarget()
    {

        //�����_���ɍ��W���擾
        m_targetposition = DecideRamdomPosition();
        //�^�[�Q�b�g��ݒ�
        m_navMeshAgent.SetDestination(m_targetposition);

        
    }

    /// <summary>
    /// ��ԋ߂��H�ו��̍��W��Ԃ�
    /// </summary>
    /// <returns></returns>
    GameObject DecideNearPosition()
    {
        //�͈͓��̐H�ו��𒲂ׂ�

        for(int i=0;i< m_food.Length;i++)
        {
            m_eval[i] = 0;
        }

        float nearLength = m_range;

        int noRangeCount = 0;

        //��ԋ߂��H�ו��̔z��ԍ�
        nearPosNumber = 0;

        m_addEvalValue = 100;

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

            m_eval[amount] = 10 * foodPoint;
            

            //�͈͓��Ȃ�
            if(m_range> Length)
            {
                //�������g����ł��߂��Ȃ�
                if (nearLength > Length)
                {
                    //��ԋ߂��H�ו��̕]���l���グ�Ă���
                    m_eval[amount] += m_addEvalValue;
                    //���̕]���l��10�傫������
                    m_addEvalValue += 10;

                    //��ԋ߂��H�ו������ւ���
                    nearLength = Length;
                    nearPosNumber = amount;
                }
            }
            else
            {
                //�͈͓��ɂȂ��̂ŃJ�E���g�𑝂₷
                noRangeCount++;
            }

        }

        //�S�Ă̐H�ו����͈͓��ɂȂ�������
        if(noRangeCount== m_food.Length)
        {
            return null;
        }

        //�]���l�̍ő�l
        int Max = m_eval.Max();
        m_maxEvalNumber = 0;
        for (int amount = 0; amount < m_eval.Length; amount++)
        {
            if (m_eval[amount] == Max)
            {
                //���ꂪ��ԑ傫���]���l�̔z��̔ԍ�
                m_maxEvalNumber = amount;
                break;
            }
        }


        Debug.Log(m_eval[m_maxEvalNumber]);

        return m_food[m_maxEvalNumber];
    }

    /// <summary>
    /// �����_���ɍ��W��Ԃ�
    /// </summary>
    /// <returns></returns>
    Vector3 DecideRamdomPosition()
    {
        Vector3 ramdompos = UnityEngine.Random.insideUnitSphere;

        ramdompos *= 200.0f;

        return ramdompos;
    }

    //��ԋ߂��H�ו���菭����Ƀ|�C���g�������H�ו�������
    //�S�Ă̐H�ו��ɕ]���l
    //�߂��H�ו��قǕ]���l������


}
