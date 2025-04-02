using System;

public interface ISelectable
{
    public event Action<SelectionData> OnSelectionDataChanged;
    public void OnSelect();
    public void OnDeselect();
    public SelectionData GetSelectionData();
}