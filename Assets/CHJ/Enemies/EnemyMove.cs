using System;
using UnityEngine;
public class EnemyMove : MonoBehaviour
{
    // Enemy 스크립트
    private Enemy _enemy;
    private EnemyState _enemyState;
    [SerializeField] private float distanceCheckTolerance = 0.4f;
    
    //public float speed = 10f;

    private Vector3 _fromPosition;
    private float _progress = 0f;
    private float _thisWaveDuration = 0f;
    
    [SerializeField] private Transform target;
    private int wavePointIndex = 0;//현재 목표로하는 웨이포인트 인덱스

    // 일정 시간후 도달할 위치 예측
    public Vector3 GetPredictedPosition(float timeAheadInSeconds)
    {
        if (timeAheadInSeconds <= 0f) return transform.position;
        
        float timeToNextWaypoint = _thisWaveDuration - _progress;

        // 시간내 다음 웨이포인트 도달한다면
        if (timeAheadInSeconds <= timeToNextWaypoint)
        {
            return Vector3.Lerp(transform.position, target.position, timeAheadInSeconds / timeToNextWaypoint);
        }
        else
        {
            // 다음 웨이포인트 이후로 도착하지 않으면
            float remainTime = timeAheadInSeconds - timeToNextWaypoint;
            int nextIndex = wavePointIndex + 1;

            while (nextIndex < Waypoints.points.Length)
            {
                // 이번 구간 거리
                float segmentDistance = Vector3.Distance(Waypoints.points[nextIndex - 1].position, Waypoints.points[nextIndex].position);
                float segmentTime = segmentDistance / _enemyState.speed;

                if (remainTime <= segmentTime)
                {
                    return Vector3.Lerp(Waypoints.points[nextIndex - 1].position,
                        Waypoints.points[nextIndex].position,
                        remainTime / segmentTime);
                }

                remainTime -= segmentTime;
                nextIndex++;
            }
            
            // 모든 웨이 지난 후 예측시 그냥 마지막 웨이 위치
            return Waypoints.points[^1].position;
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
        target = Waypoints.points[0]; 
    }

    void Update()
    {
        /*
        Vector3 dir = target.position - transform.position;

        //방향을 단위벡터로 바꾸기 위해 normalized 진행 후 speed를 곱한만큼 진행 (프레임-시간 보정으로 deltaTime을 사용)
        transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World); //World Space에서 이동
        transform.LookAt(target);
        */
        _progress += Time.deltaTime;
        
        //transform.position = Vector3.Lerp(_fromPosition, target.position, _progress / _thisWaveDuration);
        transform.position = Vector3.MoveTowards(transform.position, target.position, _enemyState.speed * Time.deltaTime);
        transform.LookAt(target);

        //웨이포인트 도착 시 다음 웨이포인트로 변경
        if (Vector3.Distance(transform.position, target.position) <= distanceCheckTolerance)
        {
            GetNextWayPoint(); //다음 웨이포인트를 타겟으로 변경
            
            /*
            var distance = Vector3.Distance(transform.position, target.position);
            _thisWaveDuration = distance / _enemyState.speed;
            
            _fromPosition = transform.position;
            _progress = 0f;
            */
        } 
    }

    void GetNextWayPoint()
    {
        if (wavePointIndex >= Waypoints.points.Length - 1) //모든 웨이포인트를 방문
        {
            EndPath();
            return;
        }

        wavePointIndex++; 
        target = Waypoints.points[wavePointIndex]; //다음 인덱스의 웨이포인트
    }
    
    void EndPath() //목표에 도달
    {
        // Enemy 스크립트에서 처리
        _enemy.EndPath();
    }

    void DeadToMoveStop()
    {
        //Debug.Log("MoveStop");
        this.enabled = false;
    }
}
