using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] public List<SoundMapping> soundMappings;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip gameBGM;

    private Dictionary<SoundEffect, SoundMapping> _soundDictionary;
    private Dictionary<SoundMapping, int> _soundMappingCount;

    private void Awake()
    {
        // Singleton setup
        if (instance != null) Destroy(gameObject);
        instance = this;
        
        // Sound dictionary init for quick access
        _soundDictionary = new Dictionary<SoundEffect, SoundMapping>();
        _soundMappingCount = new Dictionary<SoundMapping, int>();
        foreach (var map in soundMappings)
        {
            if (_soundDictionary.TryAdd(map.sound, map))
            {
                _soundMappingCount[map] = 0;
            }
        }
    }

    private void Start()
    {
        backgroundMusicSource.clip = gameBGM;
        backgroundMusicSource.Play();
    }

    public void PlaySound(SoundEffect sfxToPlay)
    {
        if (_soundDictionary.TryGetValue(sfxToPlay, out var soundMapping))
        {
            if (_soundMappingCount[soundMapping] >= soundMapping.maximumCount
                && soundMapping.maximumCount != 0)
            {
                // 최대 재생가능수를 넘어 오디오 재생하지 않음
                return;
            }
            
            var clip = soundMapping.clip;
            var audioSource = PoolManager.Instance.Audio.Get();
            audioSource.PlayOneShot(clip);
            DOVirtual.DelayedCall(clip.length, () => PoolManager.Instance.Audio.Release(audioSource));
            
            _soundMappingCount[soundMapping]++;
            DOVirtual.DelayedCall(clip.length, () => _soundMappingCount[soundMapping]--);
        }
    }
}