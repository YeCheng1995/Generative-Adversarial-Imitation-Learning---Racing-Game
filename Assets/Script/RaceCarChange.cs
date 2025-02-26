using UnityEngine;

public class RaceCarChange : MonoBehaviour
{
    private GameObject racecarobject;
    private Transform mainCamera;
    private GameObject carparent;
    private GameObject[] aicarparent;
    private GameObject selectcar;
    private GameObject[] AICar;

    public Vector3 cameraOffset = new Vector3(30f, 180f, 0f);


    void Awake()
    {
        carparent = GameObject.FindGameObjectWithTag("CarParent");
      //  aicarparent = GameObject.FindGameObjectsWithTag("AICarParent");

        selectcar = (GameObject)Resources.Load("Car Prefabs/car_13");
       // GameObject aicar0 = (GameObject)Resources.Load("AI Car Prefabs/car_7");
        // GameObject aicar1 = (GameObject)Resources.Load("AI Car Prefabs/car_9");


        GameObject Car = (GameObject)Instantiate(selectcar, new Vector3(0, 0, 0), Quaternion.identity);
        // GameObject AICar0 = (GameObject)Instantiate(aicar0, new Vector3(0, 0, 0), Quaternion.identity);
        //  GameObject AICar1 = (GameObject)Instantiate(aicar1, new Vector3(0, 0, 0), Quaternion.identity);
        Car.transform.SetParent(carparent.transform, false);
        //AICar0.transform.SetParent(aicarparent[0].transform, false);
        // AICar1.transform.SetParent(aicarparent[1].transform, false);

        GameObject[] CarTag = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject cartag in CarTag)
        {
            cartag.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        ////GameObject[] AICarTag = GameObject.FindGameObjectsWithTag("AICar");
        //foreach (GameObject aicartag in AICarTag)
        //{
        //    aicartag.GetComponent<RCCAICarController>().enabled = false;
        //}
        mainCamera = this.gameObject.transform;
        RCCCarControllerV2 racecarcontroller = carparent.transform.GetChild(0).GetComponent<RCCCarControllerV2>();
        racecarobject = new GameObject();
        racecarobject = racecarcontroller.gameObject;
        racecarobject.GetComponent<RCCCarControllerV2>().canControl = true;
        racecarobject.GetComponent<RCCCarControllerV2>().runEngineAtAwake = false;
        racecarobject.GetComponent<RCCCarControllerV2>().engineRunning = false;
        racecarobject.GetComponent<RCCCarControllerV2>().KillOrStartEngine();
        mainCamera.GetComponent<RCCCamManager>().enabled = true;
        mainCamera.GetComponent<RCCCarCamera>().playerCar = racecarobject.transform;
        mainCamera.GetComponent<RCCCamManager>().cameraChangeCount = 5;
    }

    void Start()
    {
        mainCamera.GetComponent<RCCCamManager>().ChangeCamera();
    }

    void Update()
    {
        mainCamera.position = Vector3.Lerp(mainCamera.position, carparent.transform.position + (-mainCamera.forward * 12f) + new Vector3(0f, .5f, 0f), Time.deltaTime * 5f);
        mainCamera.rotation = Quaternion.Euler(cameraOffset);
        GetComponent<Camera>().fieldOfView = 50;
    }
}