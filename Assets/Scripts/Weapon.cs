using UnityEngine;

/// <summary>class for weapons, manages everything from effects to bullets and fire</summary>
public class Weapon : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Bullets per minute")]
    [SerializeField] int rate_of_fire;
    [Tooltip("overheat per shot")]
    [SerializeField] float overheat_rate;
    [Tooltip("refresh per second")]
    [SerializeField] float refresh_rate;
    public float overheat_threshold;
    [SerializeField] float bullet_speed;
    [Tooltip("in seconds")]
    [SerializeField] float refresh_delay;

    [Header("Assign in Inspector")]
    [SerializeField] GameObject bullet_model;
    [SerializeField] Transform muzzle;

    [HideInInspector]
    public float overheat;

    ParticleSystem fire_effect;
    float fire_timer,refresh_timer;

    public void Fire ()
    {
        if(overheat>=overheat_threshold)
            return;

        fire_timer+=Time.deltaTime;

        if(fire_timer>=Time.deltaTime/rate_of_fire)
        {
            fire_timer=0;
            refresh_timer=0;
            overheat+=overheat_threshold;
            fire_effect.Play();
            GameObject bullet=Instantiate(bullet_model,muzzle.position,muzzle.rotation);
            bullet.GetComponent<Rigidbody>().velocity=muzzle.forward*bullet_speed;
        }
    }

    public void Refresh ()
    {
        if(refresh_timer>=refresh_delay)
            overheat-=refresh_rate;
        else
            refresh_timer+=Time.deltaTime;
    }
}