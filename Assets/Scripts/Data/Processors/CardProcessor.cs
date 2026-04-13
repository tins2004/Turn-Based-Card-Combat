using System.IO;
using UnityEditor;
using UnityEngine;

public class CardProcessor : SheetProcessorBase
{
    public override string SheetName => "card";
    private const string SAVE_PATH = "Assets/Data/Cards";

    public override void ProcessData(string[] lines)
    {
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        
        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');

            if (columns.Length < 3)
            {
                Debug.LogWarning("Don't have enough value!");
                continue;
            }

            string cardName = columns[0].Trim('\"');
            string assetPath = $"{SAVE_PATH}/{cardName}.asset";

            CardSO card = ScriptableObject.CreateInstance<CardSO>();

            card.Name = cardName;
            card.Type = columns[1].Trim('\"');
            int.TryParse(columns[2].Trim('\"'), out card.Level);

            AssetDatabase.CreateAsset(card, assetPath);
        }

        AssetDatabase.SaveAssets();
    }
}
