using System.Reflection;

namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public abstract class Enumeration(int id, string name) : IComparable
{
	public int Id { get; private set; } = id;
	public string Name { get; private set; } = name;

	public override string ToString()
	{
		return Name;
	}

	public static IEnumerable<T> GetAll<T>() where T : Enumeration
	{
		return GetAll<T>(typeof(T));
	}

	public static IEnumerable<Enumeration> GetAll(Type type)
	{
		return GetAll<Enumeration>(type);
	}

	private static IEnumerable<T> GetAll<T>(Type type) where T : Enumeration
	{
		return type.GetFields(BindingFlags.Public |
							  BindingFlags.Static |
							  BindingFlags.DeclaredOnly)
			.Select(f => f.GetValue(null))
			.Cast<T>();
	}

	// 若需要客製化 Equals 再討論
	public override sealed bool Equals(object? obj)
	{
		if (obj == null)
		{
			return false;
		}

		if (obj is not Enumeration otherValue)
		{
			return false;
		}

		var typeMatches = GetType().Equals(obj.GetType());
		var idMatches = Id.Equals(otherValue.Id);

		return typeMatches && idMatches;
	}

	public override sealed int GetHashCode()
	{
		return Id.GetHashCode();
	}

	public static T FromId<T>(int id) where T : Enumeration
	{
		var matchingItem = Parse<T, int>(id, nameof(id), item => item.Id == id);
		return matchingItem;
	}

	public static T FromName<T>(string name) where T : Enumeration
	{
		var matchingItem = Parse<T, string>(
			name,
			nameof(name),
			item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase)
		);
		return matchingItem;
	}

	private static TEnumeration Parse<TEnumeration, TSource>(TSource source, string description, Func<TEnumeration, bool> predicate) where TEnumeration : Enumeration
	{
		var matchingItem = GetAll<TEnumeration>().FirstOrDefault(predicate)
						   ?? throw new InvalidOperationException($"'{source}' is not a valid {description} in {typeof(TEnumeration)}");

		return matchingItem;
	}

	public int CompareTo(object? obj)
	{
		if (obj is not Enumeration otherValue)
		{
			throw new ArgumentException($"Argument is not of type {nameof(Enumeration)}", nameof(obj));
		}

		return Id.CompareTo(otherValue.Id);
	}
}