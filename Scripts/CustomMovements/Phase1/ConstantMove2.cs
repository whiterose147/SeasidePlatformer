using System.Collections;
using System.Collections.Generic;
using CMF;
using UnityEngine;

public class ConstantMove2 : MonoBehaviour
{
    private Transform localTransform;
    [SerializeField] private List<Transform> wayPointTransforms;

    [SerializeField] private float rotationSpeed = 1f;
    private float rate;
    public float movementSpeed = 10f;

    private Quaternion currentRotation;
    private Quaternion desiredRotation;

    TriggerArea triggerArea;

    private bool bSequence = false;

    private int index = 0;
    private int movementCounter = 0;

    private void Start(){ triggerArea = GetComponentInChildren<TriggerArea>(); }

    private void OnEnable()
    {
        localTransform = transform;
        MasterPlatformControl.MovingSequence += Movement;
    }

    private void OnDisable() { MasterPlatformControl.MovingSequence -= Movement; }

    private void Movement(bool incomingBool)
    {
        if (index >= 12)
            index = 0;

        if (movementCounter >= 5) // Movement counter needs to be 1 higher than actual num of movement sequences (5 = 4 movement sequences)
            movementCounter = 0;

        if (index == 0 && bSequence == false)
        {
            StartCoroutine(MovePart1());
        }
        else if (index == 5 && bSequence == false)
        {
            StartCoroutine(Rotation1());
        }
        else if (index == 6 && bSequence == false)
        {
            movementCounter = 0;
            StartCoroutine(MovePart2());
        }
        else if (index >= 8  && bSequence == false) // This works I guess LOL
        {
            StartCoroutine(Rotation2());
        }
    }
    IEnumerator MovePart1()
    {
        if (triggerArea != null)
            triggerArea.gameObject.SetActive(true);

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
        if (triggerArea != null)
            triggerArea.gameObject.SetActive(true);

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

    IEnumerator Rotation1()
    {
        if (triggerArea != null)
            triggerArea.gameObject.SetActive(false);

        bSequence = true;
        float incrementRateR = 0;

        while (incrementRateR < 1)
        {
            incrementRateR += Time.deltaTime * rotationSpeed;
            incrementRateR = Mathf.Clamp(incrementRateR, 0, 1);
            currentRotation = localTransform.localRotation;
            desiredRotation = Quaternion.Euler(90, 0, 0);
            localTransform.localRotation = Quaternion.Lerp(currentRotation, desiredRotation, incrementRateR);

            yield return new WaitForEndOfFrame();
        }

        incrementRateR = 0;
        index++;
        bSequence = false;
    }

    IEnumerator Rotation2()
    {
        if (triggerArea != null)
            triggerArea.gameObject.SetActive(false);

        bSequence = true;
        float incrementRateR = 0;

        while (incrementRateR < 1)
        {
            incrementRateR += Time.deltaTime * rotationSpeed;
            incrementRateR = Mathf.Clamp(incrementRateR, 0, 1);
            currentRotation = localTransform.localRotation;
            desiredRotation = Quaternion.Euler(-90, 0, 0);
            localTransform.localRotation = Quaternion.Lerp(currentRotation, desiredRotation, incrementRateR);

            yield return new WaitForEndOfFrame();
        }

        incrementRateR = 0;
        index++;
        bSequence = false;
    }
}
