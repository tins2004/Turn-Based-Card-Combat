using UnityEngine;

public interface IHasAnimations {
    Sprite[] GetSpriteByType(AnimationType type);
}