public class CardModel
{
    public CardSO cardData { get; private set; }
    public ActorOnFloorRepository _actorOnFloorRepository { get; private set; }

    public CardModel(CardSO cardData)
    {
        this.cardData = cardData;
        
        _actorOnFloorRepository = ActorOnFloorRepository.Instance;
    }
}
