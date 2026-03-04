using UnityEngine;

public class DrawerInteract : MonoBehaviour
{
    [SerializeField]
    private Drawer drawer;
    [SerializeField]
    private DrawerSide drawerSide;

    private int toutchingHandCount;

    private enum DrawerSide
    {
        TOP, BOTTOM
    }

    private void Update()
    {
        if (toutchingHandCount > 0)
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) == 1 || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 1)
            {
                switch (drawerSide) {
                    case DrawerSide.TOP: drawer.InteractTop();
                        break;
                    case DrawerSide.BOTTOM: drawer.InteractBottom();
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interact")
        {
            toutchingHandCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interact")
        {
            toutchingHandCount--;
        }
    }
}
