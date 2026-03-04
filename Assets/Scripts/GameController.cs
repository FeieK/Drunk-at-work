using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Locomotion;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public FirstPersonLocomotor firstPersonLocomotor;
    public Transform rig;
    public Transform head;
    public ManagerBehaviour manager;
    public int timesCought;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        timesCought -= 1;
        firstPersonLocomotor.DisableMovement();


        StartCoroutine(DelayTeleport());
        StartCoroutine(FirstManagerCycle());
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Start)) Teleport();

    }

    IEnumerator DelayTeleport()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Teleporting...");
        Teleport();
    }

    IEnumerator FirstManagerCycle()
    {
        yield return new WaitForSeconds(5);
        manager.TriggerWalk();
        yield return new WaitForSeconds(60);
        StartCoroutine(TryManagerCycle());
    }
    IEnumerator TryManagerCycle()
    {
        yield return new WaitForSeconds(30);
        if (Random.Range(0, 5) == 5)
        {
            manager.TriggerWalk();
        }

    }
    private void Teleport()
    {
        Vector3 headOffset = rig.position - head.position;
        rig.position = new Vector3(0, head.position.y, 0) + headOffset;
    }
}
