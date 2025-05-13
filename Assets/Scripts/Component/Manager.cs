using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

// Main script controlling store movement position, rotation,
// functions etc.
public class Manager : MonoBehaviour
{
    public Camera cam;
    public LayerMask floorLayer;
    public LayerMask componentLayer;
    private bool placementMode = false;
    private bool refusePlacement = false;

    // Assets and materials
    [SerializeField] private GameObject[] prefabs;
    private Dictionary<string, GameObject> prefabList;
    public Material ghostMaterial;

    // Components
    public ComponentMenu componentMenu;
    public ComponentPanel componentPanel;
    
    // Selected component
    private GameObject selected;
    private string selectedName;
    private Material objMaterial;
    private bool newObj = false;

    // Position
    private Vector3 prevPos;
    private float moveSpeed = 10.0f;
    private float moveX, moveZ;

    // Collision
    private bool isCollide = false;
    private Vector3 borderPos;

    // Load component prefabs
    void Start()
    {
        prefabList = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in prefabs) {
            prefabList.Add(prefab.name, prefab);
        }      
    }

    // Update position and rotation of selected component
    void Update()
    {
        // Use raycast to determine mouse click position
        Ray floorRay = cam.ScreenPointToRay(Input.mousePosition);
        Ray componentRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitFloor, hitComponent;

        // If placement mode is activated and raycast collides with floor
        if (placementMode && Physics.Raycast(floorRay, out hitFloor, Mathf.Infinity, floorLayer)) {

            // Create new component if selected from menu
            if (newObj && selected == null) {
                Transform newObject = prefabList[selectedName].transform;
                selected = Instantiate(prefabList[selectedName], new Vector3(0, newObject.transform.position.y, 0), newObject.rotation);
                selected.GetComponent<StoreComponent>().addManager(GetComponent<Manager>());

                changeObjectState(placementMode); // Change state

                componentPanel.openPanel(selected); // Open component panel

                prevPos = hitFloor.point; // Get raycast hit point
            } 

            // Calculate moving distance
            moveX = prevPos.x - selected.transform.position.x;
            moveZ = prevPos.z - selected.transform.position.z;
            
            // If component collides with border, restrict moving direction
            if (isCollide) {stopMovement();}

            // Assigne new position to component
            float posX = selected.transform.position.x + moveX * moveSpeed * Time.deltaTime;
            float posZ = selected.transform.position.z + moveZ * moveSpeed * Time.deltaTime; 
            selected.transform.position = new Vector3(posX, selected.transform.position.y, posZ);

            prevPos = hitFloor.point; // Update previous position

            // Click to place component, then deactivate placement mode
            if (Input.GetMouseButtonDown(0)) {
                disableMode();
            }

            // Delete component if Delete key is pressed
            if (Input.GetKeyDown(KeyCode.Delete)) {
                deleteComponent();
            }

            if (selected != null) {
                componentPanel.receiveObjectData(selected); // Update component panel
            }

        // When an existing component is selected in non-placement mode
        } else if (!placementMode && Physics.Raycast(componentRay, out hitComponent, Mathf.Infinity, componentLayer)) {
            // Use raycast to get initial moving position
            if (Input.GetMouseButtonDown(0) && hitComponent.transform.tag == "Component") {
                if (Physics.Raycast(floorRay, out hitFloor, Mathf.Infinity, floorLayer)) {
                    prevPos = hitFloor.point;
                }

                // Activate placement mode
                activateMode(false, null, hitComponent.transform.gameObject);
            }
        }
    }

    public bool isPlacementMode() {return placementMode;}

    // Activate placement mode
    public void activateMode(bool newOb, string name = null, GameObject component = null) {
        if (!placementMode) {
            placementMode = true;
            cam.GetComponent<CameraSystem>().enabled = false; // Disable camera tools

            newObj = newOb;
            selected = component; // Assigned selected component

            if (newOb) { // If new component, get component name
                selectedName = name;
            } else { // If existing component, change to placement state
                changeObjectState(placementMode);
                componentPanel.openPanel(selected); // Show component panel
            }
        }
    }

    // Disable placement mode
    public bool disableMode() {
        if (!refusePlacement) {
            placementMode = false;
            componentMenu.deactivateAllToggles();
            cam.GetComponent<CameraSystem>().enabled = true; // Enable camera tools and functions
            newObj = false;
            refusePlacement = false;
            if (selected != null) {
                changeObjectState(placementMode); // Selected component resumes to normal state
            }
        } else {return false;}

        return true;
    }

    // Selected component turns green in placement mode
    void changeObjectState(bool moving) {
        Renderer renderer = selected.GetComponent<Renderer>();

        if (moving) {
            objMaterial = renderer.material;
            renderer.material = ghostMaterial;
        } else {renderer.material = objMaterial;}

    }

    // Store border information when collision occurs
    public void borderCollisionTrigger(Vector3 pos, bool isEnter) {
        if (isEnter) {
            isCollide = true;
            borderPos = pos;
        } else {isCollide = false;}
    }

    // Prevent component from moving outside the floorplan
    // Check which border is collided and restrict movement
    // towards the direction
    void stopMovement() {
        if (borderPos.x < 0f && moveX < 0f) {moveX = 0f;} 
        else if (borderPos.x > 0f && moveX > 0f) {moveX = 0f;}

        if (borderPos.z < 0f && moveZ < 0f) {moveZ = 0f;} 
        else if (borderPos.z > 0f && moveZ > 0f) {moveZ = 0f;}
    }

    // Selected component turns red when collide with other components
    public void componentCollision(bool isCollide) {
        refusePlacement = isCollide;
        if (selected != null) {
            Renderer renderer = selected.GetComponent<Renderer>();

            if (refusePlacement) {
                renderer.material.color = new Color(1f, 0f, 0f);
            } else {renderer.material = ghostMaterial;}
        }
    }

    // Delete component and disable placement mode
    public void deleteComponent() {
        Destroy(selected.gameObject);
        disableMode();
    }

    // Receive data change from component panel and update properties
    public void updateDataFromPanel(string name, string input) {

        // Data stored accordingly
        if (selected != null) {

            Vector3 pos = selected.transform.position;
            Quaternion rot = selected.transform.rotation;

            if (name == "name") {
                selected.name = input;
            } else if (name == "posX") {
                selected.transform.position = new Vector3(float.Parse(input), pos.y, pos.z);
            } else if (name == "posZ") {
                selected.transform.position = new Vector3(pos.x, pos.y, float.Parse(input));
            } else if (name == "rot") {
                selected.transform.rotation = Quaternion.Euler(rot.x, float.Parse(input), rot.z);
            } 
        }
    }


}
