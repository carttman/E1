using System;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip gameBGM;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        backgroundMusicSource.clip = gameBGM;
        backgroundMusicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null.");
            return;
        }

        var audioSource = PoolManager.Instance.Audio.Get();
        audioSource.PlayOneShot(clip);
        DOVirtual.DelayedCall(clip.length, () => PoolManager.Instance.Audio.Release(audioSource));
    }
}
