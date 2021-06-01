using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_Camera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Slider Slider_Cam = null;

    [Header("Variables")]
    Vector3 Cam_Start_Pos;

    // Start is called before the first frame update
    void Start()
    {
        Cam_Start_Pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_Camera_Motion(float range)
    {
        Slider_Cam.minValue = -(range / 2.0f);
        Slider_Cam.maxValue = (range / 2.0f);
        Slider_Cam.value = Slider_Cam.minValue;
        
    }

    public void Slider_Cam_Move()
    {
        transform.position = new Vector3(Slider_Cam.value, Cam_Start_Pos.y, Cam_Start_Pos.z);
    }

    public void Move_Cam(bool left)
    {
    
    }
}
