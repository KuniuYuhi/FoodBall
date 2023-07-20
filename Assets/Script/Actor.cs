using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField,Header("食べ物を食べた数")]
    int eatFoods = 0;

    [SerializeField, Header("自身のサイズ")]
    Vector3 size = Vector3.one;

    [SerializeField, Header("拡大する量")]
    const float m_scaleUp = 1.05f;
    //キャッシュ
    protected Rigidbody m_rigidbody;
    protected GameObject m_gameCameraObj;
    //ジャンプ可能かどうか
    protected bool m_isJumpFlag = true;
    virtual protected void GetStartInformation(){}

    //食べ物を獲得したかどうか。AI用
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
        //必要な情報を取得
        m_rigidbody = GetComponent<Rigidbody>();
        m_gameCameraObj = Camera.main.gameObject;
        GetStartInformation();
    }


    /// <summary>
    /// 食べた食べ物の数を返す
    /// </summary>
    /// <returns></returns>
    public int GetEatFoods()
    {
        return eatFoods;
    }

    /// <summary>
    /// サイズを大きくする
    /// </summary>
    protected void SizeUp()
    {
        if(transform.localScale.x<transform.localScale.x*2.0f)
        {
            transform.localScale *= m_scaleUp;
        }
        
    }

    /// <summary>
    /// サイズを小さくする
    /// </summary>
    protected void SizeDown()
    {
        transform.localScale *= m_scaleUp;
    }

    protected void isFalling()
    {

    }


    protected void OnTriggerEnter(Collider other)
    {
        //もし自身と衝突したらオブジェクトのタグが食べ物だったら
        if(other.CompareTag("Food"))
        {
            Food m_food=other.GetComponent<Food>();
            //食べた量を加算する
            eatFoods+= m_food.GetPoint();
            m_rigidbody.mass = (1.0f + (eatFoods * 0.05f));
            m_rigidbody.mass = Mathf.Min(m_rigidbody.mass, 2.5f);
            //モデルを大きくする
            SizeUp();

            //食べ物を消す
            Destroy(other.gameObject);

            //食べ物を食べたのですぐに新しいターゲットを決める
            SetTarget();

            //食べ物を食べたのでtrueにする
            SetGetFoodFlag(true);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Vector3 Backlash = collision.gameObject.transform.position - transform.position;
            Backlash.y += 50.0f;

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
