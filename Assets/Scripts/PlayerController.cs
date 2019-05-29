using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/// <summary>class for player, centralizes call</summary>
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float joystick_deadzone;

    [Header("Assing in Inspector")]
    [SerializeField] Weapon[] weapons;
    [SerializeField] Slider[] weapons_overheat;
    [SerializeField] Image hp_interface;
    
    Robot robot;
    Transform left_hand,right_hand,joystick;
    Quaternion rot_offset;
    Vector3 pos_offset;
    Vector2 robot_input;

    bool first_grab,grab_joystick;

    void Awake ()
    {
        left_hand=transform.GetChild(0);
        right_hand=transform.GetChild(1);
        robot=GetComponent<Robot>();

        weapons_overheat[0].maxValue=weapons[0].overheat_threshold;
        weapons_overheat[1].maxValue=weapons[1].overheat_threshold;

        joystick=null;
    }

    void GrabRight (Hand hand)
    {
        if(hand.grab.GetStateDown(hand.source))
            rot_offset=hand.transform.rotation;

        if(grab_joystick)
        {
            if(!CustomHelper.MOL(hand.transform.rotation.eulerAngles.x,rot_offset.eulerAngles.x,joystick_deadzone))
                robot_input.x=hand.transform.rotation.eulerAngles.x-rot_offset.eulerAngles.x;

            if(!CustomHelper.MOL(hand.transform.rotation.eulerAngles.z,rot_offset.eulerAngles.z,joystick_deadzone))
                robot_input.y=hand.transform.rotation.eulerAngles.z-rot_offset.eulerAngles.z;
        }
    }

    void NotGrabRight ()
    {
        grab_joystick=false;
    }

    void Fire ()
    {
        if(grab_joystick)
        {
            foreach(Weapon weapon in weapons)
                weapon.Fire();
        }
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

    void OnHandTrigger (Trigger trigger)
    {
        Debug.Log("Trigger "+trigger.type+" called on "+trigger.hand.source+" hand");

        if(trigger.collider.CompareTag("Joystick")&&trigger.hand.source==SteamVR_Input_Sources.RightHand&&trigger.type==TriggerType.stay&&trigger.hand.grab.GetState(trigger.hand.source))
            grab_joystick=true;
    }
}