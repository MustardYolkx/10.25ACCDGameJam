using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel
{
    public UI_Type uiType;

    /// <summary>
    /// Corresponding object in the scene for this panel
    /// </summary>
    public GameObject ActiveObj;

    public BasePanel(UI_Type uitype)
    {
        uiType= uitype;
    }

    public virtual void OnStart()
    {
        Debug.Log($"{uiType.Name} Start working!");
        UI_Method.GetInstance().AddOrGetComponent<CanvasGroup>(ActiveObj).interactable = true;
    }

    public virtual void OnEnable()
    {
        UI_Method.GetInstance().AddOrGetComponent<CanvasGroup>(ActiveObj).interactable = true;
    }
    public virtual void OnDisable()
    {
        UI_Method.GetInstance().AddOrGetComponent<CanvasGroup>(ActiveObj).interactable = false;
    }

    public virtual void OnDestroy()
    {
        UI_Method.GetInstance().AddOrGetComponent<CanvasGroup>(ActiveObj).interactable = false;
    }
}