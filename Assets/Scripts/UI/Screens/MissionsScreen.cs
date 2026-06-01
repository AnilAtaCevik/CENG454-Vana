using UnityEngine;
using System.Collections.Generic;

public class MissionsScreen : MonoBehaviour, IScreen
{
    public string ScreenName => "MissionsScreen";

    [SerializeField] private GameObject canvas;
    [SerializeField] private MissionData[] missions;
    [SerializeField] private MissionCardPool cardPool;

    private List<MissionCard> _activeCards = new List<MissionCard>();

    public void Show()
    {
        canvas.SetActive(true);
        LoadMissions();
    }

    public void Hide()
    {
        canvas.SetActive(false);
        cardPool.ReturnAll(_activeCards);
    }

    // Gets cards from pool and sets them up with mission data
    private void LoadMissions()
    {
        cardPool.ReturnAll(_activeCards);
        foreach (var mission in missions)
        {
            MissionCard card = cardPool.GetCard();
            card.Setup(mission);
            _activeCards.Add(card);
        }
    }
}