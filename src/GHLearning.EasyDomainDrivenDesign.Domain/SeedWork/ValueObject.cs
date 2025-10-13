namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public abstract class ValueObject
{
	protected abstract IEnumerable<object> GetEqualityComponents();

	public override bool Equals(object? obj) // 修改此處，將 object 改為 object?
	{
		if (obj == null || obj.GetType() != GetType())
			return false;
		var other = (ValueObject)obj;
		return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
	}

	public override int GetHashCode()
	{
		return GetEqualityComponents()
			.Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
	}
}