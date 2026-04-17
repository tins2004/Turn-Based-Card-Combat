using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Character", menuName = "Asset/Actor/CharacterSO")]
public class CharacterSO : ScriptableObject, IHasAnimations
{
    [Required] public string Health;
    [Required] public string Mana;
    [Required] public CardSO StartingCard;
    public List<AnimationData> Animations = new List<AnimationData>();

    public Sprite[] GetSpriteByType(AnimationType type)
    {
        var animation = Animations.Find(a => a.animationType == type);
        return animation?.sprites;
    }
}
