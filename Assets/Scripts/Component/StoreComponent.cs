using UnityEngine;

// Manage collision between store components
public class StoreComponent : MonoBehaviour
{
    private Manager manager;

    // Add Manager component when component first initialised
    public void addManager(Manager mng) {
        manager = mng;
    }

    // Inform manager when collision happens
    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Component") {
            manager.componentCollision(true);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.transform.tag == "Component") {
            manager.componentCollision(false);
        }
    }
}
