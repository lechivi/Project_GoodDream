using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class AudioNameCreator
{
    private const string MENUITEM_PATH = "Tool/Create/Audio Name";
    private const string EXPORT_PATH = "Assets/_Data/Scripts/Constants/AUDIO.cs";

    private static readonly string FILENAME = Path.GetFileName(EXPORT_PATH);
    private static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(EXPORT_PATH);

    [MenuItem(MENUITEM_PATH)]
    public static void Create()
    {
        if (!CanCreate()) return;

        CreateScript();

        EditorUtility.DisplayDialog(FILENAME, "Creation completed!", "OK");
    }

    public static void CreateScript()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendFormat("public static class {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();
        builder.AppendLine("{");

        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] sfxList = Resources.LoadAll("Audio/SFX");

        foreach (AudioClip bgm in bgmList)
        {
            builder.Append("\t").AppendFormat(@"public const string {0} = ""{1}"";", bgm.name.ToUpper(), bgm.name).AppendLine();
        }

        builder.AppendLine("\t");

        foreach (AudioClip sfx in sfxList)
        {
            builder.Append("\t").AppendFormat(@"public const string {0} = ""{1}"";", sfx.name.ToUpper(), sfx.name).AppendLine();
        }

        builder.AppendLine("}");

        string directoryName = Path.GetDirectoryName(EXPORT_PATH);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        File.WriteAllText(EXPORT_PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    [MenuItem(MENUITEM_PATH, true)]
    private static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }
}
