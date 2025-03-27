using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class Tower : MonoBehaviour, ISelectable
{ 
    #region UI Selection
    public event Action<SelectionData> OnSelectionDataChanged;
    public void OnSelect()
    {
        isSelected = true;
        SelectionManager.instance.OnSelect(this);
        if (_selectionIndicator)
        {
            _selectionIndicator.SetActive(true);
        }
    }

    public void OnDeselect()
    {
        isSelected = false;
        _rangeIndicator.gameObject.SetActive(false);
        if (_selectionIndicator)
        {
            _selectionIndicator.SetActive(false);
        }
    }

    [SerializeField] private TowerSelectionData selectionData;
    public SelectionData GetSelectionData() => selectionData;
    

    // Stats
    [SerializeField] protected int kills = 0;
    public int Kills
    {
        get => kills;
        set
        {
            if (value == kills) return;
            kills = value;
            selectionData.Kills = value;
        }
    }

    
    [SerializeField] protected float dealtDamage = 0f;
    public float DealtDamage
    {
        get => dealtDamage;
        set
        {
            if (Mathf.Approximately(value, dealtDamage)) return;
            dealtDamage = value;
            selectionData.DealtDamage = value;
        }
    }
    #endregion

    public enum EnemyCompareType
    {
        HighestAge,
        LowestAge,
        HighestHealth,
        LowestHealth
    }

    [SerializeField] protected EnemyCompareType targetingType = EnemyCompareType.HighestAge;
    public EnemyCompareType TargetingType
    {
        get => targetingType;
        set
        {
            if (value ==  targetingType) return;
            targetingType = value;
            ChangeEnemyComparer(targetingType);
        }
    }
    protected EnemyComparer _enemyComparer;
    

    protected bool _isSelectable = false;
    [SerializeField] protected bool isSelected = false;
    protected SphereCollider SphereCollider;
    
    // 타워 static data
    [SerializeField] public TowerData towerData;
    
    // 타겟 범위
    [SerializeField, Range(1.5f, 100f)]
    protected float targetingRange = 1.5f;

    public float TargetingRange => targetingRange;

    // 타워가 때릴 수 있는 타겟들
    [SerializeField] protected List<Enemy> potentialTargets;
    
    // 타워 파트들 (좌표 처리용)
    [SerializeField] protected Transform rotatingPart;
    [SerializeField] protected Transform turret;

    protected RangeIndicator _rangeIndicator;
    [SerializeField] private GameObject _selectionIndicator;
    
        
    protected abstract class EnemyComparer: IComparer<Enemy>
    {
        public abstract int Compare(Enemy x, Enemy y);
    }
    
    protected class HighestAge : EnemyComparer
    {
        public override int Compare(Enemy a, Enemy b)
        {
            return -(a.Age.CompareTo(b.Age));
        }
    }
    
    protected class LowestAge : EnemyComparer
    {
        public override int Compare(Enemy a, Enemy b)
        {
            return a.Age.CompareTo(b.Age);
        }
    }
    
    protected class HighestHealth : EnemyComparer
    {
        public override int Compare(Enemy a, Enemy b)
        {
            return -(a.GetComponent<StatsComponent>().Health.CompareTo(b.GetComponent<StatsComponent>().Health));
        }
    }
    
    protected class LowestHealth : EnemyComparer
    {
        public override int Compare(Enemy a, Enemy b)
        {
            return a.GetComponent<StatsComponent>().Health.CompareTo(b.GetComponent<StatsComponent>().Health);
        }
    }
    
    protected void Awake()
    {
        Debug.Assert(towerData);
        
        SphereCollider = GetComponent<SphereCollider>();
        SphereCollider.radius = targetingRange;
        
        _rangeIndicator = GetComponentInChildren<RangeIndicator>();
        _rangeIndicator.gameObject.SetActive(false);

        selectionData = new TowerSelectionData(towerData, kills, dealtDamage);
        selectionData.tower = this;
        selectionData.OnSelectionDataChanged += data => OnSelectionDataChanged?.Invoke(data);
        
        ChangeEnemyComparer(targetingType);
    }

    protected void ChangeEnemyComparer(EnemyCompareType compareTypeType)
    {
        switch (compareTypeType)
        {
            case EnemyCompareType.HighestAge:
                _enemyComparer = new HighestAge();
                return;
            case EnemyCompareType.LowestAge:
                _enemyComparer = new LowestAge();
                return;
            case EnemyCompareType.HighestHealth:
                _enemyComparer = new HighestHealth();
                return;
            case EnemyCompareType.LowestHealth:
                _enemyComparer = new LowestHealth();
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(compareTypeType), compareTypeType, null);
        }
    }

    protected void Start()
    {
        StartCoroutine(TurnOnSelectable(0.5f));
        //transform.DOShakePosition(0.5f, 0.1f).SetEase(Ease.OutElastic).SetUpdate(true).SetLink(gameObject);
        transform.DOShakeRotation(0.5f, 0.25f).SetEase(Ease.OutElastic).SetUpdate(true).SetLink(gameObject);
        transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 0.5f).SetEase(Ease.OutElastic).SetUpdate(true).SetLink(gameObject);
    }

    protected IEnumerator TurnOnSelectable(float time)
    {
        yield return new WaitForSeconds(time);
        _isSelectable = true;
    }

    protected void OnValidate()
    {
        ChangeEnemyComparer(targetingType);
    }

    // 타겟 지정 함수
    protected bool AcquireTarget(out Transform pTarget)
    {
        potentialTargets.RemoveAll((e) => !e || e.IsDead);
        
        if (potentialTargets.Count == 0)
        {
            pTarget = null;
            return false;
        }
        
        potentialTargets.Sort(_enemyComparer);

        var first = potentialTargets.First();
        if (first)
        {
            pTarget = first.transform;
        }
        else
        {
            pTarget = null;
            return false;
        }
        return pTarget;
    }

    // 타겟 쳐다보는 함수
    protected void TrackTarget(ref Transform pTarget)
    {
        var direction = pTarget.position - rotatingPart.position;
        direction.y = 0f;
        
        rotatingPart.LookAt(direction);
    }
    
    // 범위 안에 들어올시 타겟에 추가
    protected void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{other.name} has entered the tower range");
        if (other.CompareTag("Enemy"))
        {
            potentialTargets.Add(other.GetComponent<Enemy>());
        }
    }

    // 범위 나갈시 타겟에서 삭제
    protected void OnTriggerExit(Collider other)
    {
        if (potentialTargets.Contains(other.GetComponent<Enemy>()))
        {
            potentialTargets.Remove(other.GetComponent<Enemy>());
        }
    }

    // 범위 시각화
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
    }

    protected void OnMouseUpAsButton()
    {
        if (!_isSelectable) return;
        OnSelect();
    }

    protected void OnMouseEnter()
    {
        _rangeIndicator.gameObject.SetActive(true);
    }
    
    protected void OnMouseExit()
    {
        if (isSelected) return;
        _rangeIndicator.gameObject.SetActive(false);
    }

    public void UpgradeTo(TowerData upgradeTowerData)
    {
        Debug.Assert(towerData.upgradesTo[0] == upgradeTowerData || towerData.upgradesTo[1] == upgradeTowerData);
        
        var newTower = Instantiate(upgradeTowerData.towerPrefab, transform.position, transform.rotation);
        var newTowerComponent = newTower.GetComponent<Tower>();
        newTowerComponent.Kills = kills;
        newTowerComponent.DealtDamage = dealtDamage;
        newTower.GetComponent<BuildingTowerGhost>().enabled = false;
        newTowerComponent.enabled = true;
        
        SelectionManager.instance.OnSelect(newTower.GetComponent<Tower>());
        
        Destroy(gameObject);
    }

}


