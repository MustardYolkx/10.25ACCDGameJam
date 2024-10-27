using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsFile : MonoBehaviour
{

    public string fileName;

    public bool hasVirus;

    public float virusInfectSpeed;
    public float currentProcess;

    private GameObject virusProcess;
    // Start is called before the first frame update

    public void Awake()
    {
        if (transform.Find("VirusProcess").gameObject != null)
        {
            virusProcess = transform.Find("VirusProcess").gameObject;

        }
    }
    void Start()
    {
        virusInfectSpeed = GameRoot.GetInstance().virusOriginSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        AddVirusInfectProcess();
    }

    public void AddVirusInfectProcess()
    {
        if(hasVirus)
        {
            currentProcess += virusInfectSpeed * Time.deltaTime - GameRoot.GetInstance().antiVirusKillingSpeed * Time.deltaTime;
            virusProcess.GetComponent<Image>().fillAmount = currentProcess / 1;
        }
        if (currentProcess < 0)
        {
            currentProcess = 0;
        }
        else if(currentProcess >1)
        {
            currentProcess = 1;

            Sprite targetSprite = Resources.Load<Sprite>("Sprite/Icon/" + fileName + "_Virus");
          
            GameRoot.GetInstance().computerFile_Dictionary[fileName].GetComponent<Image>().sprite = targetSprite;
        }
    }
}
