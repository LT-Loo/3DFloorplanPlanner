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
    public ComponentPanel componentPanel;
    [SerializeField] private GameObject[] prefabs;
    private Dictionary<string, GameObject> prefabList;
    private bool placementMode = false;
    private GameObject selected;
    private string selectedName;
    private bool newObj = false;
    private Vector3 prevPos, currPos;
    private float moveSpeed = 10.0f;
    private Material objMaterial;
    private bool isCollide = false;
    private bool refusePlacement = false;
    private float moveX, moveZ;
    private float offsetX, offsetY;
    private Vector3 rayCurrPos, rayPrevPos, offsetV3;
    private Vector3 borderPos;
    private Vector3 posBeforePanelEdit;
    private Quaternion rotBeforePanelEdit;
    private bool updatedByPanel = false;
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
            // distz;
            if (newObj && selected == null) {
                Transform newObject = prefabList[selectedName].transform;
                selected = Instantiate(prefabList[selectedName], new Vector3(0, newObject.transform.position.y, 0), newObject.rotation);
                selected.GetComponent<StoreComponent>().addManager(GetComponent<Manager>());
                newObj = false;
                prevPos = hitFloor.point;
                // prevPos = selected.transform.position;
                // currPos = hitFloor.point;
                // offsetV3 = selected.transform.position - hitFloor.point;
                // distz = selected.transform.position.z - cam.transform.position.z;
                // prevPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distz));
                // rayCurrPos = rayPrevPos = hitFloor.point; //new Vector3(hitFloor.point.x - selected.transform.position.x, 0f, hitFloor.point.z - selected.transform.position.z);
                changeObjectState(placementMode);
                componentPanel.openPanel(selected);
            } 

            // Ray rayy = Camera.main.ScreenPointToRay(Input.mousePosition + offsetV3); 
            // if (Physics.Raycast(rayy, out hitComponent, Mathf.Infinity, floorLayer)) {
                // selected.transform.position = new Vector3(hitComponent.point.x, selected.transform.position.y, hitComponent.point.z);
            // }
            // currPos = hitFloor.point;
            // float offsetX = hitFloor.point.x - selected.transform.position.x;
            // float offsetZ = hitFloor.point.z - selected.transform.position.z;
            // Debug.Log("offset: " + offsetX + " " + offsetZ);
            // moveX = currPos.x - prevPos.x;
            // moveZ = currPos.z - prevPos.z;
            // Debug.Log("move: " + moveX + " " + moveZ);
            moveX = prevPos.x - selected.transform.position.x;
            moveZ = prevPos.z - selected.transform.position.z;
            // distz = selected.transform.position.z - cam.transform.position.z;
            // Vector3 move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distz)) - prevPos;
            // moveX = hitFloor.point.x - prevPos.x;
            // moveZ = hitFloor.point.z - prevPos.x;
            // moveX = rayCurrPos.x - rayPrevPos.x;
            // moveZ = rayCurrPos.z - rayPrevPos.z;
            // Vector3 move = hitFloor.point - prevPos;
            
            if (isCollide) {stopMovement();}

            // selected.transform.position = new Vector3(hitFloor.point.x - offSetX, selected.transform.position.y, hitFloor.point.z - offsetZ);
            // Vector3 target = hitFloor.point + offsetV3; 
            // selected.transform.position = new Vector3(target.x, selected.transform.position.y, target.z);

            float posX = selected.transform.position.x + moveX * moveSpeed * Time.deltaTime; // - offsetX; // * Time.deltaTime;
            float posZ = selected.transform.position.z + moveZ * moveSpeed * Time.deltaTime; // - offsetZ; //* Time.deltaTime;
            selected.transform.position = new Vector3(posX, selected.transform.position.y, posZ);
            // selected.transform.position += move * moveSpeed * Time.deltaTime;
            // prevPos = hitFloor.point - (selected.transform.position - hitFloor.point);
            // distz = selected.transform.position.z - cam.transform.position.z;
            prevPos = hitFloor.point;
            
            // prevPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distz));
            // rayPrevPos = rayCurrPos;
            // rayCurrPos = hitFloor.point;
            // prevPos = selected.transform.position;

            componentPanel.receiveObjectData(selected);

            
            if (Input.GetMouseButtonDown(0)) {
                disableMode();
                Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, (hitFloor.point.z - cam.transform.position.z))));
            }

            if (Input.GetKeyDown(KeyCode.Delete)) {
                deleteComponent();
            }

        } else if (!placementMode && Physics.Raycast(componentRay, out hitComponent, Mathf.Infinity, componentLayer)) {
            if (Input.GetMouseButtonDown(0) && hitComponent.transform.tag == "Component") {
                if (Physics.Raycast(floorRay, out hitFloor, Mathf.Infinity, floorLayer)) {
                    // prevPos = selected.transform.position;
                    prevPos = hitFloor.point;
                    // offsetV3 = selected.transform.position - hitFloor.point;
                    // Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    // rayPrevPos = hitFloor.point;
                    // prevPos = new Vector3(hitFloor.point.x + selected.transform.position.x, 0f, hitFloor.point.z + selected.transform.position.z);
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
            componentPanel.openPanel(selected);
        }
    }
    public bool disableMode() {
        if (!refusePlacement) {
            placementMode = false;
            // moving = false;
            componentMenu.deactivateAllToggles();
            cam.GetComponent<CameraSystem>().enabled = true;
            newObj = false;
            refusePlacement = false;
            if (selected != null) {
                changeObjectState(placementMode);
            }
        } else {return false;}

        return true;
    }

    void changeObjectState(bool moving) {
        Renderer renderer = selected.GetComponent<Renderer>();

        if (moving) {
            objMaterial = renderer.material;
            renderer.material = ghostMaterial;
        } else {renderer.material = objMaterial;}

    }

    public void borderCollisionTrigger(Vector3 pos, bool isEnter) {
        if (isEnter) {
            isCollide = true;
            borderPos = pos;
        } else {isCollide = false;}
    }

    void stopMovement() {
        if (borderPos.x < 0f && moveX < 0f) {moveX = 0f;} 
        else if (borderPos.x > 0f && moveX > 0f) {moveX = 0f;}

        if (borderPos.z < 0f && moveZ < 0f) {moveZ = 0f;} 
        else if (borderPos.z > 0f && moveZ > 0f) {moveZ = 0f;}
    }

    public void componentCollision(bool isCollide) {
        refusePlacement = isCollide;
        if (selected != null) {
            Renderer renderer = selected.GetComponent<Renderer>();

            if (refusePlacement) {
                renderer.material.color = new Color(1f, 0f, 0f);
            } else {renderer.material = ghostMaterial;}
        }
    }

    public void deleteComponent() {
        componentPanel.deleteComponent();
        Destroy(selected.gameObject);
        disableMode();
    }

    public void updateDataFromPanel(string name, string input) {
        Vector3 pos = selected.transform.position;
        Quaternion rot = selected.transform.rotation;
        posBeforePanelEdit = pos;
        rotBeforePanelEdit = rot;
        if (selected != null) {
            if (name == "name") {
                selected.name = input;
            } else if (name == "posX") {
                selected.transform.position = new Vector3(float.Parse(input), pos.y, pos.z);
            } else if (name == "posZ") {
                selected.transform.position = new Vector3(pos.x, pos.y, float.Parse(input));
            } else if (name == "rot") {
                selected.transform.rotation = Quaternion.Euler(rot.x, float.Parse(input), rot.z);
            }
            updatedByPanel = true;
            // Physics.SyncTransforms();
        }
    }

    public void returnInitPos(GameObject selected) {
        if (updatedByPanel) {
            selected.transform.position = posBeforePanelEdit;
            selected.transform.rotation= rotBeforePanelEdit;
            componentPanel.exceedLimit(true);
            updatedByPanel = false;
        }
    }

}
