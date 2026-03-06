using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CubicalDetect : MonoBehaviour
{
    List<Transform> list = new List<Transform>();

    void FixedUpdate()
    {
        foreach (Transform can in list)
        {
            if (can.localScale.x <= 0.01f)
            {
                list.Remove(can);
                can.gameObject.SetActive(false);

                break;
            }
            can.localScale = new Vector3(can.localScale.x - 0.01f, can.localScale.y - 0.01f, can.localScale.z - 0.01f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BeerCan"))
        {
            
            if (!list.Contains(other.transform.parent) && other.transform.parent.gameObject.activeSelf)
            {
                StartCoroutine(DelayAdd(other.transform.parent));
                GameController gameController = GameController.instance;
                gameController.threadLevel += 20;
                gameController.negativePoints += 250;
                gameController.deductionPercent += 2;
            }
        }
    }

    IEnumerator DelayAdd(Transform transform)
    {
        GameController gameController = GameController.instance;
        yield return new WaitForSeconds(3);
        if (!list.Contains(transform) && transform.gameObject.activeSelf)
        {
            gameController.negativePoints += 25;
            list.Add(transform);
        }
    }
}
