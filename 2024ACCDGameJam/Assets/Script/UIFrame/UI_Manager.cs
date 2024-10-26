using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager 
{
    /// <summary>
    /// The stack to store UI_Panel
    /// </summary>
    public Stack<BasePanel> stack_ui;
    /// <summary>
    /// store the panel name correspond to the obj
    /// </summary>
    public Dictionary<string, GameObject> dict_uiObj;
    /// <summary>
    /// Canvas in this scene
    /// </summary>
    public GameObject canvasObj;

    private static UI_Manager instance;

    public static UI_Manager GetInstance()
    {
        if(instance == null)
        {
            Debug.LogError("UI_Manager doesn't exist");
            return instance;
        }
        else
        {
            return instance;
        }
    }

    public UI_Manager()
    {
        instance= this;
        stack_ui = new Stack<BasePanel>();
        dict_uiObj= new Dictionary<string, GameObject>();
    }
    public GameObject GetSingleObject(UI_Type uiType)
    {
        if(dict_uiObj.ContainsKey(uiType.Name))
        {
            return dict_uiObj[uiType.Name];
        }
        if (canvasObj == null)
        {
            canvasObj = UI_Method.GetInstance().FindCanvas();

        }

        GameObject gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(uiType.Path),canvasObj.transform);
        return gameObject;
    }
    /// <summary>
    /// Push a panel to the stack
    /// </summary>
    /// <param name="basePanel_push">the panel you want to push</param>
    public void Push(BasePanel basePanel_push)
    {
        Debug.Log($"{basePanel_push.uiType.Name}has been pushed");

        if (stack_ui.Count > 0)
        {
            stack_ui.Peek().OnDisable();
        }

        GameObject ui_object = GetSingleObject(basePanel_push.uiType);
        dict_uiObj.Add(basePanel_push.uiType.Name, ui_object);
        basePanel_push.ActiveObj = ui_object;

        if(stack_ui.Count == 0)
        {
            stack_ui.Push(basePanel_push);
        }
        else
        {
            if (stack_ui.Peek().uiType.Name != basePanel_push.uiType.Name)
            {
                stack_ui.Push(basePanel_push);
            }
        }

        basePanel_push.OnStart();
    }
    /// <summary>
    /// Pop out a panel from stack
    /// </summary>
    /// <param name="isLoad">when true, pop out all the panels in stack</param>
    public void Pop(bool isLoad)
    {
        if (isLoad)
        {
            if(stack_ui.Count > 0)
            {
                stack_ui.Peek().OnDisable();
                stack_ui.Peek().OnDestroy();
                GameObject.Destroy(dict_uiObj[stack_ui.Peek().uiType.Name]);
                dict_uiObj.Remove(stack_ui.Peek().uiType.Name);
                stack_ui.Pop();
                Pop(true);
            }
        }
        else
        {
            stack_ui.Peek().OnDisable();
            stack_ui.Peek().OnDestroy();
            GameObject.Destroy(dict_uiObj[stack_ui.Peek().uiType.Name]);
            dict_uiObj.Remove(stack_ui.Peek().uiType.Name);
            stack_ui.Pop();

            if (stack_ui.Count > 0)
            {
                stack_ui.Peek().OnEnable();
            }
            
        }
    }
}


