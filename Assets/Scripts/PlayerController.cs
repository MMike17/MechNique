using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/// <summary>class for player, centralizes call</summary>
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float joystick_deadzone,handle_deadzone,aim_deadzone;
    [SerializeField] float bop_magnitude;
    [SerializeField] float angle_limit_x;
    [SerializeField] float angle_limit_z;
    [SerializeField] Vector2 max_speed;

    [Header("Assing in Inspector")]
    [SerializeField] Weapon[] weapons;
    [SerializeField] Slider[] weapons_overheat;
    [SerializeField] Image hp_interface;
    [SerializeField] Text score;
    
    Robot robot;
    Transform joystick,camera_bop;
    Hand right_hand,left_hand;

    Rigidbody rigid;
    AdjustsColor adjust_color;
    Vector3 pos_offset,robot_input,aim_input;
    float walking_timer;
    Vector2 speed;
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

        Aim();

        Interface();
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

        // left hand
        if(left_hand.grab_joystick)
        {
            if(!CustomHelper.MOL(left_hand.transform.position.z,left_hand.pos_offset.z,handle_deadzone))
                aim_input=left_hand.transform.position-left_hand.pos_offset;
            else
                aim_input=transform.forward;
        }
    }

    void Movement ()
    {
        //clamping the angle values
        robot_input.x=Mathf.Clamp(robot_input.x, -angle_limit_x, angle_limit_x);
        robot_input.z=Mathf.Clamp(robot_input.z, -angle_limit_z, angle_limit_z);

        //setting speed.x at a percentage of wrist angle
        speed.x=(robot_input.x / angle_limit_x * max_speed.x);

        //don't worry about angle_limit_z and robot_input.z, 'ts just a matter of Vectors. And speed.y actually is speed.z
        speed.y=(robot_input.z / angle_limit_z * max_speed.y);

        if (robot_input.x !=0 || robot_input.z !=0) 
        { // when moving
            Vector3 bop_vector=transform.position;
            walking_timer+=Time.deltaTime * speed.magnitude;/* * real speed here <- faster headbop*/;

            bop_vector.y+=Mathf.Sin(walking_timer)*bop_magnitude;
            camera_bop.position=bop_vector;
        }
        else
        { // when not moving
             camera_bop.position=Vector3.MoveTowards(camera_bop.position,transform.position,0.1f);
        }

        rigid.velocity= new Vector3 (speed.x, rigid.velocity.y, speed.y);
    }

    void Aim ()
    {
        foreach(Weapon weapon in weapons)
            weapon.transform.LookAt(weapon.transform.position+aim_input);

        if(left_hand.grab_joystick&&left_hand.fire)
            Fire();
        else
            Refresh();
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

    void Interface ()
    {
        weapons_overheat[0].value=Mathf.MoveTowards(weapons_overheat[0].value,weapons[0].overheat,0.5f);
        weapons_overheat[1].value=Mathf.MoveTowards(weapons_overheat[1].value,weapons[1].overheat,0.5f);
        hp_interface.fillAmount=Mathf.MoveTowards(hp_interface.fillAmount,robot.actual_hp/robot.max_hp,0.1f);
        score.text=GameManager.instance.score.ToString();

        adjust_color.SetColor(robot.actual_hp/robot.max_hp);
    }
}