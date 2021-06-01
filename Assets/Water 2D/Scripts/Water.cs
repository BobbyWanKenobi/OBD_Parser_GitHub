using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    //Our renderer that'll make the top of the water visible
    LineRenderer Body;

    //Our physics arrays
    float[] xpositions;
    float[] ypositions;
    float[] velocities;
    float[] accelerations;

    //Our meshes and colliders
    GameObject[] meshobjects;
    GameObject[] colliders;
    Mesh[] meshes;

    //Our particle system
    public GameObject splash;

    //The material we're using for the top of the water
    public Material mat;

    //The GameObject we're using for a mesh
    public GameObject watermesh;

    //All our constants
    const float z = 0f;

    [Header("REFERENCES")]
    Parser parser = null;

    [Header("WATER DIMENSIONS & POS")]
    [SerializeField] float Left_param = -10;
    [SerializeField] float Width_param = 20;
    [SerializeField] float Top_param = 0;
    [SerializeField] float Bottom_param = -3;
    [SerializeField] Vector3 startRot = new Vector3(-90, 0, 0);

    [Header("Variables")]
    float minVal;
    float maxVal;
    Color lineColor;

    void Start()
    {
        
    }

    //Called regularly by Unity
    void FixedUpdate()
    {
        ////Here we use the Euler method to handle all the physics of our springs:
        //for (int i = 0; i < xpositions.Length ; i++)
        //{
        //    //MyAdd
        //    ypositions[i] = i;
        //    Body.SetPosition(i, new Vector3(xpositions[i], ypositions[i], z));
        //}
    }

    public void Drav_Line(int id, int lineNO, int totalNoLines)
    {
        parser = Parser.parseInstr;

        //Spawning our Line
        Spawn_Line(Left_param, Width_param, Top_param, Bottom_param, parser.OBD_Data.GetLength(0));
        Set_Line_Color(lineNO);
        parser.Lines_List.Add(this.gameObject);

        Get_Ranges(id);

        Set_Text(id);

        //Here we ASIGN VALUES
        float val;
        float range = maxVal - minVal;
        float camSize = Camera.main.orthographicSize;
        float rangeFix = (camSize * 0.9f) / range;
        float yShift = ((minVal + maxVal) / 2.0f) * rangeFix;
        
        //Shift different lines to avoid overlap
        int divider = 10;
        if (totalNoLines < divider)
            divider = totalNoLines;
        float thisLineShift = -(camSize / 100f) * (divider / 2) + (camSize / 100f) * (lineNO % divider);

        for (int x = 0; x < xpositions.Length; x++)
        {
            //MyAdd
            val = parser.OBD_Data[x, id] * rangeFix - yShift + thisLineShift;

            ypositions[x] = x;
            Body.SetPosition(x, new Vector3(xpositions[x], val, z));
        }

        //Set Start 
        transform.eulerAngles = startRot;

        Debug.Log("id = " + id + "  > parser.OBD_Data.GetLength(0) = " + parser.OBD_Data.GetLength(0) + "  > parser.OBD_Data.GetLength(1) = " + parser.OBD_Data.GetLength(1) +
            "  > minVal = " + minVal + "  > maxVal = " + maxVal + "  > camSize = " + Camera.main.orthographicSize);
    }

    void Spawn_Line(float Left, float Width, float Top, float Bottom, int pointsNo)
    {
        //Calculating the number of edges and nodes we have
        int edgecount = pointsNo - 1;    //Mathf.RoundToInt(Width) /** 5*/;
        int nodecount = edgecount;

        //Add our line renderer and set it up:
        Body = gameObject.AddComponent<LineRenderer>();
        Body.material = mat;
        Body.material.renderQueue = 1000;
        Body.positionCount = nodecount;
        //Body.SetWidth(0.1f, 0.1f);
        Body.startWidth = 0.3f;
        Body.endWidth = 0.3f;

        //Declare our physics arrays
        xpositions = new float[nodecount];
        ypositions = new float[nodecount];

        Width = pointsNo;
        Left = Width / 2 * -1;

        //For each node, set the line renderer and our physics arrays
        for (int i = 0; i < nodecount; i++)
        {
            ypositions[i] = Top;
            xpositions[i] = Left + Width * i / edgecount;
            Body.SetPosition(i, new Vector3(xpositions[i], Top, z));
        }
    }

    void Get_Ranges(int id)
    {
        //find min and max value
        minVal = parser.OBD_Data[0, id];
        maxVal = minVal;

        for (int x = 0; x < xpositions.Length; x++)
        {
            if (parser.OBD_Data[x, id] > maxVal)
                maxVal = parser.OBD_Data[x, id];

            if (parser.OBD_Data[x, id] < minVal)
                minVal = parser.OBD_Data[x, id];
        }
    }

    void Set_Line_Color(int id)
    {
        if (id > 11)
            id = id - 11;

        switch (id)
        {
            case 0:
                lineColor = new Color(1.0f, 1.0f, 1.0f);
                break;

            case 1:
                lineColor = new Color(1.0f, 1.0f, 0.0f);
                break;

            case 2:
                lineColor = new Color(1.0f, 0.0f, 0.0f);
                break;

            case 3:
                lineColor = new Color(0.0f, 1.0f, 1.0f);
                break;

            case 4:
                lineColor = new Color(0.3f, 0.6f, 1.0f);
                break;

            case 5:
                lineColor = new Color(0.0f, 1.0f, 0.0f);
                break;


            case 6:
                lineColor = new Color(0.5f, 0.5f, 0.5f);
                break;

            case 7:
                lineColor = new Color(1.0f, 1.0f, 0.5f);
                break;

            case 8:
                lineColor = new Color(1.0f, 0.5f, 0.5f);
                break;

            case 9:
                lineColor = new Color(0.5f, 1.0f, 1.0f);
                break;

            case 10:
                lineColor = new Color(0.6f, 0.6f, 1.0f);
                break;

            case 11:
                lineColor = new Color(0.5f, 1.0f, 0.5f);
                break;
        }

        //lineColor = new Color( Random.Range(0.6f, 1f), Random.Range(0.6f, 1f), Random.Range(0.6f, 1f));

        Body.GetComponent<Renderer>().material.SetColor("_Color", lineColor);
    }

    void Set_Text(int id)
    {
        string str = id + ". " + parser.OBD_Header[id].ValueName + " (" + parser.OBD_Header[id].Units + ")";
        str = ColorString(str, lineColor);

        UImanager.UIinst.Text_Graph.text += str + "\n";

        str = id + ". MIN: " + minVal + " / MAX: " + maxVal;
        str = ColorString(str, lineColor);

        UImanager.UIinst.Text_Range.text += str + "\n";

    }

    string ColorString(string text, Color color)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
    }


}
