namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public abstract class Enumeration(int id, string name, string description) : IComparable
{
    public string Name { get; private set; } = name;
    public int Id { get; private set; } = id;

    public string Description { get; private set; } = description;

    public int CompareTo(object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        if (obj is not Enumeration) throw new ArgumentException($"Object must be of type {nameof(Enumeration)}", nameof(obj));

        return Id.CompareTo(((Enumeration)obj).Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue) return false;

        return Id == otherValue.Id && Name == otherValue.Name;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public |
                                         System.Reflection.BindingFlags.Static |
                                         System.Reflection.BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public static bool operator ==(Enumeration left, Enumeration right) => Equals(left, right);

    public static bool operator !=(Enumeration left, Enumeration right) => !Equals(left, right);

    public static bool operator <(Enumeration left, Enumeration right) => left.CompareTo(right) < 0;

    public static bool operator <=(Enumeration left, Enumeration right) => left.CompareTo(right) <= 0;

    public static bool operator >(Enumeration left, Enumeration right) => left.CompareTo(right) > 0;

    public static bool operator >=(Enumeration left, Enumeration right) => left.CompareTo(right) >= 0;
}