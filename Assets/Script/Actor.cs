using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField,Header("食べた数")]
    int eatFoods = 0;

    // 拡大率
    const float m_scaleUp = 1.035f;

    // 初期座標
    protected Vector3 m_defPos;

    //キャッシュ
    protected Rigidbody m_rigidbody;
    protected GameObject m_gameCameraObj;
    protected GameManager m_gameManager;

    [SerializeField]
    protected float m_eatMinRange = 300.0f;
    [SerializeField]
    protected float m_eatMaxRange = 400.0f;
    [SerializeField]
    protected float m_jumpMinRange = 100.0f;
    [SerializeField]
    protected float m_jumpMaxRange = 200.0f;

    [SerializeField, Header("音量")]
    public int m_eatVolume = 13;
    [SerializeField]
    public int m_jumpVolume = 6;

    [SerializeField, Header("効果音")]
    protected AudioClip m_jump;
    [SerializeField]
    protected AudioClip m_eatFood;

    [SerializeField,Header("食べる音を2Dサウンドにする")]
    bool m_eatSE2D = false;

    protected bool m_isJumpFlag = true;
    virtual protected void GetStartInformation(){}

    bool m_getFoodFlag = false;

    public void SetGetFoodFlag(bool flag)
    {
        m_getFoodFlag = flag;
    }

    public bool GetGetFoodFlag()
    {
        return m_getFoodFlag;
    }

    private void Start()
    {
        //初期化
        m_rigidbody = GetComponent<Rigidbody>();
        m_gameCameraObj = Camera.main.gameObject;
        m_gameManager = GameObject.FindGameObjectWithTag("BallController").
            GetComponent<GameManager>();

        m_defPos = transform.position;

        GetStartInformation();
    }

    public int GetEatFoods()
    {
        return eatFoods;
    }

    protected void SizeUp(int point)
    {
        for (int i = 0; i < point; i++)
        {
            if (transform.localScale.x > 300.0f)
            {
                return;
            }

            transform.localScale *= m_scaleUp;
        }
        
    }

    protected void SizeDown()
    {
        transform.localScale *= m_scaleUp;
    }

    protected void isFalling()
    {

    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Food"))
        {
            Food m_food=other.GetComponent<Food>();
            eatFoods+= m_food.GetPoint();
            m_rigidbody.mass = (1.0f + (eatFoods * 0.01f));
            m_rigidbody.mass = Mathf.Min(m_rigidbody.mass, 1.4f);
            SizeUp(m_food.GetPoint());

            Destroy(other.gameObject);

            SetTarget();

            SetGetFoodFlag(true);

            if (m_eatSE2D)
            {
                GameManager.PlaySE(m_eatFood, m_eatVolume);
            }
            else
            {
                GameManager.PlaySE3D(
                    m_eatFood,
                    transform.position,
                    m_eatMinRange,
                    m_eatMaxRange,
                    m_eatVolume
                    );
            }

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("BallPlayer"))
        {
            Vector3 Backlash = collision.gameObject.transform.position - transform.position;
            Backlash = Backlash.normalized;
            Backlash *= 30.0f;
            Backlash.y = 20.0f;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(Backlash, ForceMode.Impulse);
            m_rigidbody.AddForce(-Backlash, ForceMode.Impulse);
        }
    }
    virtual protected void SetTarget(){}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_isJumpFlag = true;
            return;
        }

        if (collision.gameObject.CompareTag("JumpGround"))
        {
            m_isJumpFlag = true;
            return;
        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_isJumpFlag = false;
            return;
        }

        if (collision.gameObject.CompareTag("JumpGround"))
        {
            m_isJumpFlag = false;
            return;
        }
    }

}
