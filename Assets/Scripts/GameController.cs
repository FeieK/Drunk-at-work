using System.Collections;
using Oculus.Interaction.Locomotion;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;

public class GameController : MonoBehaviour
{
    public FirstPersonLocomotor firstPersonLocomotor;
    public Transform rig;
    public Transform head;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstPersonLocomotor.DisableMovement();
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Start)) Teleport();

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Teleporting...");
        Teleport();
    }
    private void Teleport()
    {
        Vector3 headOffset = rig.position - head.position;
        rig.position = new Vector3(0, head.position.y, 0) + headOffset;
    }
}
