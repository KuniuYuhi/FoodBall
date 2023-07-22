using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : Actor
{
    public enum EnAICharacter
    {
        enCharacter_Cat,
        enCharacter_Duck,
        enCharacter_Penguin
    }

    [SerializeField, Header("AIの種類")]
    EnAICharacter m_enAiCharacter;
    public EnAICharacter GetAICharactor()
    {
        return m_enAiCharacter;
    }

    [SerializeField, Header("移動速度")]
    float m_moveSpeed;

    [Header("ジャンプ力")]
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

    // デフォルトの半径
    float m_defRadius = 0.0f;

    // 初回実行
    override protected void GetStartInformation()
    {
        // 半径を記憶しておく
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

        //ジャンプ
        Jamp();

        // 半径を更新
        m_navMeshAgent.radius = m_defRadius + (transform.localScale.x * 0.001f);

        m_navMeshAgent.nextPosition = transform.position;

        // 落下対策
        if (transform.position.y <= 650.0f)
        {
            transform.position = m_defPos;
        }
    }

    public Vector3 GetNextPosition()
    {
        // 次の座標を返す
        if (m_navMeshAgent.path.corners.Length <= m_nowIndex)
        {
            m_nowIndex = Mathf.Min(m_nowIndex, m_navMeshAgent.path.corners.Length - 1);
        }

        return m_navMeshAgent.path.corners[m_nowIndex];
    }

    //AIの移動
    void MoveAI()
    {
        // 念のため
        if (m_navMeshAgent.path.corners.Length <= m_nowIndex)
        {
            m_nowIndex = Mathf.Min(m_nowIndex, m_navMeshAgent.path.corners.Length - 1);
        }

        Vector3 targetPosition = m_navMeshAgent.path.corners[m_nowIndex];
        Vector3 nowPosition = transform.GetChild(1).transform.position;     // Targetの座標
        targetPosition.y = 0.0f;
        nowPosition.y = 0.0f;

        // 距離チェック
        if (Vector3.Distance(nowPosition, targetPosition) < 20.0f)
        {
            if (Vector3.Distance(nowPosition, m_navMeshAI.GetTargetPosition()) < 5.0f)
            {
                // 最終目的地についた

            }
            else
            {
                // 次の座標を見る
                m_nowIndex++;
                // エラー防止
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

        // 距離
        Vector3 diff = targetPosition - transform.GetChild(1).transform.position;

        diff = diff * m_moveSpeed * Time.deltaTime;
        diff.y = 0.0f;
        //Debug.Log("Move:" + diff);
        m_rigidbody.AddForce(diff);
        m_navMeshAgent.nextPosition = transform.GetChild(1).transform.position;
    }

    void Jamp()
    {
        RaycastHit raycastHit;
       if(Physics.Raycast(transform.position, GetNextPosition(),out raycastHit, transform.localScale.x + 1.0f))
        {
            if(m_isJumpFlag == true)
            {
                m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);

                //GameManager.PlaySE3D(
                //    m_jump,
                //    transform.position,
                //    m_jumpMinRange,
                //    m_jumpMaxRange,
                //    m_jumpVolume
                //    );
            }
        }


    }

    override protected void SetTarget()
    {
        m_navMeshAI.SetTarget();
    }
}
