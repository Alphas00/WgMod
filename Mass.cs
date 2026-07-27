namespace WgMod;

// Mass in kg.
public readonly record struct Mass(float Value)
{
    public const float KgToPounds = 2.2046226218f;

    public override string ToString() => Value.ToString();
    public string Display() => $"{Value:0.#} kg ({ToPounds():0.#} lbs)";

    public readonly float ToPounds() => Value * KgToPounds;
    public static Mass FromPounds(float pounds) => new(pounds / KgToPounds);

    public static Mass operator +(Mass a, Mass b) => new(a.Value + b.Value);
    public static Mass operator -(Mass a, Mass b) => new(a.Value - b.Value);

    public static implicit operator float(Mass mass) => mass.Value;
    public static implicit operator Mass(float mass) => new(mass);
}
