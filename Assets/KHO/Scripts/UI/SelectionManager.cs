using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;

    [SerializeField] private GameObject _selectionUI;
    [SerializeField] private GameObject _towerUI;
    [SerializeField] private GameObject _enemyUI;
    private ISelectable selectedObject;

    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GameEventHub.Instance.OnTilePointerClick += t => DeselectSelected();
    }

    private void Update()
    {
        if (selectedObject == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var hits = new RaycastHit[50];
            if (Physics.RaycastNonAlloc(TouchRay, hits) > 0)
                // If we find no selectable,
                if (hits.All(h => h.collider?.gameObject.GetComponent<ISelectable>() == null))
                    DeselectSelected();
        }
    }

    public void OnSelect(ISelectable selectable)
    {
        if (selectable == selectedObject || selectable == null) return;
        if (selectedObject != null) DeselectSelected();
        selectedObject = selectable;
        selectedObject.OnSelectionDataChanged += OnSelectionDataChanged;
        OnSelectionDataChanged(selectedObject.GetSelectionData());

        _selectionUI.SetActive(true);
    }

    public void DeselectSelected()
    {
        if (selectedObject == null) return;
        selectedObject.OnSelectionDataChanged -= OnSelectionDataChanged;
        selectedObject.OnDeselect();
        selectedObject = null;

        _selectionUI.SetActive(false);
        TooltipSystem.Hide();
    }

    private void OnSelectionDataChanged(SelectionData data)
    {
        if (data == null) return;
        if (data is TowerSelectionData towerSelectionData)
        {
            _enemyUI.SetActive(false);
            _towerUI.SetActive(true);
            _towerUI.GetComponent<TowerSelectionUI>()?.HandleUIChange(towerSelectionData);
        }
        else if (data is EnemySelectionData enemySelectionData)
        {
            _towerUI.SetActive(false);
            _enemyUI.SetActive(true);
            _enemyUI.GetComponent<EnemySelectionUI>()?.HandleUIChange(enemySelectionData);
        }
    }
}