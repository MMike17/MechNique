using UnityEngine;

/// <summary>class robots that can take damage</summary>
public class Robot : MonoBehaviour
{
    [Header("Settings")]
    public int max_hp;

    [HideInInspector]
    public float actual_hp;

    void Awake ()
    {
        actual_hp=max_hp;
    }

    public void TakeDamage (float damage)
    {
        actual_hp-=damage;
    }
}