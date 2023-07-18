using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : Actor
{
    NavMeshAgent m_navMesh;

    GameObject[] m_food;

    [SerializeField,Header("ターゲットの座標")]
    Vector3 m_targetposition = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        m_navMesh = GetComponent<NavMeshAgent>();

        FindFoods();

        DicideTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ステージ上の食べ物を検索する
    /// </summary>
    void FindFoods()
    {
        m_food = GameObject.FindGameObjectsWithTag("Food");
    }

    void DicideTarget()
    {
        //食べ物を検索
        //FindFoods();
        //ターゲットの座標を取得
        m_targetposition = DecideNearPosition();
        //ターゲットを設定
        m_navMesh.destination = m_targetposition;
    }

    /// <summary>
    /// 一番近い食べ物の座標を返す
    /// </summary>
    /// <returns></returns>
    Vector3 DecideNearPosition()
    {
        //食べ物の座標を取得
        Vector3 foodpos = m_food[0].transform.position;
        //自身から食べ物に向かうベクトルを計算
        Vector3 diff = foodpos - transform.position;
        //ベクトルを長さに変換
        float nearLength = diff.magnitude;

        int nearPosNumber = 0;

        for(int amount=1;amount<m_food.Length;amount++)
        {
            //食べ物の座標を取得
            Vector3 FoodPos = m_food[amount].transform.position;
            //自身から食べ物に向かうベクトルを計算
            Vector3 Diff = FoodPos - transform.position;
            //ベクトルを長さに変換
            float Length = Diff.magnitude;

            //もし最も近いなら
            if(nearLength> Length)
            {
                nearLength = Length;
                
                nearPosNumber = amount;
            }
        }

        return m_food[nearPosNumber].transform.position;

    }
}
