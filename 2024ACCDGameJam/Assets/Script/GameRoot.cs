using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    
    private UI_Manager uiManager;
    public UI_Manager UIManager_Root { get => uiManager; }

    public List<string> fileNames = new List<string>();

    private static GameRoot instance;
    public Dictionary<string, GameObject> currentOpenFile_Dictionary = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> computerFile_Dictionary = new Dictionary<string, GameObject>();

    public float antiVirusKillingSpeed = 0f;
    public bool isAntiSystemKilling;
    public float virusOriginSpeed = 0.1f;

    public AntivirusSystem antivirusSystem;
    public GameObject cursor;
    public Transform idlePos;

    public OwnerAI ownerScr;


    public float entireProcess;

    public List<IsFile> file_List;
    public static GameRoot GetInstance()
    {
        if(instance == null)
        {
            Debug.LogError("Can't get Gameroot");
            return instance;
        }
        return instance;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        uiManager = new UI_Manager();
        ownerScr= FindObjectOfType<OwnerAI>();
        antivirusSystem = GetComponent<AntivirusSystem>();
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        UIManager_Root.canvasObj = UI_Method.GetInstance().FindCanvas();
        StartCoroutine(CheckEntireProcess());
        //Scene1 scene1 = new Scene1();

        //SceneControl_Root.dict_scene.Add(scene1.SceneName, scene1);

        //NPC1_Dialogue npc1 = new NPC1_Dialogue();
        //Dialog_Dictionary.dict_dialogue.Add("NPC1", npc1);

        #region Push First Panel
        UIManager_Root.Push(new StartPanel());

        #endregion

        Instantiate(cursor, uiManager.canvasObj.transform, idlePos);
    }

    IEnumerator CheckEntireProcess()
    {
        while (true)
        {
            float totalPercent = 0;
            for (int i = 0; i < file_List.Count; i++)
            {
                totalPercent += file_List[i].currentProcess;
            }
            entireProcess = totalPercent / file_List.Count;

            // 调用胜利条件检查
            CheckWinCondition();

            yield return new WaitForSeconds(0.2f); // 每0.2秒检查一次
        }
    }

    public void CheckWinCondition()
    {
        if(entireProcess >= 1)
        {
            GameWin();
        }
    }

    private void GameWin()
    {

        GameObject winPanel = Resources.Load<GameObject>("UIPanel/WinPanel");
        Instantiate(winPanel, uiManager.canvasObj.transform);
    }
}
