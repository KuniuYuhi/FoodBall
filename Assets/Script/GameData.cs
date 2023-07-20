using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// リザルトが終わったら手動で消してください
public class GameData : MonoBehaviour
{
    // キャラクターの種類
    public enum ActorCharacter
    {
        enActorCharacter_Sheep,
        enActorCharacter_Penguin,
        enActorCharacter_Duck,
        enActorCharacter_Cat,
    }

    // プレイ結果の構造体
    [System.Serializable]
    public struct ActorData
    {
        public ActorCharacter actorCharacter;
        public int Score;
    }

    ActorData[] m_actorData;
    // プレイ結果を保存
    public void SetActorData(ActorData[] actorData)
    {
        // サイズを設定
        m_actorData = new ActorData[actorData.Length];
        m_actorData = actorData;
    }
    // プレイ結果を返す
    public ActorData[] GetActorData()
    {
        return m_actorData;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
