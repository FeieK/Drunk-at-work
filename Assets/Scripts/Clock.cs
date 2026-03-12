using System;
using Oculus.Interaction;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private Grabbable grabbable;
    private Rigidbody rb;

    [SerializeField]
    private Transform hourTransform;
    [SerializeField]
    private Transform minuteTransform;
    [SerializeField]
    private Transform secondTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbable.SelectingPointsCount > 0 && rb.constraints != RigidbodyConstraints.None)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    void FixedUpdate()
    {
        float second = DateTime.Now.Second;
        float minute = DateTime.Now.Minute;
        float hour = DateTime.Now.Hour;

        float mS = minute + second / 60;
        float hMS = hour + (minute + second / 60) / 60;
        secondTransform.localRotation = Quaternion.Euler(0, -(second * 6) + 96, 0);
        minuteTransform.localRotation = Quaternion.Euler(0, -(mS * 6) + 96, 0);
        hourTransform.localRotation = Quaternion.Euler(0, -(hMS * 30) + 96, 0);
    }
}
