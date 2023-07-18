using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System;

public class AI : Actor
{
    NavMeshAgent m_navMesh;

    GameObject[] m_food;

    [SerializeField,Header("ターゲットの座標")]
    Vector3 m_targetposition = Vector3.zero;

    [Header("検索するタイミングの秒数")]
    public int m_findTiming = 5;

    //評価値
    int[] m_eval;

    int nearPosNumber = 0;

    //List<int>[] eval = new List<int>();

    //nearPosNumber用意する

    // Start is called before the first frame update
    void Start()
    {
        m_navMesh = GetComponent<NavMeshAgent>();

        //食べ物を検索するタイミングを設定
        FindTiming();

        FindFoods();
        DicideTarget();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    /// <summary>
    /// 一定間隔ごとに食べ物を検索し、ターゲットを設定する
    /// </summary>
    void FindTiming()
    {
        Observable.Interval(TimeSpan.FromSeconds(m_findTiming))
          .Subscribe(_ => DicideTarget()).AddTo(this);
    }

    /// <summary>
    /// ステージ上の食べ物を検索する
    /// </summary>
    bool FindFoods()
    {
        m_food = GameObject.FindGameObjectsWithTag("Food");

        //食べ物がなかったら
        if(m_food.Length==0)
        {
            return false;
        }

        

        //食べ物の数だけ配列を用意する
        int size = m_food.Length;
        m_eval = new int[size];

        Debug.Log(m_eval.Length);

        //食べ物があった
        return true;
    }

    /// <summary>
    /// ターゲットを決定
    /// </summary>
    void DicideTarget()
    {
        //食べ物を検索
        //食べ物があったら
        if(FindFoods()==true)
        {
            //ターゲットの座標を取得
            m_targetposition = DecideNearPosition();
            //ターゲットを設定
            m_navMesh.destination = m_targetposition;
        }
        else
        {
            //ランダムに座標を取得
            m_targetposition = DecideRamdomPosition();
            //ターゲットを設定
            m_navMesh.destination = m_targetposition;
        }
       
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

        //一番近い食べ物の配列番号
        nearPosNumber = 0;

        for(int amount=1;amount<m_food.Length;amount++)
        {
            //食べ物の座標を取得
            Vector3 FoodPos = m_food[amount].transform.position;
            //自身から食べ物に向かうベクトルを計算
            Vector3 Diff = FoodPos - transform.position;
            //ベクトルを長さに変換
            float Length = Diff.magnitude;

            

            //もし自身から最も近いなら
            if (nearLength> Length)
            {
                //既に一番近い食べ物の評価値が入っているなら
                if(m_eval[nearPosNumber]>0)
                {
                    //まず一番近い食べ物の評価値をたす
                    m_eval[amount] += m_eval[nearPosNumber];
                }

                //一番近い食べ物の評価値を上げていく
                m_eval[amount] += 100;

                //一番近い食べ物を入れ替える
                nearLength = Length;
                
                nearPosNumber = amount;

                
            }
            else
            {
                //一番近い食べ物の評価値を上げていく
                m_eval[nearPosNumber] += 100;
            }
        }

        Debug.Log(m_eval[nearPosNumber]);

        return m_food[nearPosNumber].transform.position;
    }

    /// <summary>
    /// ランダムに座標を返す
    /// </summary>
    /// <returns></returns>
    Vector3 DecideRamdomPosition()
    {
        Vector3 ramdompos = UnityEngine.Random.insideUnitSphere;

        ramdompos *= 20.0f;

        return ramdompos;
    }

    //一番近い食べ物より少し先にポイントが高い食べ物がある
    //全ての食べ物に評価値
    //近い食べ物ほど評価値が高い

}
