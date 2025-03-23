using System;
using UnityEngine;

public abstract class SelectionData
{
    public Action<SelectionData> OnSelectionDataChanged;
}
