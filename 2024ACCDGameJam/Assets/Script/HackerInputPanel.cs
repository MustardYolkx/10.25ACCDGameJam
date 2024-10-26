using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HackerInputPanel : MonoBehaviour
{

    public RectTransform rootVirusInstantiatePos;

    public bool gotARootVirus;
    private string currentInput;
    private string currentInputNoEdit;
    private string currentFileName;
    private TMP_InputField inputField;

    private TextMeshProUGUI inputHistory;
    private void Start()
    {
        inputField = UI_Method.GetInstance().GetOrAddComponentInChild<TMP_InputField>(gameObject, "HackerInput");
        inputHistory = UI_Method.GetInstance().GetOrAddComponentInChild<TextMeshProUGUI>(gameObject, "InputHistory");
        UI_Method.GetInstance().GetOrAddComponentInChild<TMP_InputField>(gameObject, "HackerInput").ActivateInputField();
        inputField.onValueChanged.AddListener(OnInputValueChanged);
        UI_Method.GetInstance().GetOrAddComponentInChild<TMP_InputField>(gameObject, "HackerInput").onSubmit.AddListener((str) => {
            OpenFiles(str);
        });


        RegisterMethod("openunity", new Action(OpenUnity));
        //RegisterMethod("closepage", new Action(() => { ClosePage(currentFileName); }));
    }
    private void OnInputValueChanged(string value)
    {
        
        Debug.Log("Input Changed£º" + value.ToLower());
    }
    private void OpenFiles(string inputContent)
    {
        if (inputContent != null)
        {
            currentInputNoEdit = inputContent;
            currentInput = inputContent.ToLower();
           
        }
        Debug.Log("1");
        Debug.Log(currentInput);
        OperateInputMessage();
    }

    public void OperateInputMessage()
    {
        string[] splitSymbol = { " ", "(", ")" };
        string[] inputContent = currentInput.Split(splitSymbol, StringSplitOptions.None);
        string content = "";
        if (inputContent.Contains("open"))
        {
            for (int i = 0; i < inputContent.Length; i++)
            {
                content += inputContent[i];
                Debug.Log(content);
                
            }
            
            InvokeByDictionary(content);
            AddInputToText(currentInputNoEdit, true);
            ClearInputField();
        }
        else if (inputContent.Contains("copy")&& inputContent.Contains("rootvirus"))
        {
            if(!gotARootVirus)
            {
                GameObject rootVirus = Resources.Load<GameObject>("Prefab/RootVirus");
                Instantiate(rootVirus, rootVirusInstantiatePos);
                AddInputToText(currentInputNoEdit, true);
                ClearInputField();
                gotARootVirus = true;
            }
            else
            {
                AddInputToText("Already got a RootVirus", false);
            }
            
        }
        else if(inputContent.Contains("infect") && inputContent.Contains("subvirus"))
        {
            if(gotARootVirus)
            {
                foreach (string name in GameRoot.GetInstance().computerFile_Dictionary.Keys)
                {
                    if (name == inputContent[2])
                    {
                        AddInputToText(currentInputNoEdit, true);
                        ClearInputField();
                        //InstantiateAProcessBar
                        GameObject processBar = Resources.Load<GameObject>("Prefab/VirusInstallProcessBar");
                        GameObject bar = Instantiate(processBar,GameRoot.GetInstance().UIManager_Root.canvasObj.transform);
                        bar.GetComponent<VirusInstallProcessBar_UI>().hackerInput = this;
                        bar.GetComponent<VirusInstallProcessBar_UI>().contentFromHackInput = name;
                        //InfectVirus(name);
                    }
                }
            }
            else
            {

            }
        }
        else if (inputContent.Contains("close"))
        {
            for (int i = 0; i < inputContent.Length; i++)
            {
                content += inputContent[i];
                Debug.Log(content);

            }
            
            ClosePage(inputContent[1]);
            AddInputToText(currentInputNoEdit, true);
            ClearInputField();
        }
        else
        {
            AddInputToText(currentInputNoEdit,false);
            ClearInputField();
        }
    }
    
    public void InfectVirus(string content)
    {

        Sprite targetSprite = Resources.Load<Sprite>("Sprite/" + content + "_Virus");
        GameRoot.GetInstance().computerFile_Dictionary[content].GetComponentInChildren<IsFile>().hasVirus = true;
        GameRoot.GetInstance().computerFile_Dictionary[content].GetComponentInChildren<Image>().sprite = targetSprite;
        
    }
    public void AddInputToText(string content,bool isCorrect)
    {
        if(isCorrect)
        {
            inputHistory.text += "\n"  + "<color=white>" + content;
        }
        else
        {
            inputHistory.text += "\n" + "<color=red>" + content;
        }
        
    }

    public void ClearInputField()
    {
        inputField.text = "";
    }
    #region Invoke Function by string
    private Dictionary<string, Delegate> methodDictionary = new Dictionary<string, Delegate>();


    public void RegisterMethod(string methodName, Delegate method)
    {
        methodDictionary[methodName] = method;
    }


    public object InvokeByDictionary(string methodName, params object[] parameters)
    {
        if (methodDictionary.TryGetValue(methodName, out Delegate method))
        {
            try
            {
                return method.DynamicInvoke(parameters);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error invoking method {methodName}: {e.Message}");
                return null;
            }
        }

        Debug.LogError($"Method {methodName} not registered!");
        return null;
    }
    #endregion
    private void OpenUnity()
    {
        GameObject UnityPanel = Resources.Load<GameObject>("UIPanel/UnityPanel");
        GameObject unityPage = Instantiate(UnityPanel, GameRoot.GetInstance().UIManager_Root.canvasObj.transform);
        GameRoot.GetInstance().currentOpenFile_Dictionary.Add(unityPage.GetComponent<PageInfo>().fileName, unityPage);
        
        //GameRoot.GetInstance().UIManager_Root.Push(new UnityPanel());
        //Scene2 scene2 = new Scene2();
        //GameRoot.GetInstance().SceneControl_Root.LoadScene(scene2.SceneName, scene2);
    }

    private void ClosePage(string name)
    {
        Destroy(GameRoot.GetInstance().currentOpenFile_Dictionary[name]);
        GameRoot.GetInstance().currentOpenFile_Dictionary.Remove(name);

        //GameRoot.GetInstance().UIManager_Root.Push(new UnityPanel());
        //Scene2 scene2 = new Scene2();
        //GameRoot.GetInstance().SceneControl_Root.LoadScene(scene2.SceneName, scene2);
    }
}
