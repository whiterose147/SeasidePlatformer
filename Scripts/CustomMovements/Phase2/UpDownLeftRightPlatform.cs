using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class UpDownLeftRightPlatform : MonoBehaviour
{
    private Transform localTransform;
    [SerializeField] private List<Transform> wayPointUpDown;
    [SerializeField] private List<Transform> wayPointLeftRight;


    [SerializeField] private float rotationSpeed = 1f;
    private float rate;

    private Quaternion currentRotation;
    private Quaternion desiredRotation;

    private GameObject player;

    private bool bSequence = false;
    
    private int index = 0;
    private int movementCounter = 0;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        localTransform = transform;
    }

    private void OnDisable()
    {
        // This will be the only platform not to inherit from the Master Timer.
    }

    private void Update()
    {
        if (index == 4)
        {
            index = 0;
            movementCounter = 0;
            wayPointUpDown.Reverse();
        }
     
        if (bSequence == false && index == 0)
            StartCoroutine(Rotation1());


        else if (bSequence == false && index == 1)
            StartCoroutine(Rotation2());

        
        else if (bSequence == false && index == 2)
            StartCoroutine(MovePart1());

        else if (bSequence == false && index == 3)
        {
            movementCounter = 0;
            StartCoroutine(MovePart3());
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
            localTransform.position = Vector3.Lerp(wayPointUpDown[0].position, wayPointUpDown[1].position, rate); //Blend between the waypoints using lerp

            if (localTransform.position == wayPointUpDown[1].position)
                incrementRate = 1;

            yield return new WaitForEndOfFrame();
        }

        if (rate >= 1)
        {
            rate = 0;
            bSequence = false;
        }

        movementCounter++;
        if (movementCounter != 5)
        {
            StartCoroutine(MovePart2());
        }
        else if (movementCounter == 5)
            index = 3;
    }



    IEnumerator MovePart2()
    {
        bSequence = true;
        float incrementRate = 0;

        while (incrementRate != 1)
        {
            rate += Time.deltaTime;
            Mathf.Clamp(rate, 0f, 1f);
            localTransform.position = Vector3.Lerp(wayPointUpDown[1].position, wayPointUpDown[0].position, rate); //Blend between the waypoints using lerp

            if (localTransform.position == wayPointUpDown[0].position)
                incrementRate = 1;

            yield return new WaitForEndOfFrame();
        }

        if (rate >= 1)
        {
            rate = 0;
            bSequence = false;
        }

        movementCounter++;
        if (movementCounter != 5)
            StartCoroutine(MovePart1());
    }

    IEnumerator Rotation1()
    {
        bSequence = true;
        float incrementRateR = 0;

        while (incrementRateR < 1)
        {
            if (player.transform.parent != null)
                player.transform.parent = null;

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
        bSequence = true;
        float incrementRateR = 0;

        while (incrementRateR < 1)
        {

            if (player.transform.parent != null)
                player.transform.parent = null;

            incrementRateR += Time.deltaTime * rotationSpeed;
            incrementRateR = Mathf.Clamp(incrementRateR, 0, 1);
            currentRotation = localTransform.localRotation;
            desiredRotation = Quaternion.Euler(-90, 0, 0);
            localTransform.localRotation = Quaternion.Lerp(currentRotation, desiredRotation, incrementRateR);

            yield return new WaitForEndOfFrame();
        }

        incrementRateR = 0;
        index = 2;
        bSequence = false;
    }
    IEnumerator MovePart3()
    {
        bSequence = true;
        float incrementRate = 0;

        while (incrementRate != 1)
        {
            rate += Time.deltaTime;
            Mathf.Clamp(rate, 0f, 1f);
            localTransform.position = Vector3.Lerp(wayPointLeftRight[0].position, wayPointLeftRight[1].position, rate); //Blend between the waypoints using lerp

            if (localTransform.position == wayPointLeftRight[1].position)
                incrementRate = 1;

            yield return new WaitForEndOfFrame();
        }

        if (rate >= 1)
        {
            rate = 0;
            bSequence = false;
        }

        movementCounter++;
        if (movementCounter != 6)
        {
            StartCoroutine(MovePart4());
        }
        else if (movementCounter == 6)
        {
            index = 4;
        }
    }
    IEnumerator MovePart4()
    {
        bSequence = true;
        float incrementRate = 0;

        while (incrementRate != 1)
        {
            rate += Time.deltaTime;
            Mathf.Clamp(rate, 0f, 1f);
            localTransform.position = Vector3.Lerp(wayPointLeftRight[1].position, wayPointLeftRight[0].position, rate); //Blend between the waypoints using lerp

            if (localTransform.position == wayPointLeftRight[0].position)
                incrementRate = 1;

            yield return new WaitForEndOfFrame();
        }

        if (rate >= 1)
        {
            rate = 0;
            bSequence = false;
        }
        movementCounter++;

        if (movementCounter != 6)
            StartCoroutine(MovePart3());
    }
}
