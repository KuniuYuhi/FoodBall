using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System;
using System.Linq;

public class NavMeshAI : MonoBehaviour
{
    [SerializeField]
    GameObject m_targetEnemy;
    NavMeshAgent m_navMeshAgent;
    AI m_targetAI;

    Vector3 m_nowTargetPosition = Vector3.zero;
    public Vector3 GetTargetPosition()
    {
        return m_nowTargetPosition;
    }

    GameObject[] m_food;

    [SerializeField, Header("ターゲットの座標")]
    Vector3 m_targetposition = Vector3.zero;

    [Header("検索するタイミングの秒数")]
    public int m_findTiming = 5;

    //評価値
    int[] m_eval;

    int nearPosNumber = 0;

    int m_maxEvalNumber = 0;

    int m_addEvalValue = 100;

    Vector3 m_oldDestination = Vector3.zero;

    //List<int>[] eval = new List<int>();

    //nearPosNumber用意する

    void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_targetAI = m_targetEnemy.GetComponent<AI>();
        m_targetAI.SetNavMeshAgent(m_navMeshAgent);
        m_targetAI.SetNavMeshAI(this);

        transform.position = m_targetEnemy.transform.position;
        m_navMeshAgent.transform.position= m_targetEnemy.transform.position;

        //食べ物を検索するタイミングを設定
        FindTiming();

        FindFoods();
        DicideTarget();
    }

    //private void FixedUpdate()
    //{
     
    //}

    void Update()
    {
        //もしDestinationが自身と近かったら


        m_navMeshAgent.nextPosition = m_targetEnemy.transform.position;
    }

    public void SetTarget()
    {
         FindFoods();
         DicideTarget();
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
        if (m_food.Length == 0)
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
        if (FindFoods() == true)
        {
            //自身から一番近い食べ物を決める
            DecideNearPosition();
            //評価値の最大値
            int Max = m_eval.Max();
            m_maxEvalNumber = 0;
            for(int amount=0;amount<m_eval.Length;amount++)
            {
                if(m_eval[amount]== Max)
                {
                    //これが一番大きい評価値の配列の番号
                    m_maxEvalNumber = amount;
                    break;
                }
            }

           
            //ターゲットの座標を取得
            m_targetposition = m_food[nearPosNumber].transform.position;
            //ターゲットを設定
            //m_navMeshAgent.destination = m_targetposition;
           
            m_navMeshAgent.SetDestination(m_targetposition);


            Vector3 a = transform.position - m_targetposition;
            Vector3 b=transform.position- m_navMeshAgent.destination;

            float A = a.magnitude;
            float B = b.magnitude;

            if (B<1.0)
            {
                m_navMeshAgent.ResetPath();
                //ランダムに座標を取得
                m_targetposition = DecideRamdomPosition();
                //ターゲットを設定
                m_navMeshAgent.destination = m_targetposition;
            }

            Debug.Log(m_navMeshAgent.destination);

            m_oldDestination = m_navMeshAgent.destination;
        }
        else
        {
            //ランダムに座標を取得
            m_targetposition = DecideRamdomPosition();
            //ターゲットを設定
            m_navMeshAgent.destination = m_targetposition;
        }

        if (m_nowTargetPosition != m_targetposition)
        {
            // リセット
            m_targetAI.SetNowIndex(0);
        }
        m_nowTargetPosition = m_targetposition;
    }

    /// <summary>
    /// 一番近い食べ物の座標を返す
    /// </summary>
    /// <returns></returns>
    Vector3 DecideNearPosition()
    {
        //範囲内の食べ物を調べる

        for(int i=0;i< m_food.Length;i++)
        {
            m_eval[i] = 0;
        }

        //食べ物の座標を取得
        Vector3 foodpos = m_food[0].transform.position;
        //自身から食べ物に向かうベクトルを計算
        Vector3 diff = foodpos - transform.position;
        //ベクトルを長さに変換
        float nearLength = diff.magnitude;

        //一番近い食べ物の配列番号
        nearPosNumber = 0;

        for (int amount = 1; amount < m_food.Length; amount++)
        {
            //食べ物の座標を取得
            Vector3 FoodPos = m_food[amount].transform.position;
            //自身から食べ物に向かうベクトルを計算
            Vector3 Diff = FoodPos - transform.position;
            //ベクトルを長さに変換
            float Length = Diff.magnitude;

            //フードのポイント
            int foodPoint = m_food[amount].GetComponent<Food>().GetPoint();

            m_eval[amount] = 50 * foodPoint;
            m_addEvalValue = 100;

            //もし自身から最も近いなら
            if (nearLength > Length)
            {
                
                //一番近い食べ物の評価値を上げていく
                m_eval[amount] += m_addEvalValue;
                //次の評価値は10大きくする
                m_addEvalValue += 10;

                //一番近い食べ物を入れ替える
                nearLength = Length;

                nearPosNumber = amount;


            }
            else
            {
                //一番近い食べ物の評価値を上げていく
                //m_eval[nearPosNumber] += 100;
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

        ramdompos *= 200.0f;

        return ramdompos;
    }

    //一番近い食べ物より少し先にポイントが高い食べ物がある
    //全ての食べ物に評価値
    //近い食べ物ほど評価値が高い


}
