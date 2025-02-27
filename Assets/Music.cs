using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private AudioClip[] musicSounds;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = musicSounds[Random.Range(0, musicSounds.Length)];
        _source.Play();
    }
}
