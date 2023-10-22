using UnityEditor;
using UnityEngine;
using System.IO;

using System;
using System.Linq;


public class CreateViewMeditor : EditorWindow
{
    /////////////////////////
    //  PRIVATE VARIABLES  //
    /////////////////////////

    private string _name;
    private string _folder;

    public void OnGUI()
    {
        GUILayout.Label("View Mediator Information", EditorStyles.boldLabel);
        _name = EditorGUILayout.TextField("Asset Name ", _name);
        if (GUILayout.Button("Create View and Mediator"))
        {
            CreateFiles();
            this.Close();

        }

    }

    [MenuItem("Assets/Create/CreateViewMeditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateViewMeditor));
    }

    //  PRIVATE METHODS  //


    private void CreateFiles()
    {
        string curDir = GetCurrentPath();
        string[] splitPath = curDir.Split('/');
        _folder = splitPath[splitPath.Length - 1];
        CreateViewMediator(splitPath, curDir);

        AssetDatabase.Refresh();


    }

    private void CreateViewMediator(string[] splitPath, string curDir)
    {

        string ViewPath = curDir + "/" + Capitalize(_name) + "View" + ".cs";
        string MediatorPath = curDir + "/" + Capitalize(_name) + "Mediator" + ".cs";

        string ClassName = Capitalize(_name);
        string nameSpace = _folder;


        string viewTemplate =
        "using UnityEngine;\n" +
        "using Core;\n" +
        "using System.Collections;\n" +
        "using System.Collections.Generic;\n" +
        "namespace " + nameSpace + "\n" +
        "{\n" +
        "\tpublic class " + ClassName + ("View: MonoBehaviour,IView\n") +
        "\t{\n\n" +
        "\t\t///  INSPECTOR VARIABLES       ///\n\n" +
        "\t\t///  PRIVATE VARIABLES         ///\n\n" +
        "\t\t///  PRIVATE METHODS           ///\n\n" +
        "\t\t///  PUBLIC API                ///\n\n" +

        "\t}\n" +
        "}";

        string mediatorTemplate =
       "using UnityEngine;\n" +
       "using Core;\n" +
        "using Zenject;\n" +
       "using UniRx;\n" +
       "using System;\n" +
       "using System.Collections;\n" +
       "using System.Collections.Generic;\n" +
       "namespace " + nameSpace + "\n" +
       "{\n" +
       "\tpublic class " + ClassName + ("Mediator: MediatorBase<" + ClassName + "View>, IInitializable, IDisposable\n") +
       "\t{\n\n" +
       "\t\t///  INSPECTOR VARIABLES       ///\n\n" +
       "\t\t///  PRIVATE VARIABLES         ///\n\n" +
       "\t\t///  PRIVATE METHODS           ///\n\n" +
       "\t\t///  LISTNER METHODS           ///\n\n" +
       "\t\t///  PUBLIC API                ///\n\n" +
       "\t\t///  IMPLEMENTATION            ///\n\n" +
       "\t\t[Inject]\n\n" +
       "\t\tprivate SignalBus _signalBus;\n\n" +
       "\t\treadonly CompositeDisposable _disposables = new CompositeDisposable();\n\n" +
       "\t\tpublic void Initialize()\n" +
       "\t\t{\n\n" +
       "\t\t}\n\n" +
       "\t\tpublic void Dispose()\n" +
       "\t\t{\n\n" +
       "\t\t\t_disposables.Dispose();\n\n" +
       "\t\t}\n\n" +
       "\t}\n" +
       "}";
        Debug.Log(ViewPath);
        using (StreamWriter view = File.CreateText(ViewPath))
        {
            view.WriteLine(viewTemplate);

        }

        using (StreamWriter view = File.CreateText(MediatorPath))
        {
            view.WriteLine(mediatorTemplate);

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