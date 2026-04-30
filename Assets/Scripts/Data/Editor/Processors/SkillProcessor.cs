using System.IO;
using UnityEditor;
using UnityEngine;

public class SkillProcessor : SheetProcessorBase
{
    public override string SheetName => "skill";

    public override void ProcessData(string[] lines)
    {   
        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');

            if (columns.Length != 6)
            {
                Debug.LogWarning("Bad value!");
                continue;
            }

            string skillId = columns[0].Trim('\"');
            string skillType = columns[1].Trim('\"');

            string assetPath = $"{ScriptableObjectPaths.SKILL_SAVE_PATH}/{skillType}/{skillId}.asset";

            if (!Directory.Exists(Path.GetDirectoryName(assetPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
            }

            SkillSO skill = AssetDatabase.LoadAssetAtPath<SkillSO>(assetPath);
            bool isNew = false;

            if (skill == null)
            {
                skill = ScriptableObject.CreateInstance<SkillSO>();
                isNew = true;
            }

            skill.SkillId = skillId;
            skill.Type = skillType;

            skill.Name = columns[2].Trim('\"');;
            skill.Description = columns[3].Trim('\"');
            int.TryParse(columns[4].Trim('\"'), out skill.RangeSkillImpact);
            int.TryParse(columns[5].Trim('\"'), out skill.ImpactValue);

            skill.SkillAlgorithm = ScriptableObjectPaths.FindAssetById<SkillStrategy>($"{skillId}_ALGORITHM", ScriptableObjectPaths.ALGORITHM_SAVE_PATH);

            if (isNew)
            {
                AssetDatabase.CreateAsset(skill, assetPath);
            }
            else
            {
                EditorUtility.SetDirty(skill);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
