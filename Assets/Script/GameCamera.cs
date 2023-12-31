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
    GameManager m_gameManager;
    Player m_playerC;
    private float m_nowX_Rot = 0.0f;
    float m_defRange;

    [SerializeField]
    bool m_springCamera;

    void Start()
    {
        // Playerタグがついたオブジェクトを探す
        m_player = GameObject.FindGameObjectWithTag("BallPlayer");
        m_playerC = m_player.GetComponent<Player>();
        m_gameManager = GameObject.FindGameObjectWithTag("BallController").
            GetComponent<GameManager>();

        // 初期X軸の回転量を保存
        m_nowX_Rot = transform.localEulerAngles.x;
        m_defRange = CameraRange;

        // 初期化
        CameraUpdate();

    }


    void Update()
    {
        if (m_gameManager.GetGameMode() != GameManager.GameState.enGameMode_Play)
        {
            CameraSet();

            return;
        }

        CameraUpdate();
    }

    void CameraUpdate()
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

        CameraSet();

    }

    void CameraSet()
    {
        // 座標計算
        // カメラの前方向を使って移動量を計算
        Vector3 cameraMove = transform.forward * -CameraRange;
        // カメラを少し持ち上げる
        cameraMove.y += CameraY_Up;
        // 座標設定
        transform.position = m_player.transform.position + cameraMove;

        // 距離を調整
        CameraRange = m_defRange + ((m_playerC.transform.localScale.x) * 1.0f);

        // 埋まり対策
        if (m_springCamera)
        {
            transform.position = WallCheck(m_player.transform.position, transform.position);
        }
    }

    Vector3 WallCheck(Vector3 targetPosition, Vector3 desiredPosition)
    {
        RaycastHit wallHit;
        Vector3 offset = targetPosition - desiredPosition;
        offset = offset.normalized;
        offset *= 10.0f;

        if (Physics.Raycast(targetPosition, desiredPosition - targetPosition,
            out wallHit, Vector3.Distance(targetPosition, desiredPosition)))
        {
            return wallHit.point + offset;
        }
        else
        {
            return desiredPosition;
        }
    }


}