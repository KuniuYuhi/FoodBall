using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    //パラメータ
    //プレイヤーの移動速度
    [SerializeField,Header("プレイヤーの移動速度")]
    float m_moveSpeed = 0.0f;
    [SerializeField, Header("プレイヤーのジャンプ力")]
    float m_jumpPower = 0.0f;

    


    GameObject m_mainCamera;

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

    //プレイヤーのジャンプ量設定
    public void SetJumpPower(float power)
    {
        m_jumpPower = power;
    }

    //プレイヤーのジャンプ量の取得
    public float GetJumpPower()
    {
        return m_jumpPower;
    }




    protected override void GetStartInformation()
    {
        base.GetStartInformation();

        // メインカメラのゲームオブジェクトを取得する
        m_mainCamera = Camera.main.gameObject;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Jump();
    }
    //移動処理
    private void Move()
    {
        if (m_gameManager.GetGameMode() != GameManager.GameState.enGameMode_Play)
        {
            return;
        }

        Vector3 m_playerMove = Vector3.zero;
        Vector3 m_stickL = Vector3.zero;
        m_stickL.z = Input.GetAxis("Vertical");
        m_stickL.x = Input.GetAxis("Horizontal");

        Vector3 forward = m_mainCamera.transform.forward;
        Vector3 right = m_mainCamera.transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        right *= m_stickL.x;
        forward *= m_stickL.z;
        // 移動速度に上記で計算したベクトルを加算する
        m_playerMove += right + forward;

        //プレイヤーの速度を設定することで移動できる
        m_playerMove = (m_playerMove * m_moveSpeed * Time.deltaTime);
        m_playerMove.y = 0.0f;
        m_rigidbody.AddForce(m_playerMove);
        //Debug.Log(m_playerMove);

    }

    private void Jump()
    {
        if (m_gameManager.GetGameMode() != GameManager.GameState.enGameMode_Play)
        {
            return;
        }

        if (Input.GetKeyDown("joystick button 0")||Input.GetKeyDown(KeyCode.Space))
        {
            if(m_isJumpFlag)
            {
                m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
            }
        }
    }
}
