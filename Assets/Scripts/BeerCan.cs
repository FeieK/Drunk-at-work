using System;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class BeerCan : MonoBehaviour
{
    private Animator animator;
    public Boolean isOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OpenBeer()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetTrigger("OpenBeer");
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
