using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class Jet : MonoBehaviour
{

    Rigidbody rigidbody;
    AudioSource audioSource;

    [SerializeField] float THRUST_FORCE = 900f;
    [SerializeField] float ROTATE_SPEED = 200f;

    int currentScene;
    [SerializeField] int lastLevelIndex = 3;

    float TIME_DELAY = 2f;

    [SerializeField] AudioClip[] audioClip; 


    enum State { Alive, Die, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Alive:
                Thrust();
                Rotate();
                break;

            case State.Transcending:
                audioSource.PlayOneShot(audioClip[1]);
                break;

            case State.Die:
                audioSource.PlayOneShot(audioClip[2]);
                break;

            default:
                audioSource.Stop();
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * THRUST_FORCE * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClip[0]);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Wall":
                state = State.Die;
                Invoke("RestartScene", TIME_DELAY);
                break;

            case "Finish":
                state = State.Transcending;
                Invoke("CheckForNextLevel", TIME_DELAY);
                break;
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    private void CheckForNextLevel()
    {
        if (currentScene == lastLevelIndex)
        {
            SceneManager.LoadScene(0);
            return;
        }
        SceneManager.LoadScene(currentScene + 1);
    }
}
