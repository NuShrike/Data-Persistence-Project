using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public MainManager Manager;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        _audioSource.Play();

        Destroy(other.gameObject);
        Manager.GameOver();
    }
}
