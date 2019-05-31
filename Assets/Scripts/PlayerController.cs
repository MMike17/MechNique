using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/// <summary>class for player, centralizes call</summary>
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float joystick_deadzone;
    [SerializeField] float bop_magnitude;

    [Header("Assing in Inspector")]
    [SerializeField] Weapon[] weapons;
    [SerializeField] Slider[] weapons_overheat;
    [SerializeField] Image hp_interface;
    
    Robot robot;
    Transform joystick,camera_bop;
    Hand right_hand,left_hand;
    Vector3 pos_offset,robot_input;
    float walking_timer;
    bool first_grab;

    void Awake ()
    {
        robot=GetComponent<Robot>();

        weapons_overheat[0].maxValue=weapons[0].overheat_threshold;
        weapons_overheat[1].maxValue=weapons[1].overheat_threshold;

        joystick=null;
        walking_timer=0;
        first_grab=false;
    }

    void AwakeHand (Hand hand,string side)
    {
        if(side=="left")
            left_hand=hand;

        if(side=="right")
            right_hand=hand;
    }

    void Update ()
    {
        DetectController();

        Movement();

        if(right_hand.grab_joystick&&right_hand.fire)
            Fire();
        else
            Refresh();
    }

    void DetectController ()
    {
        // right hand
        if(right_hand.grab_joystick)
        {
            if(!CustomHelper.MOL(right_hand.transform.rotation.eulerAngles.x,right_hand.rot_offset.eulerAngles.x,joystick_deadzone))
                robot_input.x=right_hand.transform.rotation.eulerAngles.x-right_hand.rot_offset.eulerAngles.x;

            if(!CustomHelper.MOL(right_hand.transform.rotation.eulerAngles.z,right_hand.rot_offset.eulerAngles.z,joystick_deadzone))
                robot_input.z=right_hand.transform.rotation.eulerAngles.z-right_hand.rot_offset.eulerAngles.z;

            if(!CustomHelper.MOL(right_hand.joystick_value.x,0,joystick_deadzone))
                robot_input.y=right_hand.joystick_value.x;
        }
        else
            robot_input=Vector3.zero;
    }

    void Movement ()
    {
        // when moving
        Vector3 bop_vector=transform.position;
        walking_timer+=Time.deltaTime/* * real speed here <- faster headbop*/;

        bop_vector.y+=Mathf.Sin(walking_timer)*bop_magnitude;
        camera_bop.position=bop_vector;

        // when not moving
        camera_bop.position=Vector3.MoveTowards(camera_bop.position,transform.position,0.1f);
    }

    void Fire ()
    {
        foreach(Weapon weapon in weapons)
            weapon.Fire();
    }

    void Refresh ()
    {
        foreach(Weapon weapon in weapons)
            weapon.Refresh();
    }

    void Iterface ()
    {
        weapons_overheat[0].value=Mathf.MoveTowards(weapons_overheat[0].value,weapons[0].overheat,0.5f);
        weapons_overheat[1].value=Mathf.MoveTowards(weapons_overheat[1].value,weapons[1].overheat,0.5f);
        hp_interface.fillAmount=Mathf.MoveTowards(hp_interface.fillAmount,robot.actual_hp/robot.max_hp,0.1f);
    }
}