using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chose_Value_BTN : MonoBehaviour
{
    /*---------------------------------------------------------------------------------------------
    *  Copyright (c) Media4. All rights reserved.
    *  Attached to PowerUp objects.
    *  Manage PowerUps
    *--------------------------------------------------------------------------------------------*/

    [Header("References")]
    [SerializeField] Image Button_Image = null;
    [SerializeField] Text Button_Text = null;
    [SerializeField] Transform Parent = null;
    [SerializeField] Parser parser = null;

    [Header("Variables")]
    public int ID = 0;
    public bool selected = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_Button(int id, string str)
    {
        ID = id;
        Button_Text.text = str;

        //Parent to content
        transform.SetParent(Parent);

        //Set scale
        transform.localScale = Vector3.one;

        Set_Button_Color();
    }

    public void Button_Pressed()
    {
        selected = !selected;

        Set_Button_Color();
    }

    public void Select_Unselect(bool isselected)
    {
        selected = isselected;

        Set_Button_Color();
    }

    void Set_Button_Color()
    {
        if (selected)
        {
            Button_Image.color = Color.green;
        }
        else
        {
            Button_Image.color = Color.red;
        }
    }
}
