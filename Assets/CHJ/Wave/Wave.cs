using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Wave
{
    public GameObject enemy; //Spawn 될 Enemy
    public int SpawnCount; //Spawn 될 숫자
    public float SpawnRate; //Spawn 주기
}
