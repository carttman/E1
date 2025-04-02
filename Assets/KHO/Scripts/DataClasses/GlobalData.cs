using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Scriptable Objects/GlobalData")]
public class GlobalData : ScriptableObject
{
    [SerializeField] public ElementData[] elementData;
    [SerializeField] public RateChance[] towerRateChance = new RateChance[10];
    [SerializeField] public int[] towerLevelUpCost = new int[9];

    [Header("Gameplay")]
    // 약점인 속성에게 가하는 배율
    [SerializeField]
    public float ElementWeaknessMultiplier = 1.5f;

    // 역속성인 속성에게 가하는 배율
    [SerializeField] public float ElementResistanceMultiplier = 0.5f;

    [Header("UI")] [SerializeField] public Color WeaknessDamageColor = Color.red;

    [SerializeField] public Color ResistanceDamageColor = Color.gray;
    [SerializeField] public Color NormalDamageColor = Color.black;


    public Sprite GetElementIcon(Global.Element element)
    {
        foreach (var data in elementData)
            if (data.element == element)
                return data.icon;

        return null;
    }

    public Color GetElementColor(Global.Element element)
    {
        foreach (var data in elementData)
            if (data.element == element)
                return data.color;

        return Color.white; // Default color if not found
    }

    [Serializable]
    public class ElementData
    {
        public Global.Element element;
        public Color color;
        public Sprite icon;
    }

    [Serializable]
    public class RateChance
    {
        public float oneStarRate;
        public float twoStarRate;
        public float threeStarRate;
    }
}