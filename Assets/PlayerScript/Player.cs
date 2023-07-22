using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField,Header("移動速度")]
    float m_moveSpeed = 0.0f;
    [SerializeField, Header("ジャンプ力")]
    float m_jumpPower = 0.0f;

    GameObject m_mainCamera;

    //移動速度を設定
    public void SetMoveSpeed(float speed)
    {
        m_moveSpeed = speed;
    }

    //�v���C���[�̈ړ����x�̎擾
    public float GetMoveSpeed()
    {
        return m_moveSpeed;
    }

    //ジャンプ力を設定
    public void SetJumpPower(float power)
    {
        m_jumpPower = power;
    }

    //ジャンプ力を取得
    public float GetJumpPower()
    {
        return m_jumpPower;
    }

    protected override void GetStartInformation()
    {
        base.GetStartInformation();

        // カメラを記憶
        m_mainCamera = Camera.main.gameObject;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Jump();

        // 落下対策
        if (transform.position.y <= 650.0f)
        {
            transform.position = m_defPos;
        }
    }
    //移動
    private void Move()
    {
        if (m_gameManager.GetGameMode() != GameManager.GameState.enGameMode_Play)
        {
            return;
        }

        // カメラを考慮した移動
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
        m_playerMove += right + forward;

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
}
