using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySO : ScriptableObject, IHasAnimations
{
    [ReadOnly] public string EnemyId;
    [ReadOnly] public string Name;
    [ReadOnly] public string Type;
    [ReadOnly] public int Health;
    [ReadOnly] public List<SkillSO> EnemySkill;
    public List<AnimationData> Animations = new List<AnimationData>();

    public Sprite[] GetSpriteByType(AnimationType type)
    {
        var animation = Animations.Find(a => a.animationType == type);
        return animation?.sprites;
    }
}
