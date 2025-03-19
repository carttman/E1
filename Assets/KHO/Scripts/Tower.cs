using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour, ISelectable
{ 
    // ISelectable
    public event Action<SelectionData> OnSelectionDataChanged;
    public void OnSelect()
    {
        isSelected = true;
        SelectionManager.instance.OnSelect(this);
    }

    public void OnDeselect()
    {
        isSelected = false;
        _rangeIndicator.gameObject.SetActive(false);
    }
    public SelectionData GetSelectionData() => selectionData;
    // End of Iselectable

    [SerializeField] protected bool isSelected = false;
    
    [SerializeField] private TowerSelectionData selectionData;
    
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
    
    protected SphereCollider SphereCollider;
    
    // 타워 static data
    [SerializeField] public TowerData towerData;
    
    // 타겟 범위
    [SerializeField, Range(1.5f, 100f)]
    protected float targetingRange = 1.5f;

    public float TargetingRange => targetingRange;

    // 타워가 때릴 수 있는 타겟들
    [SerializeField] protected List<Transform> potentialTargets = new List<Transform>();
    
    // 타워 파트들 (좌표 처리용)
    [SerializeField] protected Transform rotatingPart;
    [SerializeField] protected Transform turret;

    protected RangeIndicator _rangeIndicator;
    
    protected void Awake()
    {
        Debug.Assert(towerData);
        
        SphereCollider = GetComponent<SphereCollider>();
        SphereCollider.radius = targetingRange;
        
        _rangeIndicator = GetComponentInChildren<RangeIndicator>();
        _rangeIndicator.gameObject.SetActive(false);

        selectionData = new TowerSelectionData(towerData, kills);
        selectionData.OnSelectionDataChanged += data => OnSelectionDataChanged?.Invoke(data);
    }
    
    // 타겟 지정 함수
    protected bool AcquireTarget(out Transform pTarget)
    {
        if (potentialTargets.Count == 0)
        {
            pTarget = null;
            return false;
        }

        for (int i = 0; i < potentialTargets.Count; i++)
        {
            if (potentialTargets[i])
            {
                pTarget = potentialTargets[i];
                return true;
            }
            else
            {
                potentialTargets.RemoveAt(i);
                i--;
            }
        }
        pTarget = null;
        return false;
    }

    // 타겟 쳐다보는 함수
    protected bool TrackTarget(ref Transform pTarget)
    {
        if (!pTarget) return false;
        
        if ((transform.position - pTarget.position).magnitude <= targetingRange)
        {
            // look at target
            turret.LookAt(pTarget.position);
            return true;
        }
        return false;
    }
    
    // 범위 안에 들어올시 타겟에 추가
    protected void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{other.name} has entered the tower range");
        if (other.CompareTag("Enemy"))
        {
            potentialTargets.Add(other.transform);
        }
    }

    // 범위 나갈시 타겟에서 삭제
    protected void OnTriggerExit(Collider other)
    {
        if (potentialTargets.Contains(other.transform))
        {
            potentialTargets.Remove(other.transform);
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
}
