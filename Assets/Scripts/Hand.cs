using UnityEngine;
using Valve.VR;

/// <summary>enum to describe trigger type (enter, stay, exit)</summary>
public enum TriggerType {enter,stay,exit};

/// <summary>class for both hands controller, used to send signals upwards</summary>
[RequireComponent(typeof(BoxCollider),typeof(Rigidbody))]
public class Hand : MonoBehaviour
{
    public SteamVR_Input_Sources source;
    public SteamVR_Action_Boolean grab;
    [SerializeField] SteamVR_Action_Boolean fire;

    void Awake ()
    {
        GetComponent<BoxCollider>().isTrigger=true;
        GetComponent<Rigidbody>().useGravity=false;
    }

    void Update ()
    {
        if(fire.GetState(source))
            SendMessageUpwards("Fire",SendMessageOptions.DontRequireReceiver);
        else
            SendMessageUpwards("Refresh",SendMessageOptions.DontRequireReceiver);

        if(grab.GetState(source))
            SendMessageUpwards("GrabRight",this,SendMessageOptions.DontRequireReceiver);
        else
            SendMessageUpwards("NotGrabRight",this,SendMessageOptions.DontRequireReceiver);
    }

    void OntriggerEnter (Collider col)
    {
        SendMessageUpwards("OnHandTrigger",new Trigger(TriggerType.enter,col,this),SendMessageOptions.DontRequireReceiver);
    }

    void OntriggerStay (Collider col)
    {
        SendMessageUpwards("OnHandTrigger",new Trigger(TriggerType.stay,col,this),SendMessageOptions.DontRequireReceiver);
    }

    void OntriggerExit (Collider col)
    {
        SendMessageUpwards("OnHandTrigger",new Trigger(TriggerType.exit,col,this),SendMessageOptions.DontRequireReceiver);
    }
}

/// <summary>class to describe trigger events</summary>
public class Trigger
{
    public Hand hand;
    public TriggerType type;
    public Collider collider;

    public Trigger (TriggerType type,Collider collider,Hand hand)
    {
        this.type=type;
        this.collider=collider;
        this.hand=hand;
    }
}