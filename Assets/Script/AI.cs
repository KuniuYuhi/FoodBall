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

    [SerializeField, Header("AIキャラクターの名前")]
    EnAICharacter m_enAiCharacter;
    public EnAICharacter GetAICharactor()
    {
        return m_enAiCharacter;
    }

    [SerializeField, Header("移動速度")]
    float m_moveSpeed;

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

    // 初期半径保存
    float m_defRadius = 0.0f;

    // 最初に実行
    override protected void GetStartInformation()
    {
        // エージェントの大きさを保存
        m_defRadius = m_navMeshAgent.radius;
    }

    void Update()
    {
        MoveAI();

        // 半径更新
        m_navMeshAgent.radius = m_defRadius * transform.localScale.x;
    }

    //AIの移動処理
    void MoveAI()
    {
        // 念のためエラー防止
        if (m_navMeshAgent.path.corners.Length <= m_nowIndex)
        {
            return;
        }

        Vector3 targetPosition = m_navMeshAgent.path.corners[m_nowIndex];
        Vector3 nowPosition = transform.position;
        targetPosition.y = 0.0f;
        nowPosition.y = 0.0f;

        // 距離チェック
        if (Vector3.Distance(nowPosition, targetPosition) < 5.0f)
        {
            // ついたのは最終目的地かどうか
            if (Vector3.Distance(nowPosition, m_navMeshAI.GetTargetPosition()) < 5.0f)
            {
                // 最終目的地につきました！

                return;
            }
            else
            {
                // 目的地についたので再設定
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

        //目的地
        //Debug.Log(targetPosition);

        // 移動処理
        Vector3 diff = targetPosition - transform.position;

        diff = diff * m_moveSpeed * Time.deltaTime;
        diff.y = 0.0f;
        //Debug.Log("Move:" + diff);
        m_rigidbody.AddForce(diff);
    }

}
