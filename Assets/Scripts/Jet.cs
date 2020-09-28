using UnityEngine;
using UnityEngine.SceneManagement;

public class Jet : MonoBehaviour
{

    Rigidbody rigidbody;
    AudioSource audioSource;

    [SerializeField] float THRUST_FORCE = 900f;
    [SerializeField] float ROTATE_SPEED = 200f;

    int currentScene;
    [SerializeField] int lastLevelIndex = 4;

    float TIME_DELAY = 2f;

    [SerializeField] AudioClip[] audioClip;

    [SerializeField] ParticleSystem[] particleSystem;


    enum State { Alive, Die, Transcending}
    State state = State.Alive;

    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }


    void Update()
    {

        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * THRUST_FORCE * Time.deltaTime);
            
            if (!audioSource.isPlaying)
            {
                // audio and particle effect is handled
                audioSource.PlayOneShot(audioClip[0]);
                particleSystem[0].Play();
            }

        }
        else
        {
            // audio and particle effect is handled
            audioSource.Stop();
            particleSystem[0].Stop();
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
                
                // audio is handled
                audioSource.Stop();
                audioSource.PlayOneShot(audioClip[2]);

                // particle effect is handled
                particleSystem[0].Stop();
                particleSystem[1].Play();
                
                // next scene is handled
                Invoke("RestartScene", TIME_DELAY);
                break;

            case "Finish":
                state = State.Transcending;

                // audio is handled
                audioSource.Stop();
                audioSource.PlayOneShot(audioClip[1]);

                // particle is handled
                particleSystem[0].Stop();

                // next scene is handled
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
