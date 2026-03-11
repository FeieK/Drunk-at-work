using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Locomotion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public FirstPersonLocomotor firstPersonLocomotor;
    public Transform rig;
    public Transform head;

    public ManagerBehaviour manager;
    public bool canGrabBeerCans;
    public int threadLevel;

    public bool gameIsPlaying;

    public int beersDrunk;
    public int negativePoints;
    public int minutesSurvived;
    [Range(0f, 100f)]
    public int deductionPercent;
    public int scansSurvived;

    public int totalPoints;

    public bool hasGameInfoOpen;

    [SerializeField]
    private GameObject gameInfoObj;
    [SerializeField]
    private GameObject gameInfoHpObj;
    private Material gameInfoHpMaterial;
    [SerializeField]
    private TextMeshPro beersDrunkText;
    [SerializeField]
    private TextMeshPro scansSurvivedText;
    [SerializeField]
    private TextMeshPro timePassedText;

    [SerializeField]
    private TextMeshPro firedBeersDrunk;
    [SerializeField]
    private TextMeshPro firedTime;
    [SerializeField]
    private TextMeshPro firedScansSurvived;
    [SerializeField]
    private TextMeshPro firedNegativePoints;
    [SerializeField]
    private TextMeshPro firedTotalPoints;


    [SerializeField]
    private Image sreenDarkenImage;

    private float holdSettingsTime;
    private bool isHolding;
    private bool hasTriggeredEnding;
    void Awake()
    {
        instance = this;
        canGrabBeerCans = false;
        threadLevel = 0;
        gameIsPlaying = false;
        beersDrunk = 0;
        negativePoints = 0;
        minutesSurvived = 0;
        scansSurvived = 0;
        totalPoints = 0;
        hasGameInfoOpen = false;
        hasTriggeredEnding = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameInfoHpMaterial = gameInfoHpObj.GetComponent<MeshRenderer>().material;
        firstPersonLocomotor.DisableMovement();
        StartCoroutine(Delay());
        StartCoroutine(FirstManagerCycle());
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start) || Input.GetKeyDown(KeyCode.I))
        {
            holdSettingsTime = 0;
            isHolding = false;
        }
        if (OVRInput.Get(OVRInput.Button.Start) || Input.GetKey(KeyCode.I))
        {
            holdSettingsTime += Time.deltaTime;

            if (holdSettingsTime > 1 && !isHolding)
            {
                isHolding = true;
                if (!hasTriggeredEnding)
                {
                    Teleport(Vector2.zero);
                }
                else
                {
                    Restart();
                }
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.Start) || Input.GetKeyUp(KeyCode.I))
        {
            if (!isHolding && !hasTriggeredEnding && gameIsPlaying)
            {
                OpenInfoMenu();
            }
        }

        if (Input.GetKey(KeyCode.N) && !hasTriggeredEnding) StartCoroutine(TriggerEnding());

        if (deductionPercent >= 100 && !hasTriggeredEnding) StartCoroutine(TriggerEnding());
    }

    public void StartGame()
    {
        gameIsPlaying = true;
        canGrabBeerCans = true;
        StartCoroutine(TryManagerCycle());
        StartCoroutine(TimePoints());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Teleport(Vector2.zero);
    }

    IEnumerator FirstManagerCycle()
    {
        yield return new WaitForSeconds(5);
        manager.TriggerWalk(true);
    }
    IEnumerator TryManagerCycle()
    {
        while (gameIsPlaying)
        {
            float waitTime = Mathf.Clamp(10 - ((float)beersDrunk / 10), 1, 10);
            if (threadLevel >= 60)
            {
                manager.TriggerWalk();
                threadLevel = Random.Range(0, 10);
                yield return new WaitForSeconds(30);
            }
            else
            {
                threadLevel += 1;
            }
            yield return new WaitForSeconds(waitTime);
        }

    }
    IEnumerator TimePoints()
    {
        while (gameIsPlaying)
        {
            yield return new WaitForSeconds(60);
            minutesSurvived += 1;
        }
    }

    IEnumerator TriggerEnding()
    {
        hasTriggeredEnding = true;
        gameIsPlaying = false;
        canGrabBeerCans = false;

        float alpha = 0;

        bool fadingIn = true;
        while (fadingIn)
        {
            alpha += 0.01f;
            sreenDarkenImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForEndOfFrame();

            if (alpha >= 1) fadingIn = false;
        }
        yield return new WaitForEndOfFrame();

        firedBeersDrunk.text = $"Beers drunk: {beersDrunk * 35}";
        firedTime.text = $"Time: {minutesSurvived * 2}";
        firedScansSurvived.text = $"Scans survived: {scansSurvived * 25}";
        firedNegativePoints.text = $"Negative points: {negativePoints}";

        totalPoints = (beersDrunk * 35) + (minutesSurvived * 2) + (scansSurvived * 25) - negativePoints;
        firedTotalPoints.text = $"Total: {totalPoints}";

        Teleport(new Vector2(0, -74));
        yield return new WaitForSeconds(1);
        bool fadingOut = true;
        while (fadingOut)
        {
            alpha -= 0.01f;
            sreenDarkenImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForEndOfFrame();

            if (alpha <= 0) fadingOut = false;
        }
    }
    private void OpenInfoMenu()
    {
        UpdateGameInfo();
        hasGameInfoOpen = !hasGameInfoOpen;
        if (hasGameInfoOpen)
        {
            gameInfoObj.transform.position = head.transform.position + head.transform.forward * .5f;
            gameInfoObj.transform.rotation = head.transform.rotation * Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            gameInfoObj.transform.position = new Vector3(0, 20, 0);
        }
    }

    private void Teleport(Vector2 newPos)
    {
        Debug.Log("Teleporting...");
        Vector3 headOffset = rig.position - head.position;
        rig.position = new Vector3(newPos.x, head.position.y, newPos.y) + headOffset;
    }

    public void UpdateGameInfo()
    {
        gameInfoHpMaterial.SetFloat("_ClipPercent", -(deductionPercent - 100));

        beersDrunkText.text = $"Beers drunk: {beersDrunk}";

        scansSurvivedText.text = $"Scans survived: {scansSurvived}";

        int minute = minutesSurvived % 60;
        int hour = (minutesSurvived - minute) / 60;
        string hourText = hour < 10 ? $"0{hour}" : $"{hour}";
        string minuteText = minute < 10 ? $"0{minute}" : $"{minute}";
        timePassedText.text = $"Time passed: {hourText}:{minuteText}";
    }

    public static void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
