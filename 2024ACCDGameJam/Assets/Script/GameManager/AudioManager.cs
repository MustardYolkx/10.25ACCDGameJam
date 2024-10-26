using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    public AudioSource bgmSource;
    public AudioClip scene1BGM;   // cover scene bgm
    public AudioClip scene3BGM;   // main scene bgm

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Register scene switching events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the scene index
        if (scene.buildIndex == 0)
        {
            PlayBGM(scene1BGM); // BGM of the cover scene
        }
        else if (scene.buildIndex == 2)
        {
            PlayBGM(scene3BGM); // BGM of the main scene
        }
        // The second scene does not need to call PlayBGM, because it will continue to play the BGM of the first scene.
    }

    // Play bgm
    public void PlayBGM(AudioClip clip)
    {
        // If the currently playing BGM is already the target BGM, do not play it again
        if (bgmSource.clip == clip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
