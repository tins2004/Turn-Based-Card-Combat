using System.Collections.Generic;
using UnityEngine;

public class DeckModel
{
    /// <summary>
    /// The cards are waiting to be drawn.
    /// </summary>
    public List<string> drawPile = new List<string>();
    
    /// <summary>
    /// The cards in hand.
    /// </summary>
    public List<string> hand = new List<string>();

    /// <summary>
    /// The cards that have been used.
    /// </summary>
    public List<string> discardPile = new List<string>();
    
    public HandCardsRepository _handCardsRepository { get; private set; }
    private const int MAX_HAND_SIZE = 10;
    // public const int CARD_PER_TURN = 5;

    private const float ANGLE_STEP = 2f;
    private const float CARD_SPACING = 230f;
    public string handCardName { get; private set; }

    public DeckModel(List<string> startingCards)
    {
        handCardName = "Card";
        _handCardsRepository = HandCardsRepository.Instance;

        drawPile = new List<string>(startingCards);
        Shuffle(drawPile);
    }

    private void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (hand.Count >= MAX_HAND_SIZE) break;

            if (drawPile.Count == 0)
            {
                if (discardPile.Count == 0) break;
                drawPile.AddRange(discardPile);
                discardPile.Clear();
                Shuffle(drawPile);
            }

            string cardId = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(cardId);
        }
    }

    public void EndTurn()
    {
        discardPile.AddRange(hand);
        hand.Clear();
    }

    public GameObject UsedCard(string cardName)
    {
        CardPresenter cardPresenter = _handCardsRepository.Get(cardName);

        if (cardPresenter == null)
        {
            Debug.LogError($"Card {cardName} not found in HandCardsRepository!");
            return null;
        }

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i] == cardPresenter.GetCardData().CardId)
            {
                string cardId = hand[i];
                hand.RemoveAt(i);
                discardPile.Add(cardId);
                break;
            }
        }
        
        return cardPresenter.gameObject;
    }

    // public Vector2 GetCenterOffset()
    // {
    //     float totalWidth = (hand.Count * CARD_WITH) + (hand.Count - 1);
        
    //     return new Vector2(totalWidth / 2f - (CARD_WITH / 2f), 0);
    // }

    // public Vector2 GetHandCardPosition(int x)
    // {
    //     float posX = x * hand.Count;
    //     return new Vector2(posX, 0) - GetCenterOffset();
    // }

    public CardTransform GetCardVisualTransform(int index)
    {
        int totalCards = hand.Count;
        if (totalCards == 0) return new CardTransform();

        float centerIndex = (totalCards - 1) / 2f;
        float relativeIndex = index - centerIndex;

        float angle = -relativeIndex * ANGLE_STEP;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        float posX = relativeIndex * CARD_SPACING;
        float posY = -Mathf.Abs(relativeIndex) * Mathf.Abs(relativeIndex) * 10f;

        return new CardTransform
        {
            Position = new Vector3(posX, posY, 0),
            Rotation = rotation
        };
    }

    // public string[] DebugData()
    // {
    //     List<string> results = new List<string>();
    //     string drawIds = string.Join("\n", drawPile);
    //     // string handIds = string.Join(", ", hand);
    //     // string discardIds = string.Join("\n", discardPile);

    //     // string logMessage = 
    //     //     $"<b>[DECK STATUS]</b>\n" +
    //     //     $"<color=#00ffffff><b>Draw Pile ({drawPile.Count}):</b> [{drawIds}]</color>\n" +
    //     //     $"<color=#00ff00ff><b>Hand ({hand.Count}):</b> [{handIds}]</color>\n" +
    //     //     $"<color=#ffa500ff><b>Discard Pile ({discardPile.Count}):</b> [{discardIds}]</color>";

    //     results.Add(drawPile.Count.ToString());
    //     results.Add(drawIds);
    //     results.Add(discardPile.Count.ToString());

    //     return results.ToArray();
    // }
}

public struct CardTransform
{
    public Vector3 Position;
    public Quaternion Rotation;
}
