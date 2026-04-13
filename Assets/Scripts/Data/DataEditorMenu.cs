using UnityEditor;

public class DataEditorMenu
{
    [MenuItem("Tools/Fetch All Data")]
    public static void FetchAll()
    {
        GoogleSheetManager.FetchSheet(new CardProcessor());
    }

    [MenuItem("Tools/Fetch Cards Data")]
    public static void FetchCards()
    {
        GoogleSheetManager.FetchSheet(new CardProcessor());
    }
}