using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private const string playerPrefsMusicVolume = "MusicVolume";
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat(playerPrefsMusicVolume, 1f);
    }
    public void ChangeVolume(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat(playerPrefsMusicVolume, value);
        PlayerPrefs.Save();
    }


    public float GetVolume()
    {
        return audioSource.volume;
    }
}
