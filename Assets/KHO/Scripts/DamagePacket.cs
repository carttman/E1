using System;
using static Global;

[Serializable]
public struct DamagePacket
{
    public float Value;
    public Element ElementType;
    public Tower Instigator;

    public DamagePacket(float value, Element elementType, Tower instigator = null)
    {
        Value = value;
        ElementType = elementType;
        Instigator = instigator;
    }
}