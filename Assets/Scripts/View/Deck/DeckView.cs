using TMPro;
using UnityEngine;

[RequireComponent(typeof(DeckPresenter))]
public class DeckView : MonoBehaviour
{
    [SerializeField] private TMP_Text drawPileText;
    [SerializeField] private TMP_Text discardPileText;

    private ObjectPool _cardPool;

    private void Start()
    {
        if (_cardPool == null)
        {
            _cardPool = GetComponent<ObjectPool>();
        }
    }

    public GameObject DrawCardsHand(string cardId, CardTransform transformData, Transform handParent, string name)
    {
        GameObject obj = CardFactory.Instance.CreateCard(cardId, handParent);

        if (obj != null)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.localPosition = transformData.Position;
            rect.localRotation = transformData.Rotation;

            obj.name = name;

            return obj;
        }

        return null;
    }

    public void DestroyCardHand(GameObject card)
    {
        if (_cardPool == null)
        {
            _cardPool = GetComponent<ObjectPool>();
        }

        _cardPool.ReturnObject(card);
    }

    public void ClearCardsHand(Transform handParent)
    {
        if (_cardPool == null)
        {
            _cardPool = GetComponent<ObjectPool>();
        }

        _cardPool.ReturnAllObjects();
    }

    public void ArrangeCardsHand(CardTransform transformData, RectTransform rect)
    {
        rect.localPosition = transformData.Position;
        rect.localRotation = transformData.Rotation;
    }

    public void DisplayDeckDemo(int drawPileCount, string drawPileList, int discardPileCount)
    {
        drawPileText.text = $"Draw Pile [{drawPileCount}]\n{drawPileList}";
        discardPileText.text = $"Discard Pile [{discardPileCount}]";
    }
}
