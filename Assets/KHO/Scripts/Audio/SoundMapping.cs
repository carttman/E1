using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SoundMapping
{
    public SoundEffect sound;
    public AudioClip clip;
    // 이 효과음이 몇개까지 플레이 될수 있는지
    // 0 = 무한
    public int maximumCount;
    public float volume = 1f;
}
