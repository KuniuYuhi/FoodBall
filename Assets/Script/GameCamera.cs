using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameCamera : MonoBehaviour
{
    public float RotSpeed = 0.5f;
    public float RotUpLimit = 40.0f;
    public float RotDownLimit = -20.0f;
    public float CameraRange = 3.0f;
    public float CameraY_Up = 1.5f;


    private GameObject m_player;
    Player m_playerC;
    private float m_nowX_Rot = 0.0f;
    float m_defRange;

    void Start()
    {
        // Playerタグがついたオブジェクトを探す
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_playerC = m_player.GetComponent<Player>();

        // 初期X軸の回転量を保存
        m_nowX_Rot = transform.localEulerAngles.x;
        m_defRange = CameraRange;
    }


    void Update()
    {
        // 上下
        float Up_rot = Time.deltaTime * RotSpeed;
        if (Input.GetAxisRaw("Vertical2") != 0.0f)
        {
            Up_rot *= -Input.GetAxisRaw("Vertical2");
        }
        else
        {
            Up_rot = 0.0f;
        }

        // 上下角度制限
        m_nowX_Rot += Up_rot;
        if (m_nowX_Rot > RotUpLimit || m_nowX_Rot < RotDownLimit)
        {
            m_nowX_Rot = Mathf.Clamp(m_nowX_Rot, RotDownLimit, RotUpLimit);
            Up_rot = 0.0f;
        }
        transform.RotateAround(m_player.transform.position, this.transform.right, Up_rot);


        // 左右
        float Left_rot = Time.deltaTime * RotSpeed;
        if (Input.GetAxisRaw("Horizontal2") != 0.0f)
        {
            Left_rot *= Input.GetAxisRaw("Horizontal2");
        }
        else
        {
            Left_rot = 0.0f;
        }
        transform.RotateAround(m_player.transform.position, Vector3.up, Left_rot);

        // 座標計算
        // カメラの前方向を使って移動量を計算
        Vector3 cameraMove = transform.forward * -CameraRange;
        // カメラを少し持ち上げる
        cameraMove.y += CameraY_Up;
        // 座標設定
        transform.position = m_player.transform.position + cameraMove;

        // 距離を調整
        CameraRange = m_defRange + ((m_playerC.GetEatFoods()) * 0.8f);
    }
}