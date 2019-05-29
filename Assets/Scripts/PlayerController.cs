using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] SteamVR_Action_Boolean grab_right;
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

    void Awake ()
    {
        left_hand=transform.GetChild(0);
        right_hand=transform.GetChild(1);
        robot=GetComponent<Robot>();

        weapons_overheat[0].maxValue=weapons[0].overheat_threshold;
        weapons_overheat[1].maxValue=weapons[1].overheat_threshold;

        joystick=null;
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

    void OnHandTrigger (Trigger trigger)
    {
        Debug.Log("Trigger "+trigger.type+" called on "+trigger.hand.source+" hand");

        bool grab_joystick=trigger.collider.CompareTag("Joystick")&&trigger.hand.source==SteamVR_Input_Sources.RightHand&&trigger.type==TriggerType.stay&&grab_right.GetState(trigger.hand.source);

        if(grab_joystick)
        {
            // if not grabbed yet
            rot_offset=trigger.hand.transform.rotation;

            // if already grabbed
            if(!CustomHelper.MOL(trigger.hand.transform.rotation.eulerAngles.x,rot_offset.eulerAngles.x,joystick_deadzone))
                robot_input.x=trigger.hand.transform.rotation.eulerAngles.x-rot_offset.eulerAngles.x;

            if(!CustomHelper.MOL(trigger.hand.transform.rotation.eulerAngles.z,rot_offset.eulerAngles.z,joystick_deadzone))
                robot_input.y=trigger.hand.transform.rotation.eulerAngles.z-rot_offset.eulerAngles.z;
        }
    }
}