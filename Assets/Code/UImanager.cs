using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    /*---------------------------------------------------------------------------------------------
    *  Copyright (c) Media4. All rights reserved.
    *  Attached to PowerUp objects.
    *  Manage PowerUps
    *--------------------------------------------------------------------------------------------*/

    [Header("References")]
    public static UImanager UIinst = null;
    [SerializeField] GameObject Panel_Select_File = null;
    [SerializeField] GameObject Panel_Select = null;
    [SerializeField] GameObject Panel_Graph = null;
    public Text Text_Graph = null;
    public Text Text_Range = null;


    // Start is called before the first frame update
    void Start()
    {
        UIinst = this;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        Panel_Select_File.SetActive(true);
        Panel_Select.SetActive(false);
        Panel_Graph.SetActive(false);

        Text_Graph.text = "";
        Text_Range.text = "";
    }

    public void Set_Select_File()
    {
        Panel_Select_File.SetActive(true);
        Panel_Select.SetActive(false);
        Panel_Graph.SetActive(false);
    }

    public void Set_Select()
    {
        Panel_Select_File.SetActive(false);
        Panel_Select.SetActive(true);
        Panel_Graph.SetActive(false);
    }

    public void Set_Graph()
    {
        Panel_Select_File.SetActive(false);
        Panel_Select.SetActive(false);
        Panel_Graph.SetActive(true);
    }

    public void Button_Select_File(string str)
    {
        Parser.parseInstr.Parse_OBD_data(str);
        Set_Select();
    }
}
