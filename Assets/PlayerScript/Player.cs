using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    //パラメータ
    //プレイヤーの移動速度
    float m_speed = 0.0f;

    //キャッシュ
    Rigidbody m_rigidbody;
    GameObject m_gameCameraObj;

    //プレイヤーの移動速度設定
    [SerializeField]
    void SetMoveSpeed(float speed)
    {
        m_speed = speed;
    }

    //プレイヤーの移動速度の取得
    [SerializeField]
    float GetMoveSpeed()
    {
        return m_speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        //必要な情報を取得
        m_rigidbody = GetComponent<Rigidbody>();
        m_gameCameraObj = Camera.main.gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 m_playerMove = Vector3.zero;
        Vector3 m_stickL = Vector3.zero;
        m_stickL.z = Input.GetAxis("Vertical");
        m_stickL.x = Input.GetAxis("Horizontal");

        Vector3 m_forward = m_gameCameraObj.transform.forward;
        Vector3 m_right = m_gameCameraObj.transform.right;
        m_forward.y = 0.0f;
        m_right.y = 0.0f;

        m_right *= m_stickL.x;
        m_forward *= m_stickL.z;

        //移動速度に上記で計算したベクトルを加算する
        Player
    }

    // Update is called once per frame
    void Update()
    {
       if()
    }
}
