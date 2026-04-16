using UnityEngine;

[System.Serializable]
public class SkillSO : ScriptableObject {
    [ReadOnly] public string SkillId;
    [ReadOnly] public string Name;
    [ReadOnly] public string Type;
    [ReadOnly] public string Description;
    [ReadOnly] public int RangeSkillImpact;
    [ReadOnly] public int ImpactValue;

    [Required("Create Skill Strategy Algorithm and add to skill")] 
    public SkillStrategy SkillAlgorithm;
}