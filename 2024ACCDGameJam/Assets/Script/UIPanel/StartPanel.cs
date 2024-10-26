using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : BasePanel
{
    private static string name = "StartPanel";
    private static string path = "Panel/JiUPanel/GameStartPanel";

    public static readonly UI_Type uI_Type = new UI_Type(path, name);

    public GameStartPanel() : base(uI_Type)
    {

    }

    public override void OnStart()
    {
        base.OnStart();
        UI_Method.GetInstance().GetOrAddComponentInChild<Button>(ActiveObj, "StoryModeButton").onClick.AddListener(OpenStoryModePanel);
        UI_Method.GetInstance().GetOrAddComponentInChild<Button>(ActiveObj, "VSModeButton").onClick.AddListener(OpenVSPanel);
        UI_Method.GetInstance().GetOrAddComponentInChild<Button>(ActiveObj, "OptionButton").onClick.AddListener(OpenOptionPanel);
        UI_Method.GetInstance().GetOrAddComponentInChild<Button>(ActiveObj, "LeaveButton").onClick.AddListener(Close);
    }

    private void OpenStoryModePanel()
    {
        //Scene2 scene2 = new Scene2();
        //GameRoot.GetInstance().SceneControl_Root.LoadScene(scene2.SceneName, scene2);
    }

    private void OpenVSPanel()
    {

    }

    private void OpenOptionPanel()
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
