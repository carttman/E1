using System;

public abstract class SelectionData
{
    public Action<SelectionData> OnSelectionDataChanged;
}