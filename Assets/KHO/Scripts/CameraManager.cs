using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera trackCamera;

    private Transform _lookAtTransform;
    
    private void Start()
    {
        waveSpawner.OnThisWaveFinished += WaveSpawnerOnThisWaveFinished;
    }

    private void WaveSpawnerOnThisWaveFinished(Enemy obj)
    {
        if (_lookAtTransform == null)
        {
            GameObject emptyGO = new GameObject();
            emptyGO.transform.SetParent(transform);
            _lookAtTransform = emptyGO.transform;
        }
        _lookAtTransform.position = obj.transform.position;
        trackCamera.Target.TrackingTarget = _lookAtTransform;
        trackCamera.gameObject.SetActive(true);

        StartCoroutine(Game.Instance.PlaySlowmo(2f));
        StartCoroutine(TurnOffTrackCamera(2f));
    }
    
    private IEnumerator TurnOffTrackCamera(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        trackCamera.gameObject.SetActive(false);
    }
}
