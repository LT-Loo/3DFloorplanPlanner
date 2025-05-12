using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Camera cam;
    public LayerMask floorLayer;
    [SerializeField] private GameObject[] prefabs;
    private Dictionary<string, GameObject> prefabList;
    private bool placementMode = false;
    private GameObject selected;
    private string selectedName;
    private bool objExist = false;
    void Start()
    {
        prefabList = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in prefabs) {
            prefabList.Add(prefab.name, prefab);
        }      
    }

    void Update()
    {
        if (placementMode && !objExist && Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, floorLayer)) {
                Transform newObject = prefabList[selectedName].transform;
                selected = Instantiate(prefabList[selectedName], new Vector3(hitInfo.point.x,newObject.transform.position.y, hitInfo.point.z), newObject.rotation);
            }
        }
    }

    public bool getMode() {return placementMode;}
    public void activateMode(bool exist, string name = null, GameObject component = null) {
        placementMode = true;
        if (!exist) {
            objExist = exist;
            selectedName = name;
        }
    }
    public void disableMode() {placementMode = false;}

}
