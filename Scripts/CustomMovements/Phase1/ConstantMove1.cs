using System.Collections;
using System.Collections.Generic;
using CMF;
using UnityEngine;

public class ConstantMove1 : MonoBehaviour
{
    [SerializeField] private List<Transform> wayPointTransforms;
    private Transform localTransform;
    
    private float rate;
    public float movementSpeed = 10f;

    private int index = 0;
    private int movementCounter = 0;

    private bool bSequence = false;

    TriggerArea triggerArea;

    private void Start(){ triggerArea = GetComponentInChildren<TriggerArea>(); }

    private void OnEnable()
    {
        localTransform = transform;
        MasterPlatformControl.MovingSequence += Movement;
    }
    private void OnDisable(){ MasterPlatformControl.MovingSequence -= Movement; }

    private void Update()
    {
        if (index >= 14)
            index = 0;
    }

    private void Movement(bool incomingBool)
    {
        if (movementCounter >= 5)
            movementCounter = 0;

        if (index == 0 && bSequence == false)
        {
            StartCoroutine(MovePart1());
        }

        if (index == 6 && bSequence == false)
        {
            StartCoroutine(MovePart2());
        }

        if (index >= 13)
        {
            StartCoroutine(MovePart1());
        }

        if (index == 5) // Pause until we get to 6 for the next movement.
            index++;

        if(index >= 9 && index <= 13)
        {
            index++;
        }

    }
    IEnumerator MovePart1()
    {
        bSequence = true;
        float incrementRate = 0;

        while (incrementRate != 1)
        {
            rate += Time.deltaTime;
            Mathf.Clamp(rate, 0f, 1f);
            localTransform.position = Vector3.Lerp(wayPointTransforms[0].position, wayPointTransforms[1].position, rate); //Blend between the waypoints using lerp

            if (localTransform.position == wayPointTransforms[1].position)
                incrementRate = 1;

            //Move all controllers on top of the platform the same distance
            if (triggerArea != null)
            {

                //Calculate a vector to the current waypoint
                Vector3 toCurrentWaypoint = wayPointTransforms[1].position - localTransform.position;

                //Get normalized movement direction
                Vector3 movement = toCurrentWaypoint.normalized;

                //Get movement for this frame
                movement *= movementSpeed * Time.deltaTime;

                for (int i = 0; i < triggerArea.rigidbodiesInTriggerArea.Count; i++)
                {
                    triggerArea.rigidbodiesInTriggerArea[i].MovePosition(triggerArea.rigidbodiesInTriggerArea[i].position + movement);
                }
            }
                
            yield return new WaitForEndOfFrame();
        }

        if (rate >= 1)
        {
            rate = 0;
            index++;
            bSequence = false;
        }

        movementCounter++;

        if (movementCounter != 5)
            StartCoroutine(MovePart2());
        
    }

    IEnumerator MovePart2()
    {
        bSequence = true;
        float incrementRate = 0;

        while (incrementRate != 1)
        {
            rate += Time.deltaTime;
            Mathf.Clamp(rate, 0f, 1f);
            localTransform.position = Vector3.Lerp(wayPointTransforms[1].position, wayPointTransforms[0].position, rate); //Blend between the waypoints using lerp

            if (localTransform.position == wayPointTransforms[0].position)
                incrementRate = 1;

            //Move all controllers on top of the platform the same distance
            if (triggerArea != null)
            {
                //Calculate a vector to the current waypoint
                Vector3 toCurrentWaypoint = wayPointTransforms[0].position - localTransform.position;

                //Get normalized movement direction
                Vector3 movement = toCurrentWaypoint.normalized;

                //Get movement for this frame
                movement *= movementSpeed * Time.deltaTime;

                for (int i = 0; i < triggerArea.rigidbodiesInTriggerArea.Count; i++)
                {
                    triggerArea.rigidbodiesInTriggerArea[i].MovePosition(triggerArea.rigidbodiesInTriggerArea[i].position + movement);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        if (rate >= 1)
        {
            rate = 0;
            index++;
            bSequence = false;
        }

        movementCounter++;
        if (movementCounter != 5)
           StartCoroutine(MovePart1());

    }
}
