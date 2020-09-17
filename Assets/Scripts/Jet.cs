using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jet : MonoBehaviour
{

    Rigidbody rigidbody;
    AudioSource audioSource;
    [SerializeField] float THRUST_FORCE = 900f;
    [SerializeField] float ROTATE_SPEED = 200f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * THRUST_FORCE * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * ROTATE_SPEED * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * ROTATE_SPEED * Time.deltaTime);
        }
        else
        {
            rigidbody.AddRelativeForce(Vector3.zero);
        }
        rigidbody.freezeRotation = true;
    }
}
