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
   
    float m_counter = 0.0f;

    GameObject targetObject;
    
    [Header("ターゲットが被った時のカウントの上限")]
    public const int m_maxEvalCountLimit = 3;

    int m_addEvalValue = 100;

    //調べる範囲
    public float m_range = 200.0f;

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

    void Update()
    {
        
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
        //しばらくその場にいたなら
        Vector3 a = transform.position - m_targetAI.GetNextPosition();
        float A = a.magnitude;

        if (A < 5.0f)
        {
            m_counter += Time.deltaTime;
            if (m_counter > 3.0f)
            {
                SetRamdomTarget();
            }
        }
        else
        {
            m_counter = 0.0f;
        }

        //食べ物を検索
        //食べ物があったら
        if (FindFoods() == true)
        {
            //ターゲットの座標を取得
            GameObject target = DecideNearPosition();

            // 同じ場所
            if(targetObject == target)
            {
                return;
            }

            targetObject = target;
            m_targetposition = targetObject.transform.position;
            m_navMeshAgent.enabled = true;
            m_navMeshAgent.SetDestination(m_targetposition);

            ResetIndex();

            Debug.Log(m_navMeshAgent.destination);

            
        }
        else
        {
            //ランダムに座標を取得
            SetRamdomTarget();
        }
    }

    void ResetIndex()
    {
        // リセット
        m_targetAI.SetNowIndex(0);

        m_nowTargetPosition = m_targetposition;
    }

    void SetRamdomTarget()
    {

        //ランダムに座標を取得
        m_targetposition = DecideRamdomPosition();
        //ターゲットを設定
        m_navMeshAgent.SetDestination(m_targetposition);

        
    }

    /// <summary>
    /// 一番近い食べ物の座標を返す
    /// </summary>
    /// <returns></returns>
    GameObject DecideNearPosition()
    {
        //範囲内の食べ物を調べる

        for(int i=0;i< m_food.Length;i++)
        {
            m_eval[i] = 0;
        }

        float nearLength = m_range;

        int noRangeCount = 0;

        //一番近い食べ物の配列番号
        nearPosNumber = 0;

        m_addEvalValue = 100;

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

            m_eval[amount] = 10 * foodPoint;
            

            //範囲内なら
            if(m_range> Length)
            {
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
            }
            else
            {
                //範囲内にないのでカウントを増やす
                noRangeCount++;
            }

        }

        //全ての食べ物が範囲内になかったら
        if(noRangeCount== m_food.Length)
        {
            return null;
        }

        //評価値の最大値
        int Max = m_eval.Max();
        m_maxEvalNumber = 0;
        for (int amount = 0; amount < m_eval.Length; amount++)
        {
            if (m_eval[amount] == Max)
            {
                //これが一番大きい評価値の配列の番号
                m_maxEvalNumber = amount;
                break;
            }
        }


        Debug.Log(m_eval[m_maxEvalNumber]);

        return m_food[m_maxEvalNumber];
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
