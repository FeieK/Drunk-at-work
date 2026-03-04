using System.Collections;
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
    private Color redScanColor;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private int raysAmount;
    [SerializeField]
    private float rayWidth;
    [SerializeField]
    private float rayLenght;

    private GotoPos gotoPos;
    private Material scannerMaterial;
    private GameController gameController;


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
        gameController = GameController.instance;
        scannerMaterial = scannerObj.GetComponent<MeshRenderer>().material;
        scannerObj.SetActive(false);

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
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        scannerObj.SetActive(true);
        bool scanning = true;
        float rotation = 75;
        while (scanning)
        {
            rotation += scanSpeed * Time.deltaTime;
            float finalRotation = Mathf.Clamp(rotation, startScanAngle, endScanAngle);
            scannerObj.transform.localRotation = Quaternion.Euler(finalRotation, 7.17f, 0);

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
                        Outline outline = hit.collider.GetComponent<Outline>();
                        outline.enabled = true;
                        scannerMaterial.color = redScanColor;
                        yield return new WaitForSeconds(5);
                        scanning = false;
                        gameController.timesCought += 1; // Its not grabbing this for some reason???
                    }

                }
            }

            yield return new WaitForEndOfFrame();
            if (finalRotation >= endScanAngle) scanning = false;
        }
        scannerObj.SetActive(false);
        scannerMaterial.color = greenScanColor;
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        gotoPos = GotoPos.END;
    }
}
