using TMPro;
using UnityEngine;

[RequireComponent(typeof(DeckPresenter))]
public class DeckView : MonoBehaviour
{
    [SerializeField] private TMP_Text drawPileText;
    [SerializeField] private TMP_Text discardPileText;

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
        Destroy(card);
    }

    public void ClearCardsHand(Transform handParent)
    {
        foreach (Transform child in handParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayDeckDemo(int drawPileCount, string drawPileList, int discardPileCount)
    {
        drawPileText.text = $"Draw Pile [{drawPileCount}]\n{drawPileList}";
        discardPileText.text = $"Discard Pile [{discardPileCount}]";
    }
}
