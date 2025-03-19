using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;
    private ISelectable selectedObject;
    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    private void Awake()
    {
        if (instance ==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSelect(ISelectable selectable)
    {
        if (selectable == selectedObject || selectable == null) return;
        if (selectedObject != null)
        {
            DeselectSelected();
        }
        selectedObject = selectable;
        selectedObject.OnSelectionDataChanged += OnSelectionDataChanged;
    }

    private void Update()
    {
        if (selectedObject == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = new RaycastHit[50];
            if (Physics.RaycastNonAlloc(TouchRay, hits) > 0)
            {
                // If we find no selectable,
                if (hits.All(h => h.collider?.gameObject.GetComponent<ISelectable>() == null))
                {
                    DeselectSelected();
                }
            }
        }
    }

    private void DeselectSelected()
    {
        if (selectedObject == null) return;
        selectedObject.OnSelectionDataChanged -= OnSelectionDataChanged;
        selectedObject.OnDeselect();
    }

    private void OnSelectionDataChanged(SelectionData obj)
    {
        return;
    }
}
