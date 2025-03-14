using System;
using UnityEngine;

public class SelectionTower : MonoBehaviour
{
    public event Action<TowerData> OnTowerBuilt;
    
    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(TouchRay, out RaycastHit hit))
        {
            transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                enabled = false;
                Tower tower = GetComponent<Tower>();
                OnTowerBuilt?.Invoke(tower.TowerData);
                tower.enabled = true;
            }
        }
    }
    
}
