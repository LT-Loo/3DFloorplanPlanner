using UnityEngine;

// Detect collision between component and floor space border
// Inform manager when collision is detected or finished
public class BorderCollision : MonoBehaviour
{
    public Manager manager;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Component") {
            manager.borderCollisionTrigger(transform.position, true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag == "Component") {
            manager.borderCollisionTrigger(transform.position, false);
        }        
    }

}
