public class CreditsItem : ICreditsComponent
{
    private string _name;

    public CreditsItem(string name)
    {
        _name = name;
    }

    public string Display()
    {
        return _name;
    }
}