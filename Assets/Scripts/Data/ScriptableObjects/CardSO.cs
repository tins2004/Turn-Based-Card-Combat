using UnityEngine;

[System.Serializable]
public class CardSO : ScriptableObject {
    [ReadOnly] public string CardId;
    [ReadOnly] public string Name;
    [ReadOnly] public string Type;
    [ReadOnly] public SkillSO Detail;
}