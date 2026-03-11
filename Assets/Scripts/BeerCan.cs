using System;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class BeerCan : MonoBehaviour
{
    private Animator animator;
    private Grabbable grabbable;

    public bool isOpen;
    public bool canDrink;
    public Outline outlineScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outlineScript.enabled = false;
        animator = GetComponent<Animator>();
        grabbable = GetComponent<Grabbable>();
        if (!GameController.instance.canGrabBeerCans)
        {
            GetComponent<Grabbable>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.SelectingPointsCount > 0)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                OpenBeer();
            }
        }
    }

    public void OpenBeer()
    {
        if (!isOpen && GameController.instance.gameIsPlaying)
        {
            isOpen = true;
            animator.SetTrigger("OpenBeer");
            GameController.instance.threadLevel += 5;
        }
    }

}
