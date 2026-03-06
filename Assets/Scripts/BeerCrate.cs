using Meta.WitAi;
using Oculus.Interaction;
using UnityEngine;

public class BeerCrate : MonoBehaviour
{
    [SerializeField]
    private GameObject beerCanPrefab;
    [SerializeField]
    private Transform interactibles;

    private GameObject beerCanGhost;
    private Grabbable beerCanGrabbable;
    private SkinnedMeshRenderer beerCanMeshRenderer;
    private bool ghostExists;
    private bool isBeingHeld;

    public bool tempIsGrabbing;

    private void Start()
    {
        ghostExists = false;
        isBeingHeld = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interact" && !ghostExists && GameController.instance.gameIsPlaying)
        {
            beerCanGhost = Instantiate(beerCanPrefab, other.transform.position, Quaternion.identity, interactibles);
            beerCanGrabbable = beerCanGhost.GetComponent<Grabbable>();
            beerCanMeshRenderer = beerCanGhost.GetComponentInChildren<SkinnedMeshRenderer>();
            beerCanMeshRenderer.enabled = false;
            ghostExists = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Interact" && GameController.instance.gameIsPlaying)
        {
            isBeingHeld = beerCanGrabbable.SelectingPointsCount != 0;
            beerCanMeshRenderer.enabled = isBeingHeld;
            ghostExists = !isBeingHeld;
            if (!isBeingHeld && GameController.instance.gameIsPlaying)
            {
                beerCanGhost.transform.position = other.gameObject.transform.position;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interact")
        {
            if (ghostExists)
            {
                beerCanGhost.DestroySafely();
                ghostExists = false;
            }
            beerCanGhost = null;
            beerCanGrabbable = null;
            beerCanMeshRenderer = null;
            
        }
    }

}
