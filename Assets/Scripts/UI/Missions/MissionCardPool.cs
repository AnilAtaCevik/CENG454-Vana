using System.Collections.Generic;
using UnityEngine;

// Object Pool pattern: reuses MissionCard objects instead of Instantiate/Destroy
public class MissionCardPool : MonoBehaviour
{
    [SerializeField] private MissionCard cardPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private int initialPoolSize = 5;

    private Queue<MissionCard> _pool = new Queue<MissionCard>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            MissionCard card = Instantiate(cardPrefab, container);
            card.gameObject.SetActive(false);
            _pool.Enqueue(card);
        }
    }

    // Returns a card from the pool, creates new one if pool is empty
    public MissionCard GetCard()
    {
        if (_pool.Count > 0)
            return _pool.Dequeue();

        MissionCard newCard = Instantiate(cardPrefab, container);
        return newCard;
    }

    // Returns a card back to the pool
    public void ReturnCard(MissionCard card)
    {
        card.ResetCard();
        _pool.Enqueue(card);
    }

    public void ReturnAll(List<MissionCard> activeCards)
    {
        foreach (var card in activeCards)
            ReturnCard(card);
        activeCards.Clear();
    }
}