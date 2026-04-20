using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemyProcessor : SheetProcessorBase
{
    public override string SheetName => "enemy";

    public override void ProcessData(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');

            if (columns.Length < 5)
            {
                Debug.LogWarning("Bad value!");
                continue;
            }

            string enemyId = columns[0].Trim('\"');
            string enemyType = columns[1].Trim('\"');

            string assetPath = $"{ScriptableObjectPaths.ENEMY_SAVE_PATH}/{enemyType}/{enemyId}.asset";

            if (!Directory.Exists(Path.GetDirectoryName(assetPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
            }

            EnemySO enemy = AssetDatabase.LoadAssetAtPath<EnemySO>(assetPath);
            bool isNew = false;

            if (enemy == null)
            {
                enemy = ScriptableObject.CreateInstance<EnemySO>();
                isNew = true;
            }

            enemy.EnemyId = enemyId;
            enemy.Type = enemyType;

            enemy.Name = columns[2].Trim('\"');
            int.TryParse(columns[3].Trim('\"'), out enemy.Health);
            
            enemy.EnemySkill = columns.Skip(4)
                                .Select(s => s.Trim('\"').Trim())
                                .Where(id => !string.IsNullOrWhiteSpace(id))
                                .Select(id => {
                                    return ScriptableObjectPaths.FindAssetById<SkillSO>(id, ScriptableObjectPaths.SKILL_SAVE_PATH);
                                })
                                .Where(skill => skill != null)
                                .ToList();
            

            if (isNew)
            {
                AssetDatabase.CreateAsset(enemy, assetPath);
            }
            else
            {
                EditorUtility.SetDirty(enemy);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
