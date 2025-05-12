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
            manager.borderCollision(transform.position, true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform.tag == "Component") {
            // Debug.Log("collider out:" + collider.transform.name);
            manager.borderCollision(transform.position, false);
        }        
    }
}
