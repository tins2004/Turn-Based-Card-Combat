public class CardModel
{
    public ActorOnFloorRepository _actorOnFloorRepository { get; private set; }
    public CardSO cardData { get; private set; }


    public CardModel(CardSO cardData)
    {
        this.cardData = cardData;
        _actorOnFloorRepository = ActorOnFloorRepository.Instance;
    }
}
