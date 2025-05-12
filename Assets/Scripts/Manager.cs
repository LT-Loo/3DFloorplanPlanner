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
    private Vector3 prevPos;
    private float moveSpeed = 10.0f;
    private Material objMaterial;
    private bool isCollide = false;
    private bool refusePlacement = false;
    private float moveX, moveZ;
    private Vector3 borderPos;
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
                selected.GetComponent<StoreComponent>().addManager(GetComponent<Manager>());
                newObj = false;
                prevPos = hitFloor.point;
                changeObjectState(placementMode);
            } 

            moveX = prevPos.x - selected.transform.position.x;
            moveZ = prevPos.z - selected.transform.position.z;
            
            if (isCollide) {checkBorderCollision();}

            float posX = selected.transform.position.x + moveX * moveSpeed * Time.deltaTime;
            float posZ = selected.transform.position.z + moveZ * moveSpeed * Time.deltaTime;
            selected.transform.position = new Vector3(posX, selected.transform.position.y, posZ);
            prevPos = hitFloor.point;

            
            if (Input.GetMouseButtonDown(0) && !refusePlacement) {
                disableMode();
            }

            if (Input.GetKeyDown(KeyCode.Delete)) {
                Destroy(selected.gameObject);
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
        refusePlacement = false;
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

    public void borderCollision(Vector3 pos, bool isEnter) {
        if (isEnter) {
            isCollide = true;
            borderPos = pos;
        } else {isCollide = false;}
    }

    void checkBorderCollision() {
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

}
