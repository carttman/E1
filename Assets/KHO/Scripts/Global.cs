public static class Global
{
    public enum Element
    {
        Water,
        Fire,
        Wind
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare
    }

    public static Element WinsTo(this Element inElement)
    {
        switch (inElement)
        {
            case Element.Water:
                return Element.Fire;
            case Element.Fire:
                return Element.Wind;
            case Element.Wind:
                return Element.Water;
            default:
                return inElement;
        }
    }

    public static bool WinsTo(this Element inElement, Element anotherElement)
    {
        return WinsTo(inElement) == anotherElement;
    }

    public static Element LosesTo(this Element inElement)
    {
        switch (inElement)
        {
            case Element.Water:
                return Element.Wind;
            case Element.Fire:
                return Element.Water;
            case Element.Wind:
                return Element.Fire;
            default:
                return inElement;
        }
    }

    public static bool LosesTo(this Element inElement, Element anotherElement)
    {
        return LosesTo(inElement) == anotherElement;
    }
}