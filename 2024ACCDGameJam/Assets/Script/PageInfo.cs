using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PageInfo : MonoBehaviour
{

    public string fileName;

    public bool hasVirus;

    public float virusInfectSpeed;
    public float currentProcess;

    public TextMeshProUGUI text;
    private GameObject virusProcess;

    public IsFile fileInfo;
    // Start is called before the first frame update
    void Start()
    {
        foreach(string name in GameRoot.GetInstance().computerFile_Dictionary.Keys)
        {
            if(name == fileName) 
            {
                fileInfo = GameRoot.GetInstance().computerFile_Dictionary[name].GetComponent<IsFile>();
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (fileInfo.currentProcess*100).ToString("0");
    }
}
