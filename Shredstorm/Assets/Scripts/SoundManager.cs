using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        Goggle,
        Vygor,
        Star,
        Misfit,
        Announcer,
        Encounter
    }

    private AudioSource audioSource;
    [SerializeField] private AudioClip [] soundList;
    //[UnityEngine.RequireComponent(typeof(Collider))]


    public static SoundManager Instance { get; set; }

    private void Awake()
    {
        {
            Instance = this;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        Instance.audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }

}
