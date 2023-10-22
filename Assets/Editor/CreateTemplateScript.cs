using UnityEditor;
using UnityEngine;
using System.IO;

using System;
using System.Linq;


public class CreateTemplateScript : EditorWindow
{
    /////////////////////////
    //  PRIVATE VARIABLES  //
    /////////////////////////

    private string _name;
    private string _folder;

    public void OnGUI()
    {
        GUILayout.Label("Template Script Information", EditorStyles.boldLabel);
        _name = EditorGUILayout.TextField("Asset Name ", _name);
        if (GUILayout.Button("Create Template Script"))
        {
            CreateFiles();
            this.Close();

        }

    }

    [MenuItem("Assets/Create/CreateTemplateScript")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateTemplateScript));
    }

    //  PRIVATE METHODS  //


    private void CreateFiles()
    {
        string curDir = GetCurrentPath();
        string[] splitPath = curDir.Split('/');
        _folder = splitPath[splitPath.Length - 1];
        CreateScript(splitPath, curDir);

        AssetDatabase.Refresh();


    }

    private void CreateScript(string[] splitPath, string curDir)
    {

        string Path = curDir + "/" + Capitalize(_name) + ".cs";
        string ClassName = Capitalize(_name);
        string nameSpace = _folder;


        string viewTemplate =
        "using UnityEngine;\n" +
        "using System.Collections;\n" +
        "using System.Collections.Generic;\n" +
        "namespace " + nameSpace + "\n" +
        "{\n" +
        "\tpublic class " + ClassName + (": MonoBehaviour\n") +
        "\t{\n\n" +
        "\t\t///  INSPECTOR VARIABLES       ///\n\n" +
        "\t\t///  PRIVATE VARIABLES         ///\n\n" +
        "\t\t///  PRIVATE METHODS           ///\n\n" +
        "\t\t///  LISTNER METHODS           ///\n\n" +
        "\t\t///  PUBLIC API                ///\n\n" +
        "\t\t///  IMPLEMENTATION            ///\n\n" +

        "\t}\n" +
        "}";

        using (StreamWriter view = File.CreateText(Path))
        {
            view.WriteLine(viewTemplate);

        }

    }

    private string Capitalize(string str)
    {
        if (str.Length == 0)
        {
            Debug.LogError("Empty String");
        }
        else if (str.Length == 1)
        {
            str = str.ToUpper();
        }
        else
        {
            str = (char.ToUpper(str[0]) + str.Substring(1));
        }
        return str;
    }


    private string GetCurrentPath()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        return path;
    }
}