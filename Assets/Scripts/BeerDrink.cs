using UnityEngine;

public class BeerDrink : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BeerCan"))
        {
            BeerCan beerCanScript = other.transform.parent.gameObject.GetComponent<BeerCan>();
            if (beerCanScript != null) {
                if (beerCanScript.isOpen && beerCanScript.canDrink)
                {
                    beerCanScript.canDrink = false;
                    GameController.instance.beersDrunk += 1;
                }
            }
        }
    }
}
