using UnityEngine;
public class EnemyMove : MonoBehaviour
{
    private Enemy _enemy;
    
    public float speed = 10f; //속도

    private Transform target; //목표 방향
    private int wavePointIndex = 0;//현재 목표로하는 웨이포인트 인덱스

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        Debug.Assert(_enemy, "Enemy 스크립트 없이 EnemyMove 사용됨");
    }

    private void Start()
    {
        //WayPoints의 points를 static으로 선언해놨기 때문에 바로 불러올 수 있다.
        target = Waypoints.points[0]; 
    }

    void Update()
    {
        //이동해야 할 방향 ( 목표 위치 - 내 위치  )
        Vector3 dir = target.position - transform.position;

        //방향 벡터로 스피드 만큼 이동
        //방향을 단위벡터로 바꾸기 위해 normalized 진행 후 speed를 곱한만큼 진행 (프레임-시간 보정으로 deltaTime을 사용)
        transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World); //World Space에서 이동
        transform.LookAt(target);

        //웨이포인트 도착 시 다음 웨이포인트로 변경
        if (Vector3.Distance(transform.position, target.position) <= 0.01f)
        {
            GetNextWayPoint(); //다음 웨이포인트를 타겟으로 변경
        } 
    }

    void GetNextWayPoint()
    {
        if (wavePointIndex >= Waypoints.points.Length - 1) //모든 웨이포인트를 방문
        {
            EndPath();
            return;
        }

        wavePointIndex++; //다음 웨이포인트 인덱스
        target = Waypoints.points[wavePointIndex]; //다음 인덱스의 웨이포인트 오브젝트를 받아온다.
    }
    
    void EndPath() //경로의 끝(목표)에 도달
    {
        _enemy.EndPath();
    }
}
