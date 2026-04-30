using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager
{
    public static void FetchSheet(SheetProcessorBase processor)
    {
        string url = $"https://docs.google.com/spreadsheets/d/{GetSpreadsheetId()}/gviz/tq?tqx=out:csv&sheet={processor.SheetName}";
        
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        var operation = webRequest.SendWebRequest();

        EditorUtility.DisplayProgressBar("Fetching Data", $"Downloading {processor.SheetName}...", 0.5f);
        while (!operation.isDone) { }
        EditorUtility.ClearProgressBar();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string rawData = webRequest.downloadHandler.text;
            
            string[] lines = rawData.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            
            processor.ProcessData(lines);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"<color=green>Thành công:</color> Đã cập nhật dữ liệu cho {processor.SheetName}");
        }
        else
        {
            Debug.LogError($"Lỗi khi lấy sheet {processor.SheetName}: {webRequest.error}");
        }
    }

    private static string GetSpreadsheetId()
    {
        string path = "Assets/Editor/Secrets/spreadSheetId.txt";
        if (System.IO.File.Exists(path))
        {
            return System.IO.File.ReadAllText(path).Trim();
        }
        Debug.LogError("Không tìm thấy file config.txt!");
        return "";
    }
}