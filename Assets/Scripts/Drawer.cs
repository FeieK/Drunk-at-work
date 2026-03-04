using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{


    private Animator animator;

    private bool topIsOpen;
    private bool bottomIsOpen;


    public static bool isAnimatingADrawer;
    void Start()
    {
        animator = GetComponent<Animator>();
        topIsOpen = false;
        bottomIsOpen = false;
        isAnimatingADrawer = false;
    }

    public void InteractTop()
    {
        topIsOpen = !topIsOpen;
        animator.SetBool("TopIsOpen", topIsOpen);
        StartCoroutine(SetBoolOnAniStop());
    }

    public void InteractBottom()
    {
        bottomIsOpen = !bottomIsOpen;
        animator.SetBool("BottomIsOpen", bottomIsOpen);
        StartCoroutine(SetBoolOnAniStop());
    }

    private IEnumerator SetBoolOnAniStop()
    {
        isAnimatingADrawer = true;
        yield return new WaitForSeconds(1);
        isAnimatingADrawer = false;
    }
}
