using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityPanel : BasePanel
{
    private static string name = "UnityPanel";
    private static string path = "UIPanel/UnityPanel";

    public static readonly UI_Type uI_Type = new UI_Type(path, name);

    public UnityPanel() : base(uI_Type)
    {

    }

    public override void OnStart()
    {
        UI_Method.GetInstance().GetOrAddComponentInChild<Button>(ActiveObj, "Close").onClick.AddListener(Close);
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

    private void Close()
    {
        GameRoot.GetInstance().UIManager_Root.Pop(false);
    }
}
