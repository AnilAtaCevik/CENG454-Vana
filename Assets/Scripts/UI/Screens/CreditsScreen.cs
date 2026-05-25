using System.Collections;
using UnityEngine;
using TMPro;

public class CreditsScreen : MonoBehaviour, IScreen
{
    public string ScreenName => "CreditsScreen";

    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private GameObject canvas;
    [SerializeField] private RectTransform creditsRect;
    [SerializeField] private float scrollSpeed = 50f;
    private Coroutine _scrollCoroutine;

    public void Show()
    {
        canvas.SetActive(true);
        // BuildAndDisplayCredits();
        creditsText.text = "";
        if (_scrollCoroutine != null) StopCoroutine(_scrollCoroutine);
        _scrollCoroutine = StartCoroutine(ScrollCredits());
    }

    public void Hide()
    {
        canvas.SetActive(false);
        if (_scrollCoroutine != null)
        {
            StopCoroutine(_scrollCoroutine);
            _scrollCoroutine = null;
        }
        creditsText.text = "";
    }

    // Builds the tree using Composite pattern and render it into a single string
    private void BuildAndDisplayCredits()
    {
        // Composite tree
        CreditsSection development = new CreditsSection("Development");
        development.Add(new CreditsItem("Anıl Ata Çevik"));
        development.Add(new CreditsItem("Ekin Karıncalı"));
        development.Add(new CreditsItem("İrem Gülce Bağır"));
        development.Add(new CreditsItem("Mehmet Berdan Gençer"));
        development.Add(new CreditsItem("Mehmet Efe Binicioğlu"));

        CreditsSection tools = new CreditsSection("Tools & Engine");
        tools.Add(new CreditsItem("Unity 6000.3.9f1"));

        CreditsSection course = new CreditsSection("Course");
        course.Add(new CreditsItem("CENG 454 - Game Programming"));
        course.Add(new CreditsItem("Spring 2025-2026"));

        CreditsSection thanks = new CreditsSection("Thanks To");
        thanks.Add(new CreditsItem("Mustafa Kemal Binli"));

        // All credits — root composite
        CreditsSection allCredits = new CreditsSection("VANA");
        allCredits.Add(development);
        allCredits.Add(tools);
        allCredits.Add(course);
        allCredits.Add(thanks);

        //Render all by one Display() call
        creditsText.text = allCredits.Display();
    }
    private IEnumerator ScrollCredits()
    {
        creditsText.text = "";
        BuildAndDisplayCredits();
        yield return null; // wait one frame for layout to update

        float screenHalfHeight = Screen.height / 2f;

        creditsRect.anchoredPosition = new Vector2(0, -screenHalfHeight);
        float stopY = 0f;

        while (creditsRect.anchoredPosition.y < stopY)
        {
            creditsRect.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            yield return null;
        }

        creditsRect.anchoredPosition = new Vector2(0, stopY);
        _scrollCoroutine = null;
    }
}