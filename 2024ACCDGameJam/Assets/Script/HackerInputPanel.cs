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


    //Call verify prefab
    //public GameObject verifyPrefab;
    //private GameObject activeVerifyInstance;

    //public GameObject qteGamePrefab;
    //public GameObject spiningPuzzlePrefab;






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


        //RegisterMethod("openunity", new Action(OpenPanel));
        //RegisterMethod("closepage", new Action(() => { ClosePage(currentFileName); }));
    }
    private void OnInputValueChanged(string value)
    {
        
        Debug.Log("Input Changed：" + value.ToLower());
    }
    private void OpenFiles(string inputContent)
    {
        if (inputContent != null)
        {
            currentInputNoEdit = inputContent;
            currentInput = inputContent.ToLower();
           
        }
        else
        {
            
        }
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
            OpenPanel(inputContent[1]);
            //InvokeByDictionary(content);
            AddInputToText(currentInputNoEdit, true);
            ClearInputField();
            CheckIfBeCatchedByOwner();
        }
        else if (inputContent.Contains("copy")&&( inputContent.Contains("virus")|| inputContent.Contains("rootvirus")))
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
        else if(inputContent.Contains("infect") && inputContent.Contains("virus"))
        {
            if(gotARootVirus)
            {
                foreach (string name in GameRoot.GetInstance().computerFile_Dictionary.Keys)
                {
                    if (name == inputContent[2])
                    {

                        AddInputToText(currentInputNoEdit, true);
                        ClearInputField();
                        
                        //generate verify prefab
                        GameObject verifyPrefab = Resources.Load<GameObject>("Prefab/MiniGamePrefeb/weaVerifyPrefab");
                        GameObject verifyInstance = Instantiate(verifyPrefab, GameRoot.GetInstance().UIManager_Root.canvasObj.transform);

                        verifyInstance.GetComponentInChildren<Verify>().OnVerificationComplete += () =>
                        {
                            Debug.Log("Add Delegate");
                            // delete verify prefab
                            Destroy(verifyInstance);
                            CheckIfBeCatchedByOwner();
                            //InstantiateAProcessBar
                            GameObject processBar = Resources.Load<GameObject>("Prefab/VirusInstallProcessBar");
                            GameObject bar = Instantiate(processBar, GameRoot.GetInstance().UIManager_Root.canvasObj.transform);
                            bar.GetComponent<VirusInstallProcessBar_UI>().hackerInput = this;
                            bar.GetComponent<VirusInstallProcessBar_UI>().contentFromHackInput = name;
                        };


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
            CheckIfBeCatchedByOwner();
        }

        //level up virus, call minigames
        else if (inputContent.Contains("level") && inputContent.Contains("up") && inputContent.Contains("virus"))
        {
            LevelUpVirus();
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

        Sprite targetSprite = Resources.Load<Sprite>("Sprite/Icon/" + content + "_Virus");
        GameRoot.GetInstance().computerFile_Dictionary[content].GetComponentInChildren<IsFile>().hasVirus = true;
        GameRoot.GetInstance().computerFile_Dictionary[content].GetComponent<Image>().sprite = targetSprite;
        
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
    private void OpenPanel(string name)
    {
        GameObject tagetPanel = Resources.Load<GameObject>("UIPanel/"+name+"Panel");
        GameObject pageOBJ = Instantiate(tagetPanel, GameRoot.GetInstance().UIManager_Root.canvasObj.transform);
        //pageOBJ.GetComponent<PageInfo>().fileInfo = GameRoot.GetInstance().computerFile_Dictionary[name].GetComponent<IsFile>();
        GameRoot.GetInstance().currentOpenFile_Dictionary.Add(pageOBJ.GetComponent<PageInfo>().fileName, pageOBJ);
        
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

    public void CheckIfBeCatchedByOwner()
    {
        if(GameRoot.GetInstance().ownerScr.currentState == OwnerAI.OwnerStates.Active)
        {
            GameRoot.GetInstance().ownerScr.SpeedUpAntiVirusSystem();
        }
    }

    void LevelUpVirus()
    {
        // Load the two mini-game prefabs
        GameObject miniGame1Prefab = Resources.Load<GameObject>("Prefab/MiniGamePrefeb/MiniGame1");
        GameObject miniGame2Prefab = Resources.Load<GameObject>("Prefab/MiniGamePrefeb/MiniGame2");

        // Randomly select one of the two prefabs
        GameObject selectedMiniGame = (UnityEngine.Random.Range(0, 2) == 0) ? miniGame1Prefab : miniGame2Prefab;

        // Instantiate the selected mini-game on the UI canvas
        GameObject miniGameInstance = Instantiate(selectedMiniGame, GameRoot.GetInstance().UIManager_Root.canvasObj.transform);

        if (selectedMiniGame == miniGame1Prefab)
        {
            qteGame qteScript = miniGameInstance.GetComponent<qteGame>();
            qteScript.OnMiniGameSuccess += () =>
            {
                StartCoroutine(DestroyMiniGameInstanceWithDelay(miniGameInstance));
                SpeedUpInfection();
            };
        }
        else if (selectedMiniGame == miniGame2Prefab)
        {
            SpinUI spinScript = miniGameInstance.GetComponent<SpinUI>();
            spinScript.OnMiniGameSuccess += () =>
            {
                StartCoroutine(DestroyMiniGameInstanceWithDelay(miniGameInstance));
                SpeedUpInfection();
            };
        }
    }

    private IEnumerator DestroyMiniGameInstanceWithDelay(GameObject miniGameInstance)
    {
        yield return new WaitForSeconds(1f); // 等待1秒
        Destroy(miniGameInstance);
    }


    void SpeedUpInfection()
    {
        Debug.Log("Infection sped up for the current file!");
        // 加速感染过程
        foreach(IsFile f in GameRoot.GetInstance().file_List)
        {
            f.virusInfectSpeed += 0.05f;
            GameRoot.GetInstance().virusOriginSpeed += 0.05f;
        }
    }
}
