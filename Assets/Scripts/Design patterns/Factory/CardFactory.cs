using UnityEngine;
using System.Collections.Generic;

public class CardFactory : SingletonMonoBehaviour<CardFactory>
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<CardSO> cardConfig;
    private Dictionary<string, CardSO> dataMapping = new Dictionary<string, CardSO>();

    protected override void Awake()
    {
        base.Awake();
        
        foreach (var card in cardConfig)
        {
            dataMapping[card.CardId] = card;
        }
    }

    public GameObject CreateCard(string cardId, Transform parent)
    {
        if (dataMapping.TryGetValue(cardId, out CardSO cardData))
        {
            GameObject obj = Instantiate(cardPrefab, parent);
            
            obj.GetComponent<CardPresenter>().SetUpCard(cardData);

            return obj;
        }

        Debug.LogError($"Card ID {cardId} not found in Factory!");
        return null;
    }
}