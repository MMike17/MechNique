using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter (Collision col)
    {
        if(col.transform.CompareTag("Robot"))
        {
            Robot robot=col.transform.GetComponent<Robot>();
            
            if(col.transform.GetComponent<Robot>()!=null)
                robot.TakeDamage(damage);
        }
    }
}