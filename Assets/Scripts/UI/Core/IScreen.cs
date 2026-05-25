public interface IScreen
{
// Interface contract for all UI screens
// every screen can be shown, hidden, and identified
    string ScreenName { get; }
    void Show();
    void Hide();
}