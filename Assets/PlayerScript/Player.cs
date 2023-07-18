using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    //パラメータ
    //プレイヤーの移動速度
    [SerializeField,Header("プレイヤーの移動速度")]
    float m_moveSpeed = 0.0f;

   

    //プレイヤーの移動速度設定
    public void SetMoveSpeed(float speed)
    {
        m_moveSpeed = speed;
    }

    //プレイヤーの移動速度の取得
    public float GetMoveSpeed()
    {
        return m_moveSpeed;
    }

    protected override void GetStartInformation()
    {
        base.GetStartInformation();

    }

    private void FixedUpdate()
    {
        Move();
    }

    //移動処理
    private void Move()
    {
        Vector3 m_playerMove = Vector3.zero;
        Vector3 m_stickL = Vector3.zero;
        m_stickL.z = Input.GetAxis("Vertical");
        m_stickL.x = Input.GetAxis("Horizontal");

        //移動速度に上記で計算したベクトルを加算する
        m_playerMove += m_stickL;

        //プレイヤーの速度を設定することで移動できる
        m_playerMove = (m_playerMove * m_moveSpeed * Time.deltaTime);
        m_playerMove.y = 0.0f;
        m_rigidbody.AddForce(m_playerMove);
        Debug.Log(m_playerMove);

    }
}
