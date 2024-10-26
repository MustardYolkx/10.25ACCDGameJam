using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartPanel : BasePanel
{
    private static string name = "StartPanel";
    private static string path = "UIPanel/StartPanel";

    public static readonly UI_Type uI_Type = new UI_Type(path, name);

    public string currentInput;


    public StartPanel() : base(uI_Type)
    {

    }

    public override void OnStart()
    {
        base.OnStart();
        UI_Method.GetInstance().GetOrAddComponentInChild<DoubleClickButton>(ActiveObj, "Unity_DoubleClickButton").OnDoubleClick.AddListener(OpenUnity);
        UI_Method.GetInstance().GetOrAddComponentInChild<Button>(ActiveObj, "MyFile").onClick.AddListener(OpenMyFile);

        IsFile[] gameObjects =ActiveObj.GetComponentsInChildren<IsFile>();
        foreach(IsFile f in gameObjects)
        {
            GameRoot.GetInstance().computerFile_Dictionary.Add(f.fileName, f.gameObject);
        }
        

       
    }
    
    private void OpenUnity()
    {

        GameRoot.GetInstance().UIManager_Root.Push(new UnityPanel());
        //Scene2 scene2 = new Scene2();
        //GameRoot.GetInstance().SceneControl_Root.LoadScene(scene2.SceneName, scene2);
    }

    private void OpenMyFile()
    {

    }



    private void Close()
    {
        GameRoot.GetInstance().UIManager_Root.Pop(false);
    }
    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
