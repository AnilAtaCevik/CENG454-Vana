using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void OnBackClicked()
    {
        UIManager.Instance.GoBack();
    }
}