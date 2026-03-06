using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class ManagerBehaviour : MonoBehaviour
{

    [SerializeField]
    private Vector2 scanPos;
    [SerializeField]
    private Vector2 startPos;
    [SerializeField]
    private Vector2 endPos;
    [SerializeField]
    private Vector2 standbyPos;
    [SerializeField]
    private float startScanAngle;
    [SerializeField]
    private float endScanAngle;
    [SerializeField]
    private float scanSpeed;
    [SerializeField]
    private GameObject scannerObj;
    [SerializeField]
    private Color greenScanColor;
    [SerializeField]
    private Color greenEmisionScanColor;
    [SerializeField]
    private Color redScanColor;
    [SerializeField]
    private Color redEmisionScanColor;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private int raysAmount;
    [SerializeField]
    private float rayWidth;
    [SerializeField]
    private float rayLenght;
    [SerializeField]
    private GameObject noBeerPaperHand;
    [SerializeField]
    private GameObject noBeerPapersWallParent;

    private GotoPos gotoPos;
    private Material scannerMaterial;
    private bool firstCycle;

    private bool foundCan;
    private GameObject foundCanParent;
    private GameObject foundCanModel;

    public bool showScanGizmo;


    private enum GotoPos
    {
        START,
        SCAN,
        END,
        STANDBY,
        WAIT
    }
    void Start()
    {
        scannerMaterial = scannerObj.GetComponent<MeshRenderer>().material;
        scannerObj.SetActive(false);
        noBeerPaperHand.SetActive(false);
        noBeerPapersWallParent.SetActive(false);
        firstCycle = true;
        foundCan = false;
        gotoPos = GotoPos.STANDBY;

    }

    void Update()
    {
        switch (gotoPos)
        {
            case GotoPos.START:
                {
                    Vector3 toPos = GetV3Pos(startPos);
                    transform.position = toPos;
                    gotoPos = GotoPos.SCAN;
                    return;
                }
            case GotoPos.SCAN:
                {
                    Vector3 toPos = GetV3Pos(scanPos);
                    if (transform.position == toPos)
                    {
                        gotoPos = GotoPos.WAIT;
                        StartCoroutine(Scan());
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, toPos, walkSpeed * Time.deltaTime);
                    }
                    return;
                }
            case GotoPos.END:
                {
                    Vector3 toPos = GetV3Pos(endPos);
                    if (transform.position == toPos)
                    {
                        gotoPos = GotoPos.STANDBY;
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, toPos, walkSpeed * Time.deltaTime);
                    }
                    return;
                }
            case GotoPos.STANDBY:
                {
                    Vector3 toPos = GetV3Pos(standbyPos);
                    transform.position = toPos;
                    scannerObj.transform.localRotation = Quaternion.Euler(startScanAngle, 0, 0);
                    gotoPos = GotoPos.WAIT;
                    return;
                }
        }
    }

    public void TriggerWalk()
    {
        gotoPos = GotoPos.START;
    }

    private Vector3 GetV3Pos(Vector2 pos)
    {
        return new Vector3(pos.x, transform.position.y, pos.y);
    }

    IEnumerator Scan()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
        scannerObj.SetActive(true);
        bool scanning = true;
        foundCan = false;
        float rotation = 75;
        GameController gameController = GameController.instance;
        while (scanning)
        {
            rotation += scanSpeed * Time.deltaTime;
            float finalRotation = Mathf.Clamp(rotation, startScanAngle, endScanAngle);
            scannerObj.transform.localRotation = Quaternion.Euler(finalRotation, 0, 0);

            for (float ray = 0; ray <= raysAmount; ray++)
            {
                float singleAngle = (rayWidth * 2) / raysAmount;
                float rayAngle = (singleAngle * ray) - rayWidth;
                Vector3 targetPoint = new Vector3(rayAngle, rayLenght, 0);
                RaycastHit hit;

                if (Physics.Raycast(scannerObj.transform.position, scannerObj.transform.TransformDirection(targetPoint), out hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("BeerCan"))
                    {
                        foundCanParent = hit.collider.transform.parent.gameObject;
                        foundCanModel = hit.collider.gameObject;
                        foundCan = true;
                        Outline outline = foundCanModel.GetComponent<Outline>();
                        outline.enabled = true;
                        scannerMaterial.color = redScanColor;
                        scannerMaterial.SetColor("_EmissionColor", redEmisionScanColor);
                        yield return new WaitForSeconds(5);
                        scanning = false;

                        if (firstCycle)
                        {
                            noBeerPaperHand.SetActive(true);
                        }
                        else
                        {
                            gameController.negativePoints += 800;
                            gameController.deductionPercent += 20;
                        }
                    }

                }
            }

            yield return new WaitForEndOfFrame();
            if (finalRotation >= endScanAngle) scanning = false;
        }
        scannerObj.SetActive(false);
        scannerMaterial.color = greenScanColor;
        scannerMaterial.SetColor("_EmissionColor", greenEmisionScanColor);

        if (firstCycle)
        {
            noBeerPaperHand.SetActive(true);
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
        if (foundCan)
        {
            foundCanParent.GetComponent<Rigidbody>().useGravity = false;
            foundCanParent.transform.position = new Vector3(5, 0, 20);
            foundCanModel.SetActive(false);
        }
        else
        {
            gameController.scansSurvived += 1;
        }
        if (firstCycle)
        {
            noBeerPaperHand.SetActive(false);
            noBeerPapersWallParent.SetActive(true);
            gameController.StartGame();
            firstCycle = false;
        }
        gotoPos = GotoPos.END;
    }

    void OnDrawGizmos()
    {
        if (showScanGizmo)
        {
            Gizmos.color = Color.blue;
            for (float ray = 0; ray <= raysAmount; ray++)
            {
                float singleAngle = (rayWidth * 2) / raysAmount;
                float rayAngle = (singleAngle * ray) - rayWidth;
                Vector3 targetPoint = new Vector3(rayAngle, rayLenght, 0);
                Gizmos.DrawRay(scannerObj.transform.position, scannerObj.transform.TransformDirection(targetPoint));
            }
        }
    }
}
