using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    
    private UI_Manager uiManager;
    public UI_Manager UIManager_Root { get => uiManager; }

    

    private static GameRoot instance;
    public Dictionary<string, GameObject> currentOpenFile_Dictionary = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> computerFile_Dictionary = new Dictionary<string, GameObject>();

    public float virusOriginSpeed = 0.1f;
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
        
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        UIManager_Root.canvasObj = UI_Method.GetInstance().FindCanvas();

        //Scene1 scene1 = new Scene1();
        
        //SceneControl_Root.dict_scene.Add(scene1.SceneName, scene1);

        //NPC1_Dialogue npc1 = new NPC1_Dialogue();
        //Dialog_Dictionary.dict_dialogue.Add("NPC1", npc1);

        #region Push First Panel
        UIManager_Root.Push(new StartPanel());

        #endregion
    }
}
