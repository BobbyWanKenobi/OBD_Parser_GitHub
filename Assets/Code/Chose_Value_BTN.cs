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
    [SerializeField] Image Button_Thickness_Image = null;
    [SerializeField] Parser parser = null;

    [Header("Variables")]
    public int ID = 0;
    public bool selected = true;
    public bool thicker = false;


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

        //Thickness
        thicker = false;

        Set_Button_Color();
        Set_Button_Thickness_Color();
    }

    public void Button_Pressed()
    {
        selected = !selected;
        Set_Button_Color();
        Set_Button_Thickness_Color();
    }

    public void Button_Thicknes_Pressed()
    {
        thicker = !thicker;
        Set_Button_Thickness_Color();
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
            Button_Image.color = new Color(1.0f, 0.7f, 0.0f);
        }
        else
        {
            Button_Image.color = Color.green;
            thicker = false;
        }
    }

    void Set_Button_Thickness_Color()
    {
        if (thicker)
        {
            Button_Thickness_Image.color = new Color(1.0f, 0.7f, 0.0f);
        }
        else
        {
            Button_Thickness_Image.color = Color.green;
        }
    }
}
