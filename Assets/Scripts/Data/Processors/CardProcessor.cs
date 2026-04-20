using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CardProcessor : SheetProcessorBase
{
    public override string SheetName => "card";

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

            string cardId = columns[0].Trim('\"');
            string cardType = columns[1].Trim('\"');

            string assetPath = $"{ScriptableObjectPaths.CARD_SAVE_PATH}/{cardType}/{cardId}.asset";

            if (!Directory.Exists(Path.GetDirectoryName(assetPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
            }

            CardSO card = AssetDatabase.LoadAssetAtPath<CardSO>(assetPath);
            bool isNew = false;

            if (card == null)
            {
                card = ScriptableObject.CreateInstance<CardSO>();
                isNew = true;
            }

            card.CardId = cardId;
            card.Type = cardType;

            card.Name = columns[2].Trim('\"');
            card.Detail = ScriptableObjectPaths.FindAssetById<SkillSO>(columns[3].Trim('\"').Trim(), $"{ScriptableObjectPaths.SKILL_SAVE_PATH}");
            
            int.TryParse(columns[4].Trim('\"'), out card.EnergyRequired);

            if (isNew)
            {
                AssetDatabase.CreateAsset(card, assetPath);
            }
            else
            {
                EditorUtility.SetDirty(card);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
