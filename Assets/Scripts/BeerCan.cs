using System;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class BeerCan : MonoBehaviour
{
    [SerializeField]
    private bool isLeaveCan;

    private Animator animator;
    private int toutchingHandCount;

    public Boolean isOpen;
    public Outline outlineScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outlineScript.enabled = false;
        animator = GetComponent<Animator>();
        toutchingHandCount = 0;
        if (!GameController.instance.canGrabBeerCans && !isLeaveCan)
        {
            GetComponent<Grabbable>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (toutchingHandCount > 0)
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) == 1 || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 1)
            {
                if (!isLeaveCan)
                {
                    OpenBeer();
                }
                else
                {
                    GameController.Restart();
                }
            }
        }
    }

    void OpenBeer()
    {
        if (!isOpen && GameController.instance.gameIsPlaying)
        {
            isOpen = true;
            animator.SetTrigger("OpenBeer");
            GameController.instance.threadLevel += 5;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Interact")
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) == 1 || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 1)
            {
                OpenBeer();
            }
        }
    }

}
