using System;
using UnityEngine;
public class EnemyMove : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyState _enemyState;
    [SerializeField] private float distanceCheckTolerance = 0.4f;
    
    private Vector3 _fromPosition;
    private float _progress = 0f;
    
    [SerializeField] private Transform target;
    //현재 목표로하는 웨이포인트 인덱스
    private int _wavePointIndex = 0;

    // 일정 시간후 도달할 위치 예측
    public Vector3 GetPredictedPosition(float timeAheadInSeconds)
    {
        if (!transform) return Vector3.zero;
        if (timeAheadInSeconds <= 0f) return transform.position;
        
        float distanceToNextWaypoint = Vector3.Distance(transform.position, target.position);
        float timeToNextWaypoint = distanceToNextWaypoint / _enemyState.MoveSpeed;

        // 시간내 다음 웨이포인트 도달한다면
        if (timeAheadInSeconds <= timeToNextWaypoint)
        {
            return Vector3.Lerp(transform.position, target.position, timeAheadInSeconds / timeToNextWaypoint);
        }
        else
        {
            // 다음 웨이포인트 이후로 도착하지 않으면
            float remainTime = timeAheadInSeconds - timeToNextWaypoint;
            int nextIndex = _wavePointIndex + 1;

            while (nextIndex < Waypoints.PointTransforms.Length)
            {
                // 이번 구간 거리
                float segmentDistance = Vector3.Distance(Waypoints.PointTransforms[nextIndex - 1].position, Waypoints.PointTransforms[nextIndex].position);
                float segmentTime = segmentDistance / _enemyState.MoveSpeed;

                if (remainTime <= segmentTime)
                {
                    return Vector3.Lerp(Waypoints.PointTransforms[nextIndex - 1].position,
                        Waypoints.PointTransforms[nextIndex].position,
                        remainTime / segmentTime);
                }

                remainTime -= segmentTime;
                nextIndex++;
            }
            // 모든 웨이 지난 후 예측시 그냥 마지막 웨이 위치
            return Waypoints.PointTransforms[^1].position;
        }
    }

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.OnEnemyDied += (enemy, amount) => DeadToMoveStop();
        Debug.Assert(_enemy, "Enemy 스크립트 없이 EnemyMove 사용됨");

        _enemyState = GetComponent<EnemyState>();
    }

    private void Start()
    {
        _fromPosition = transform.position;
        target = Waypoints.PointTransforms[0]; 
    }

    void Update()
    {
        _progress += Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(transform.position, target.position, _enemyState.MoveSpeed * Time.deltaTime);
        transform.LookAt(target);

        //웨이포인트 도착 시 다음 웨이포인트로 변경
        if (Vector3.Distance(transform.position, target.position) <= distanceCheckTolerance)
        {
            GetNextWayPoint();
        } 
    }

    private void GetNextWayPoint()
    {
        if (_wavePointIndex >= Waypoints.PointTransforms.Length - 1) //모든 웨이포인트를 방문
        {
            EndPath();
            return;
        }

        _wavePointIndex++; 
        target = Waypoints.PointTransforms[_wavePointIndex]; //다음 인덱스의 웨이포인트
    }
    
    //목표에 도달
    private void EndPath() 
    {
        // Enemy 스크립트에서 처리
        _enemy.EndPath();
    }

    private void DeadToMoveStop()
    {
        this.enabled = false;
    }
}
