using UnityEngine;

public class CharacterModel : BaseActorModel
{
    public CharacterSO characterData
    {
        get { return actorData as CharacterSO; }
    }

    public CharacterModel(CharacterSO characterData)
    {
        SetUpActor(100, characterData);
    }
}
