using UnityEngine;

public class BorderCollision : MonoBehaviour
{
    public Manager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Component") {
            // Debug.Log("collider in:" + collider.transform.name);
            manager.borderCollisionTrigger(transform.position, true);
        }
    }

    void OnTriggerStay(Collider collider) {
        if (collider.transform.tag == "Component") {
            manager.returnInitPos(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag == "Component") {
            manager.borderCollisionTrigger(transform.position, false);
        }        
    }
}
