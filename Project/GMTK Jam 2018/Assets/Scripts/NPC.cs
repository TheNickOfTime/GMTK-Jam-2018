using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    // The target marker.
    public Transform target;
    public Transform[] targetNumber;

    public bool talk;
    public bool isMoving;
    // Speed in units per sec.
    public float speed;
    public float time;

    void Update()
    {
        // Get the range from the target (Player) to the enemy.
        float range = Vector3.Distance(transform.position, target.transform.position);
        // The step size is equal to speed times frame time.
        float step = speed * Time.deltaTime;

        // Move our position a step closer to the target.
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }

        if (range <= 1)
        {
            StartCoroutine(Socialize());
            talk = !talk;
            //StopCoroutine(Socialize());
        }
        if (talk)
        {
            target = targetNumber[0];
        }
        else if (!talk)
        {
            target = targetNumber[1];
        }
                  
    }

    public IEnumerator Socialize()
    {
        isMoving = false;
        yield return new WaitForSeconds(time);
        isMoving = true;
    }

}
