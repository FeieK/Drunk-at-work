using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class DrawerHoldObj : MonoBehaviour
{

    [SerializeField]
    private GameObject world;

    private List<GameObject> insideObjects = new List<GameObject>();
    private List<GameObject> preOutsideObjects = new List<GameObject>();
    void Update()
    {
        //foreach (GameObject obj in insideObjects) {
        //    Grabbable grab = obj.GetComponent<Grabbable>();
        //    if (grab.SelectingPointsCount == 0)
        //    {
        //        obj.gameObject.transform.parent = transform;
        //    }
        //}
        //foreach (GameObject obj in preOutsideObjects) {
        //    Grabbable grab = obj.GetComponent<Grabbable>();
        //    if (grab.SelectingPointsCount == 0)
        //    {
        //        obj.gameObject.transform.parent = world.transform;
        //    }
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Interactible" && !insideObjects.Contains(other.gameObject))
    //    {
    //        insideObjects.Add(other.gameObject);

    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Interactible" && insideObjects.Contains(other.gameObject))
    //    {
    //        preOutsideObjects.Add(other.gameObject);
    //        insideObjects.Remove(other.gameObject);
    //    }
    //}
}
