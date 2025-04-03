using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class Tower : MonoBehaviour, ISelectable
{
    public enum EnemyCompareType
    {
        HighestAge,
        LowestAge,
        HighestHealth,
        LowestHealth
    }

    [SerializeField] protected EnemyCompareType targetingType = EnemyCompareType.HighestAge;
    [SerializeField] protected bool isSelected;

    // 타워 static data
    [SerializeField] public TowerData towerData;

    // 타겟 범위
    [SerializeField] [Range(1.5f, 100f)] protected float targetingRange = 1.5f;

    // 타워가 때릴 수 있는 타겟들
    [SerializeField] protected List<Enemy> potentialTargets;

    // 타워 파트들 (좌표 처리용)
    [SerializeField] protected Transform rotatingPart;
    [SerializeField] protected Transform turret;
    
    // UI
    [SerializeField] private GameObject selectionIndicator;
    [SerializeField] private GameObject hoverUIPrefab;
    [SerializeField] private Vector3 hoverUIOffset = new(0, 100, 0);

    protected TowerHoverUI _hoverUI;
    private RectTransform _hoverUIRectTransform;
    protected EnemyComparer _enemyComparer;
    protected bool _isSelectable;
    protected RangeIndicator _rangeIndicator;
    protected SphereCollider SphereCollider;
    
    private Global.Rarity _rarity;

    public EnemyCompareType TargetingType
    {
        get => targetingType;
        set
        {
            if (value == targetingType) return;
            targetingType = value;
            ChangeEnemyComparer(targetingType);
        }
    }

    public Global.Rarity Rarity
    {
        get => _rarity;
        set => ChangeRarity(value);
    }

    public float TargetingRange => targetingRange;

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

        targetingRange = towerData.TowerStats[(int)Rarity].range;
        
        var mainUI = GameObject.FindGameObjectWithTag("Main UI");
        _hoverUI = Instantiate(hoverUIPrefab, mainUI.transform).GetComponent<TowerHoverUI>();
        _hoverUIRectTransform = _hoverUI.GetComponent<RectTransform>();
        _hoverUI.gameObject.SetActive(false);
    }

    protected void Start()
    {
        StartCoroutine(TurnOnSelectable(0.3f));
        //transform.DOShakePosition(0.5f, 0.1f).SetEase(Ease.OutElastic).SetUpdate(true).SetLink(gameObject);
        transform.DOShakeRotation(0.5f, 0.25f).SetEase(Ease.OutElastic).SetUpdate(true).SetLink(gameObject);
        transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 0.5f).SetEase(Ease.OutElastic).SetUpdate(true)
            .SetLink(gameObject);

        AudioManager.instance.PlaySound(SoundEffect.TowerBuilt);
        
        _hoverUI.gameObject.SetActive(true);
        _hoverUI.UpdateUI(towerData.elementType, (int)_rarity + 1);
        _hoverUIRectTransform.localScale = Vector3.one;
        _hoverUIRectTransform.anchoredPosition = Vector2.zero;
        _hoverUIRectTransform.position = Camera.main.WorldToScreenPoint(turret.transform.position) + hoverUIOffset;
    }

    protected void Update()
    {
        if (_hoverUI.gameObject.activeSelf)
        {
            _hoverUIRectTransform.position = Camera.main.WorldToScreenPoint(turret.transform.position) + hoverUIOffset;
        }
    }

    protected void OnDestroy()
    {
        if (_hoverUI?.gameObject)
        {
            Destroy(_hoverUI.gameObject);
        }
    }

    // 범위 시각화
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
    }

    protected void OnMouseEnter()
    {
        _rangeIndicator.gameObject.SetActive(true);
        if (enabled)
        {
            _hoverUI.gameObject.SetActive(true);
        }
    }

    protected void OnMouseExit()
    {
        if (!enabled) return;
        if (isSelected) return;
        _hoverUI.gameObject.SetActive(false);
        _rangeIndicator.gameObject.SetActive(false);
    }

    protected void OnMouseUpAsButton()
    {
        if (!_isSelectable) return;
        OnSelect();
    }

    // 범위 안에 들어올시 타겟에 추가
    protected void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{other.name} has entered the tower range");
        if (other.CompareTag("Enemy")) potentialTargets.Add(other.GetComponent<Enemy>());
    }

    // 범위 나갈시 타겟에서 삭제
    protected void OnTriggerExit(Collider other)
    {
        if (potentialTargets.Contains(other.GetComponent<Enemy>()))
            potentialTargets.Remove(other.GetComponent<Enemy>());
    }

    protected void OnValidate()
    {
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

    protected IEnumerator TurnOnSelectable(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _isSelectable = true;
    }

    // 타겟 지정 함수
    protected bool AcquireTarget(out Transform pTarget)
    {
        potentialTargets.RemoveAll(e => !e || e.IsDead);

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

    public void UpgradeTo(TowerData upgradeTowerData)
    {
        Debug.Assert(towerData.upgradesTo[0] == upgradeTowerData || towerData.upgradesTo[1] == upgradeTowerData);

        var newTower = Instantiate(upgradeTowerData.towerPrefab, transform.position, transform.rotation);
        var newTowerComponent = newTower.GetComponent<Tower>();
        newTowerComponent.Kills = kills;
        newTowerComponent.DealtDamage = dealtDamage;
        newTowerComponent.Rarity = _rarity;
        newTower.GetComponent<BuildingTowerGhost>().enabled = false;
        newTowerComponent.enabled = true;

        SelectionManager.instance.OnSelect(newTower.GetComponent<Tower>());

        Destroy(gameObject);
    }

    public void ChangeRarity(Global.Rarity newRarity)
    {
        if (newRarity == _rarity) return;

        _rarity = newRarity;
        targetingRange = towerData.TowerStats[(int)_rarity].range;
        SphereCollider.radius = targetingRange;

        OnRarityChanged();
    }

    protected abstract void OnRarityChanged();


    protected abstract class EnemyComparer : IComparer<Enemy>
    {
        public abstract int Compare(Enemy x, Enemy y);
    }

    protected class HighestAge : EnemyComparer
    {
        public override int Compare(Enemy a, Enemy b)
        {
            return -a.Age.CompareTo(b.Age);
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
            return -a.GetComponent<StatsComponent>().Health.CompareTo(b.GetComponent<StatsComponent>().Health);
        }
    }

    protected class LowestHealth : EnemyComparer
    {
        public override int Compare(Enemy a, Enemy b)
        {
            return a.GetComponent<StatsComponent>().Health.CompareTo(b.GetComponent<StatsComponent>().Health);
        }
    }

    #region UI Selection

    public event Action<SelectionData> OnSelectionDataChanged;

    public void OnSelect()
    {
        isSelected = true;
        SelectionManager.instance.OnSelect(this);
        if (selectionIndicator) selectionIndicator.SetActive(true);
        _hoverUI.gameObject.SetActive(true);
    }

    public void OnDeselect()
    {
        isSelected = false;
        _rangeIndicator.gameObject.SetActive(false);
        if (selectionIndicator) selectionIndicator.SetActive(false);
        _hoverUI.gameObject.SetActive(false);
    }

    [SerializeField] private TowerSelectionData selectionData;

    public SelectionData GetSelectionData()
    {
        return selectionData;
    }


    // Stats
    [SerializeField] protected int kills;

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


    [SerializeField] protected float dealtDamage;

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
}