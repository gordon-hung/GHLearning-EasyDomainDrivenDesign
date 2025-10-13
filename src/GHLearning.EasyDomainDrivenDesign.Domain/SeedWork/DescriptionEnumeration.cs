namespace GHLearning.EasyDomainDrivenDesign.Domain.SeedWork;

public class DescriptionEnumeration(int id, string name, string description) : Enumeration(id, name)
{
	public string Description { get; private set; } = description;

	public string ToDescription()
	{
		return Description;
	}
}
