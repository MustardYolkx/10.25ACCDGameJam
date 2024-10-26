using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Method 
{
    private static UI_Method instance;

    public static UI_Method GetInstance()
    {
        if(instance == null)
        {
            instance= new UI_Method();
            
        }
        return instance;
    }
    /// <summary>
    /// Get canvas in game scene
    /// </summary>
    /// <returns></returns>
    public GameObject FindCanvas()
    {
        GameObject gameObject = GameObject.FindObjectOfType<Canvas>().gameObject;
        if(gameObject == null )
        {
            Debug.LogError("Can't find canvas in game scene");
            return gameObject;
        }
        return gameObject;
    }

    public GameObject FindObjectInChild(GameObject panel,string childName)
    {
        Transform[] transform = panel.GetComponentsInChildren<Transform>();

        foreach(var tra in transform)
        {
            if(tra.gameObject.name == childName)
            {
                return tra.gameObject;
            }
        }

        Debug.LogError($"Can't find{childName} in {panel.name}");
        return null;
    }
    /// <summary>
    /// Add of get Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Get_Obj"></param>
    /// <returns></returns>
    public T AddOrGetComponent<T>(GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>() != null)
        {
            return gameObject.GetComponent<T>();
        }
        else
        {
            Debug.LogError($"Can't get component from{gameObject}");
            gameObject.AddComponent<T>();
            return gameObject.GetComponent<T>();
        }
        
    }

    public T GetOrAddComponentInChild<T>(GameObject gameObject, string childName) where T : Component
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();

        foreach (var tra in transforms)
        {
            if (tra.gameObject.name == childName)
            {
                if(tra.GetComponent<T>() != null)
                {
                    return tra.gameObject.GetComponent<T>();
                }
                else
                {
                    tra.gameObject.AddComponent<T>();
                    return tra.gameObject.GetComponent<T>();
                }
            }
        }

        Debug.LogError($"Can't find{childName} In {gameObject.name}");
        return null;
    }

    /// <summary>
    /// Get current NPC List in this game scene, return a List<string>
    /// </summary>
    /// <returns></returns>
    //public List<string> GetNPCList()
    //{       
    //    List<string> list = new List<string>();
    //    Scene_NPCList npcList = GameObject.FindObjectOfType<Scene_NPCList>();
    //    foreach (string i in npcList.npcList)
    //    {
    //        list.Add(i);
    //    }
    //        return list;
    //}

    /// <summary>
    /// Instantiate the NPC List button
    /// </summary>
    /// <param name="npclist"> It's the npc list</param>
    /// <param name="listpanel">the target panel to show the buttons</param>
    /// <param name="optionButton">the npc interact button</param>
    /// <returns></returns>
    public Dictionary<string,GameObject> InstantiateNPCListButton(List<string> npclist,GameObject listpanel,GameObject optionButton)
    {
        Dictionary<string, GameObject> dic = new Dictionary<string, GameObject>();
        for(int i = 0; i < npclist.Count; i++) 
        { 
           GameObject button = GameObject.Instantiate<GameObject>(optionButton, listpanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = npclist[i];
            //button.GetComponent<Button>().onClick.AddListener();
           dic.Add(npclist[i], button);           
        }
        return dic;
    }

    /// <summary>
    /// automatically connect to corresponded dialogue
    /// </summary>
    /// <param name="dic">the npc dictionary, contains npc name and corresponded dialogue</param>
    /// <returns></returns>
    //public BaseDialogue FindDialogueInButton(Dictionary<string, GameObject> dic)
    //{
    //    GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
    //    string name= clickedButton.GetComponentInChildren<TextMeshProUGUI>().text;
    //    return GameRoot.GetInstance().Dialog_Dictionary_Root.dict_dialogue[name];
    //    //GameObject button = dic[clickedButton.name];
        
    //    //return button.GetComponent<BaseDialogue>();
    //}
}
