using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f,0f, 0f);
    float movementFactor;

    [Range(2,5)][SerializeField] float period = 2f;

    Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }

}
