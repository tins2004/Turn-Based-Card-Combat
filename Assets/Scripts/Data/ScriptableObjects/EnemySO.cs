using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySO : ScriptableObject {
    [ReadOnly] public string EnemyId;
    [ReadOnly] public string Name;
    [ReadOnly] public string Type;
    [ReadOnly] public List<SkillSO> EnemySkill;
}