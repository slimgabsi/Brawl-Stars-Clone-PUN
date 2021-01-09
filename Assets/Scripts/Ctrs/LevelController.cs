using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    public List<Transform> spawnPoints = new List<Transform>();
    private int _randomSpawnIndex;
    /// <summary>
    /// return random index 
    /// </summary>
    public int RandomSpawnIndex
    {
        get { return Random.Range(0, spawnPoints.Count); }
        private set { _randomSpawnIndex = value; }
    }
    //Singleton
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }
    public Vector3 GetRandomSpawnPos()
    {
       return spawnPoints[RandomSpawnIndex].position;
    }
   
}


