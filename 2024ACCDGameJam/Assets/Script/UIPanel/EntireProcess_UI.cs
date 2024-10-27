using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntireProcess_UI : MonoBehaviour
{
    public TextMeshProUGUI percentText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        percentText.text = (GameRoot.GetInstance().entireProcess*100).ToString("F2");
    }
}
