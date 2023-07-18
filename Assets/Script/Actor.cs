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
    const float m_scaleUp = 1.01f;
    //キャッシュ
    protected Rigidbody m_rigidbody;
    protected GameObject m_gameCameraObj;


    virtual protected void GetStartInformation(){}

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
        transform.localScale *= m_scaleUp;
    }

    /// <summary>
    /// サイズを小さくする
    /// </summary>
    protected void SizeDown()
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

            //モデルを大きくする
            SizeUp();

            //食べ物を消す
            Destroy(other.gameObject);
        }
    }

}
