using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct OBDHeader
{
    public int ID;
    public string ValueName;
    public string Units;
    public bool RangeSet;
    public float RangeMin;
    public float RangeMax;
    public string Average;
}

public struct OBDdata
{
    //public int ID;
    public float Value;
}

public class Parser : MonoBehaviour
{
    /*---------------------------------------------------------------------------------------------
    *  Copyright (c) Media4. All rights reserved.
    *  Attached to PowerUp objects.
    *  Manage PowerUps
    *--------------------------------------------------------------------------------------------*/

    [Header("References")]
    public static Parser parseInstr = null;
    [SerializeField] Chose_Value_BTN chose_Value_BTN = null;
    [SerializeField] GameObject Line = null;

    [Header("Variables")]
    public OBDHeader[] OBD_Header;
    public float[,] OBD_Data;
    List<GameObject> Buttons_List = new List<GameObject>();
    public List<GameObject> Lines_List;

    [Header("File related")]
    public string File_Name = "NISSAN(2)";

    private void Awake()
    {
        parseInstr = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Parse_OBD_data();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Parse_OBD_data(string fileName)
    {
        File_Name = fileName;

        Destroy_All_Buttons();

        var csvData = Resources.Load<TextAsset>(File_Name);
        string fileData = csvData.ToString();

        //Slit string into lines
        string[] lines = fileData.Split("\n"[0]);

        Debug.Log("Parser.Parse_OBD_data, lines.Lenght: " + lines.Length + "  > lines[0] = " + lines[0]);

        Parse_Header(lines[0]);
        Parse_Data(lines);
    }

    void Parse_Header(string str)
    {
        //Slit string into lines
        string[] stringSeparators = new string[] { "{\"ID\":" };
        string[] result = str.Split(stringSeparators, StringSplitOptions.None);

        //Debug.Log("Parser.Parse_Header, result.Lenght: " + result.Length);

        OBD_Header = new OBDHeader[result.Length - 1];

        for (int x = 1; x < result.Length; x++)
        {
            //Debug.Log(result[x]);

            //Parse ID
            string[] part = result[x].Split(","[0]);
            int id = ParseInt(part[0]);
            //Debug.Log("id = " + id);

            //Parse ValueName
            stringSeparators = new string[] { "\"LB\":\"" };
            part = result[x].Split(stringSeparators, StringSplitOptions.None);
            string[] part2 = part[1].Split("\""[0]);
            string valueName = part2[0];
            //Debug.Log("valueName = " + valueName);

            //Parse Units
            stringSeparators = new string[] { "\"UNT\":\"" };
            part = result[x].Split(stringSeparators, StringSplitOptions.None);
            part2 = part[1].Split("\""[0]);
            string units = part2[0];
            //Debug.Log("units = " + units);

            //Parse RangeMin - RangeMax
            stringSeparators = new string[] { "\"RV\":\"" };
            part = result[x].Split(stringSeparators, StringSplitOptions.None);
            part2 = part[1].Split("\""[0]);
            string[] part3 = part2[0].Split(","[0]);
            float rangeMin = 0;
            float rangeMax = 0;
            bool rangeSet = false;
            if (part3.Length > 1)
            {
                rangeMin = ParseFloat(part3[0]);
                rangeMax = ParseFloat(part3[1]);
                rangeSet = true;
            }
            //Debug.Log("rangeMin = " + rangeMin + "  > rangeMax = " + rangeMax);

            //Parse Units
            string average = "";
            stringSeparators = new string[] { "\"VA\":\"" };
            part = result[x].Split(stringSeparators, StringSplitOptions.None);
            if (part.Length > 1)
            {
                part2 = part[1].Split("\""[0]);
                if (part3.Length > 0)
                    average = part2[0];
            }
            Debug.Log("Average = " + average);

            //Assign Values
            OBD_Header[x - 1].ID = id;
            OBD_Header[x - 1].ValueName = valueName;
            OBD_Header[x - 1].Units = units;
            OBD_Header[x - 1].RangeMin = rangeMin;
            OBD_Header[x - 1].RangeMax = rangeMax;
            OBD_Header[x - 1].RangeSet = rangeSet;
            OBD_Header[x - 1].Average = average;
            Debug.Log("OBD_Header[" + (x - 1) + "].ID = " + OBD_Header[x - 1].ID + "  > OBD_Header[" + (x - 1) + "].ValueName = " + OBD_Header[x - 1].ValueName +
                "  > OBD_Header[" + (x - 1) + "].Units = " + OBD_Header[x - 1].Units +
                "  > OBD_Header[" + (x - 1) + "].RangeMin = " + OBD_Header[x - 1].RangeMin + "  > OBD_Header[" + (x - 1) + "].RangeMax = " + OBD_Header[x - 1].RangeMax +
                "  > Average = " + average);
        }

        Set_Buttons();

        Debug.Log("OBD_Header.Lenght = " + OBD_Header.Length);
    }

    void Set_Buttons()
    {
        string str = "0. , ID" + OBD_Header[0].ID + ": " + OBD_Header[0].ValueName + " > Range " + OBD_Header[0].RangeMin + " / " + OBD_Header[0].RangeMax + " ,UNIT: " + OBD_Header[0].Units;
        chose_Value_BTN.Set_Button(0, str);

        Buttons_List.Add(chose_Value_BTN.gameObject);

        for (int x = 1; x < OBD_Header.Length; x++)
        {
            string range = "";
            if (OBD_Header[x].RangeSet)
                range = " > Range: " + OBD_Header[x].RangeMin + " / " + OBD_Header[x].RangeMax;

            string average = "";
            if (OBD_Header[x].Average != "")
                average = " > Avr: " + OBD_Header[x].Average;

            str = x + ". , ID" + OBD_Header[x].ID + ": " + OBD_Header[x].ValueName + range + " ,UNIT: " + OBD_Header[x].Units + average;
            GameObject tmp = Instantiate(chose_Value_BTN.gameObject);
            tmp.GetComponent<Chose_Value_BTN>().Set_Button(OBD_Header[x].ID, str);
            Buttons_List.Add(tmp);
        }
    }

    void Parse_Data(string[] str)
    {
        //COUNT VALID DATA
        int dataCount = 0;
        for (int x = 1; x < str.Length; x++)
        {
            string[] part = str[x].Split("[\""[0]);
            if (part.Length > 1)
                dataCount++;
        }

        Debug.Log("dataCount = " + dataCount);

        //PARSE and ASSIGN DATA
        OBD_Data = new float[dataCount, OBD_Header.Length];

        int adr = 0;
        string[] stringSeparators = new string[] { "\",\"" };
        for (int x = 1; x < str.Length; x++)
        {
            string[] part = str[x].Split("[\""[0]);
            if (part.Length > 1)
            {
                part[1] = part[1].Replace("\"]", "");

                string[] data = part[1].Split(stringSeparators, StringSplitOptions.None);
                //Debug.Log("x=" + x + " L:" + data.Length + " >>" + part[1]);

                for (int y = 0; y < data.Length; y++)
                {
                    Debug.Log("x - 1 = " + (x - 1) + " > y = " + y + "  > xdim: " + OBD_Data.GetLength(0) + " ydim: " + OBD_Data.GetLength(1) + " > data.Lenght: " + data.Length +
                        "  > adr = " + adr);
                    Debug.Log(part[1]);
                    if (y < OBD_Data.GetLength(1))
                        OBD_Data[adr, y] = ParseFloat(data[y]);
                    else
                        Debug.Log("error");
                }
                adr++;
            }
        }

        Camera.main.gameObject.GetComponent<Move_Camera>().Set_Camera_Motion(dataCount);
    }

    public void Save_as_CSV()
    {
        string csvStr = "No,";

        //Header
        for (int x = 0; x < OBD_Header.Length; x++)
        {
            csvStr += OBD_Header[x].ValueName + " " + OBD_Header[x].Units;
            if (x < OBD_Header.Length - 1)
                csvStr += ",";
        }

        //End of HEADER
        csvStr += "\n";

        //Data
        for (int x = 0; x < OBD_Data.GetLength(0); x++)
        {
            string str = x + ",";
            for (int y = 0; y < OBD_Data.GetLength(1); y++)
            {
                str += OBD_Data[x, y].ToString();
                if (y < OBD_Data.GetLength(1) - 1)
                    str += ",";
            }

            csvStr += str;

            if (x < OBD_Data.GetLength(0) - 1)
                csvStr += "\n";
        }

        Debug.Log(csvStr);

        WriteString(csvStr);
    }

    //----------------------------------------------------------------------------------------------------------------------



    #region COMMON ROUTINES
    //COMMON ROUTINES XXXXXXXXXXXXXXXXXXXXXCCXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //----------------------------------------------------------------------------------------------------------------------
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public int ParseInt(string str)
    {
        int val = 0;
        int.TryParse(str, out val);
        return val;
    }

    public float ParseFloat(string str)
    {
        str = str.Replace("<", "");
        str = str.Replace(">", "");
        str = str.Replace(" ", "");
        str = str.Replace("\"", "");
        str = str.Replace("\r", "");

        float val = 0;
        float.TryParse(str, out val);
        return val;
    }

    void Preview_Data()
    {
        for (int x = 0; x < OBD_Data.GetLength(0); x++)
        {
            string str = x + ": ";
            for (int y = 0; y < OBD_Data.GetLength(1); y++)
            {
                //Debug.Log("x - 1 = " + (x - 1) + " > y = " + y + "  > xdim: " + OBD_Data.GetLength(0) + " ydim: " + OBD_Data.GetLength(1) + " > data.Lenght: " + data.Length);
                str += OBD_Data[x, y].ToString() + " ";
            }
            Debug.Log(str);
        }
    }

    public void WriteString(string str)
    {
        string path = "Assets/Resources/" + File_Name + "_out" + ".csv";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(str);
        writer.Close();

        ////Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path);
        //TextAsset asset = Resources.Load("test");

        ////Print the text from the file
        //Debug.Log(asset.text);
    }

    public void Select_Deselect_All_Buttons(bool isSelected)
    {
        for (int x = 0; x < Buttons_List.Count; x++)
            Buttons_List[x].GetComponent<Chose_Value_BTN>().Select_Unselect(isSelected);
    }

    public void Destroy_All_Buttons()
    {
        if (Buttons_List.Count > 0)
        {
            for (int x = 1; x < Buttons_List.Count; x++)
                Destroy(Buttons_List[x]);
        }

        Buttons_List = new List<GameObject>();
    }

    public void Drav_Lines()
    {
        if (Lines_List.Count > 0)
        {
            for (int x = 0; x < Lines_List.Count; x++)
            {
                Destroy(Lines_List[x]);
            }

            Lines_List = new List<GameObject>();
        }

        int lineNo = 0;

        //Count selected buttons
        int count = 0;
        for (int x = 0; x < Buttons_List.Count; x++)
        {
            if (Buttons_List[x].GetComponent<Chose_Value_BTN>().selected)
                count++;
        }

        for (int x = 0; x < Buttons_List.Count; x++)
        {
            if (Buttons_List[x].GetComponent<Chose_Value_BTN>().selected)
            {
                GameObject obj = Instantiate(Line);
                obj.transform.position = Vector3.zero;
                obj.GetComponent<Water>().Drav_Line(x, lineNo, count);
                lineNo++;
            }
        }
    }
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //----------------------------------------------------------------------------------------------------------------------
    //END OF COMMON ROUTINES XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion



    //----------------------------------------------------------------------------------------------------------------------
}
