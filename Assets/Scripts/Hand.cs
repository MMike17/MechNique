using UnityEngine;
using Valve.VR;

/// <summary>enum to describe trigger type (enter, stay, exit)</summary>
public enum TriggerType {enter,stay,exit};

/// <summary>class for both hands controller, used to send signals upwards</summary>
[RequireComponent(typeof(BoxCollider),typeof(Rigidbody))]
public class Hand : MonoBehaviour
{
    [Header("Settings")]
    public SteamVR_Input_Sources source;
    [SerializeField] SteamVR_Action_Boolean grab,fire_trigger;
    [SerializeField] SteamVR_Action_Vector2 joystick;

    [HideInInspector]
    public Vector2 joystick_value;
    [HideInInspector]
    public bool grab_joystick,fire;
    [HideInInspector]
    public Quaternion rot_offset;
    [HideInInspector]
    public Vector3 pos_offset;

    void Awake ()
    {
        GetComponent<BoxCollider>().isTrigger=true;
        GetComponent<Rigidbody>().useGravity=false;

        joystick_value=Vector2.zero;
    }

    void Update ()
    {
        if(grab.GetState(source)==false)
            grab_joystick=false;

        fire=fire_trigger.GetState(source);

        joystick_value=joystick.GetAxis(source);
    }

    void OntriggerStay (Collider col)
    {
        if(col.CompareTag("Joystick")&&grab.GetStateDown(source))
        {
            grab_joystick=true;
            rot_offset=transform.rotation;
            pos_offset=transform.position;
        }
    }
}