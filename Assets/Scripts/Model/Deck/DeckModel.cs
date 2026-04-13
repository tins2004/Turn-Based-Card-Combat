using UnityEngine;

public class DeckModel
{
    public int numberHandCards { get; private set; }
    public string handCardName { get; private set; }
    private const int MAXCARDSINHAND = 8;
    private DeckRepository _deckRepository;

    //test
    private float cardWith = 2f;

    public DeckModel(int numberHandCards)
    {
        this.numberHandCards = numberHandCards;
        handCardName = "Hand Card";

        _deckRepository = DeckRepository.Instance;
    }

    public Vector2 GetCenterOffset()
    {
        float totalWidth = (numberHandCards * cardWith) + (numberHandCards - 1);
        
        return new Vector2(totalWidth / 2f - (cardWith / 2f), 0);
    }

    public Vector2 GetHandCardPosition(int x)
    {
        float posX = x * numberHandCards;
        return new Vector2(posX, 0) - GetCenterOffset();
    }
}
