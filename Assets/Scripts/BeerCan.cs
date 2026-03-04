using System;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class BeerCan : MonoBehaviour
{
    [SerializeField]
    private Outline outlineScript;

    private Animator animator;
    private int toutchingHandCount;


    public Boolean isOpen;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController gameController = GameController.instance;
        outlineScript.enabled = false;
        animator = GetComponent<Animator>();
        toutchingHandCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (toutchingHandCount > 0)
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) == 1 || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 1)
            {
                OpenBeer();
            }
        }
    }

    void OpenBeer()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetTrigger("OpenBeer");
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
