using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System;

public class AI : Actor
{
    public enum EnAICharacter
    {
        enCharacter_Cat,
        enCharacter_Duck,
        enCharacter_Penguin
    }

    [SerializeField, Header("AI�L�����N�^�[�̖��O")]
    EnAICharacter m_enAiCharacter;
    public EnAICharacter GetAICharactor()
    {
        return m_enAiCharacter;
    }

    [SerializeField, Header("�ړ����x")]
    float m_moveSpeed;

    [Header("�W�����v��")]
    public float m_jumpPower = 0.0f;

    NavMeshAgent m_navMeshAgent;
    public void SetNavMeshAgent(NavMeshAgent navMeshAgent)
    {
        m_navMeshAgent = navMeshAgent;
    }
    NavMeshAI m_navMeshAI;
    public void SetNavMeshAI(NavMeshAI navMeshAI)
    {
        m_navMeshAI = navMeshAI;
    }

    int m_nowIndex = 0;
    public void SetNowIndex(int index)
    {
        m_nowIndex = index;
    }

    // �������a�ۑ�
    float m_defRadius = 0.0f;

    // �ŏ��Ɏ��s
    override protected void GetStartInformation()
    {
        // �G�[�W�F���g�̑傫����ۑ�
        m_defRadius = m_navMeshAgent.radius;
    }

    private void FixedUpdate()
    {
        if (m_gameManager.GetGameMode() != GameManager.GameState.enGameMode_Play)
        {
            return;
        }

        MoveAI();
    }

    void Update()
    {
        if (m_gameManager.GetGameMode() != GameManager.GameState.enGameMode_Play)
        {
            return;
        }

        //�W�����v
        Jamp();

        // ���a�X�V
        m_navMeshAgent.radius = m_defRadius + (transform.localScale.x * 0.001f);

        m_navMeshAgent.nextPosition = transform.position;
    }

    public Vector3 GetNextPosition()
    {
        // �O�̂��߃G���[�h�~
        if (m_navMeshAgent.path.corners.Length <= m_nowIndex)
        {
            m_nowIndex = Mathf.Min(m_nowIndex, m_navMeshAgent.path.corners.Length - 1);
        }

        return m_navMeshAgent.path.corners[m_nowIndex];
    }

    //AI�̈ړ�����
    void MoveAI()
    {
        // �O�̂��߃G���[�h�~
        if (m_navMeshAgent.path.corners.Length <= m_nowIndex)
        {
            m_nowIndex = Mathf.Min(m_nowIndex, m_navMeshAgent.path.corners.Length - 1);
        }

        Vector3 targetPosition = m_navMeshAgent.path.corners[m_nowIndex];
        Vector3 nowPosition = transform.position;
        targetPosition.y = 0.0f;
        nowPosition.y = 0.0f;

        // �����`�F�b�N
        if (Vector3.Distance(nowPosition, targetPosition) < 20.0f)
        {
            // �����͍̂ŏI�ړI�n���ǂ���
            if (Vector3.Distance(nowPosition, m_navMeshAI.GetTargetPosition()) < 5.0f)
            {
                // �ŏI�ړI�n�ɂ��܂����I

            }
            else
            {
                // �ړI�n�ɂ����̂ōĐݒ�
                m_nowIndex++;
                // �G���[�h�~
                if(m_navMeshAgent.path.corners.Length < m_nowIndex)
                {
                    targetPosition = m_navMeshAgent.path.corners[m_nowIndex];
                }
                else
                {
                    m_nowIndex = m_navMeshAgent.path.corners.Length - 1;
                }
            }
        }

        //�ړI�n
        //Debug.Log(targetPosition);

        // �ړ�����
        Vector3 diff = targetPosition - transform.position;

        diff = diff * m_moveSpeed * Time.deltaTime;
        diff.y = 0.0f;
        //Debug.Log("Move:" + diff);
        m_rigidbody.AddForce(diff);
        m_navMeshAgent.nextPosition = transform.position;
    }

    void Jamp()
    {
        RaycastHit raycastHit;
       if(Physics.Raycast(transform.position, GetNextPosition(),out raycastHit, transform.localScale.x + 1.0f))
        {
            if(m_isJumpFlag == true)
            {
                m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);

                GameManager.PlaySE3D(
                    m_jump,
                    transform.position,
                    m_jumpMinRange,
                    m_jumpMaxRange,
                    m_jumpVolume
                    );
            }
        }


    }

    override protected void SetTarget()
    {
        m_navMeshAI.SetTarget();
    }
}
