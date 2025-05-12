using UnityEngine;

public class StoreComponent : MonoBehaviour
{
    private Manager manager;

    public void addManager(Manager mng) {
        manager = mng;
    }

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
