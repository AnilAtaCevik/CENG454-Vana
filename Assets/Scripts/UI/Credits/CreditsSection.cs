using System.Collections.Generic;

public class CreditsSection : ICreditsComponent
{
    private string _title;
    private List<ICreditsComponent> _children = new List<ICreditsComponent>();

    public CreditsSection(string title)
    {
        _title = title;
    }

    public void Add(ICreditsComponent component)
    {
        _children.Add(component);
    }

    public string Display()
    {
        string result = $"— {_title} —\n";
        foreach (var child in _children)
            result += child.Display() + "\n";
        return result;
    }
}