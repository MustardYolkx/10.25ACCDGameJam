using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusInstallProcessBar_UI : MonoBehaviour
{

    public HackerInputPanel hackerInput;

    public string contentFromHackInput;

    public float installSpeed;
    public float currentInstallProcess;
    public Image contentBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentInstallProcess += installSpeed * Time.deltaTime;
        contentBar.fillAmount = currentInstallProcess / 1;
        transform.SetAsLastSibling();
        if (currentInstallProcess > 1)
        {
            hackerInput.InfectVirus(contentFromHackInput);
            Destroy(gameObject);
        }
    }
}
