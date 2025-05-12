using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Camera cam;
    public LayerMask floorLayer;
    public LayerMask componentLayer;
    public Material ghostMaterial;
    public ComponentMenu componentMenu;
    [SerializeField] private GameObject[] prefabs;
    private Dictionary<string, GameObject> prefabList;
    private bool placementMode = false;
    private GameObject selected;
    private string selectedName;
    private bool newObj = false;
    private float offsetX, offsetZ;
    private Vector3 prevPos;
    // private bool moving = false;
    private Material objMaterial;
    void Start()
    {
        prefabList = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in prefabs) {
            prefabList.Add(prefab.name, prefab);
        }      
    }

    void Update()
    {
        Ray floorRay = cam.ScreenPointToRay(Input.mousePosition);
        Ray componentRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitFloor, hitComponent;

        if (placementMode && Physics.Raycast(floorRay, out hitFloor, Mathf.Infinity, floorLayer)) {
            if (newObj && selected == null) {
                Transform newObject = prefabList[selectedName].transform;
                selected = Instantiate(prefabList[selectedName], new Vector3(0, newObject.transform.position.y, 0), newObject.rotation);
                newObj = false;
                prevPos = hitFloor.point;
                // offset = hitFloor.point;
                changeObjectState(placementMode);
            } 

            // float dist = selected.transform.position.z - cam.transform.position.z;
            // Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            // Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            // selected.transform.position = new Vector3(worldPos.x, selected.transform.position.y, worldPos.z);
            // selected.transform.position = ne;
            float moveX = prevPos.x - selected.transform.position.x;
            float moveZ = prevPos.z - selected.transform.position.z;
            float posX = selected.transform.position.x + moveX * 10.0f * Time.deltaTime;
            float posZ = selected.transform.position.z + moveZ * 10.0f * Time.deltaTime;
            selected.transform.position = new Vector3(posX, selected.transform.position.y, posZ);
            prevPos = hitFloor.point;
            // offsetX = hitFloor.point.x - selected.transform.position.x;
            // offsetZ = hitFloor.point.z - selected.transform.position.z;
            // selected.transform.position = new Vector3(hitFloor.point.x - offsetX, selected.transform.position.y, hitFloor.point.z);

            
            if (Input.GetMouseButtonDown(0)) {
                disableMode();
            }

        } else if (!placementMode && Physics.Raycast(componentRay, out hitComponent, Mathf.Infinity, componentLayer)) {
            if (Input.GetMouseButtonDown(0) && hitComponent.transform.tag == "Component") {
                if (Physics.Raycast(floorRay, out hitFloor, Mathf.Infinity, floorLayer)) {
                    prevPos = hitFloor.point;
                }
                // offsetZ = hitComponent.transform.position.z - Camera.main.ScreenToWorldPoint(Input.mousePosition).z;
                activateMode(false, null, hitComponent.transform.gameObject);
            }
        }
    }

    public bool getMode() {return placementMode;}
    public void activateMode(bool newOb, string name = null, GameObject component = null) {
        placementMode = true;
        cam.GetComponent<CameraSystem>().enabled = false;
        // moving = true;
        selected = component;
        if (newOb) {
            newObj = newOb;
            selectedName = name;
        } else {
            // prevPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.Log(prevPos);
            changeObjectState(placementMode);
        }
    }
    public void disableMode() {
        placementMode = false;
        // moving = false;
        componentMenu.deactivateAllToggles();
        cam.GetComponent<CameraSystem>().enabled = true;
        newObj = false;
        if (selected != null) {
            changeObjectState(placementMode);
        }
    }

    void changeObjectState(bool moving) {
        Renderer renderer = selected.GetComponent<Renderer>();

        if (moving) {
            objMaterial = renderer.material;
            renderer.material = ghostMaterial;
        } else {renderer.material = objMaterial;}

    }

}
