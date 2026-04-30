using UnityEditor;
using UnityEngine;

public static class ScriptableObjectPaths
{
    public const string SKILL_SAVE_PATH = "Assets/Data/Skills";
    public const string CARD_SAVE_PATH = "Assets/Data/Cards";
    public const string ENEMY_SAVE_PATH = "Assets/Data/Enemies";

    public const string ALGORITHM_SAVE_PATH = "Assets/Data/Algorithm";

    public static T FindAssetById<T>(string id, string pathAsset) where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets($"{id}", new[] { pathAsset });
        
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        Debug.LogWarning($"Not found {typeof(T).Name} asset with ID: {id} at {pathAsset}");
        return null;
    }
}