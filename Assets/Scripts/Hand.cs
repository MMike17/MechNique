using UnityEngine;

public enum TriggerType {enter,stay,exit};

[RequireComponent(typeof(BoxCollider),typeof(Rigidbody))]
public class Hand : MonoBehaviour
{
    //public SteamVR_Input_Sources source;
    //[SerializeField] SteamVR_Action_Boolean fire;

    void Awake ()
    {
        GetComponent<BoxCollider>().isTrigger=true;
        GetComponent<Rigidbody>().useGravity=false;
    }

    void Update ()
    {
        /*if(fire.GetState(source))
            SendMessageUpwards("Fire",SendMessageOptions.DontRequireReceiver);
        else
            SendMessageUpwards("Refresh",SendMessageOptions.DontRequireReceiver);*/
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