using UnityEditor;

public class DataEditorMenu
{
    [MenuItem("Tools/Fetch All Data")]
    public static void FetchAll()
    {
        FetchSkills();
        
        FetchCards();
        FetchEnemies();
    }

    [MenuItem("Tools/Fetch Cards Data")]
    public static void FetchCards()
    {
        GoogleSheetManager.FetchSheet(new CardProcessor());
    }

    [MenuItem("Tools/Fetch Enemies Data")]
    public static void FetchEnemies()
    {
        GoogleSheetManager.FetchSheet(new EnemyProcessor());
    }

    [MenuItem("Tools/Fetch Skills Data")]
    public static void FetchSkills()
    {
        GoogleSheetManager.FetchSheet(new SkillProcessor());
    }
}