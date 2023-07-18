using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : Actor
{
    NavMeshAgent m_navMesh;

    GameObject[] m_food;

    [SerializeField,Header("�^�[�Q�b�g�̍��W")]
    Vector3 m_targetposition = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        m_navMesh = GetComponent<NavMeshAgent>();

        FindFoods();

        DicideTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �X�e�[�W��̐H�ו�����������
    /// </summary>
    void FindFoods()
    {
        m_food = GameObject.FindGameObjectsWithTag("Food");
    }

    void DicideTarget()
    {
        //�H�ו�������
        //FindFoods();
        //�^�[�Q�b�g�̍��W���擾
        m_targetposition = DecideNearPosition();
        //�^�[�Q�b�g��ݒ�
        m_navMesh.destination = m_targetposition;
    }

    /// <summary>
    /// ��ԋ߂��H�ו��̍��W��Ԃ�
    /// </summary>
    /// <returns></returns>
    Vector3 DecideNearPosition()
    {
        //�H�ו��̍��W���擾
        Vector3 foodpos = m_food[0].transform.position;
        //���g����H�ו��Ɍ������x�N�g�����v�Z
        Vector3 diff = foodpos - transform.position;
        //�x�N�g���𒷂��ɕϊ�
        float nearLength = diff.magnitude;

        int nearPosNumber = 0;

        for(int amount=1;amount<m_food.Length;amount++)
        {
            //�H�ו��̍��W���擾
            Vector3 FoodPos = m_food[amount].transform.position;
            //���g����H�ו��Ɍ������x�N�g�����v�Z
            Vector3 Diff = FoodPos - transform.position;
            //�x�N�g���𒷂��ɕϊ�
            float Length = Diff.magnitude;

            //�����ł��߂��Ȃ�
            if(nearLength> Length)
            {
                nearLength = Length;
                
                nearPosNumber = amount;
            }
        }

        return m_food[nearPosNumber].transform.position;

    }
}
