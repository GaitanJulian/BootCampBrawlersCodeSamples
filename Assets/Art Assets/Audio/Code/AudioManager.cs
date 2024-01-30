using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer audioMixer;


    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSourcePlayer;
    [SerializeField] AudioSource sfxSourceEnvironment;

    [Header("Audio Source 2")]
    [SerializeField] AudioSource sfxJump;
    [SerializeField] AudioSource sfxPunch;

    [Header("Audio Clips Environment")]
    public AudioClip menuMusic;
    public AudioClip flagRaceMusic;
    public AudioClip fastLaneRaceMusic;
    public AudioClip circuitMusic;
    public AudioClip dodgeMusic;
    public AudioClip supplyChaseMusic; 
    public AudioClip win;
    public AudioClip moveUI;
    public AudioClip select;
    public AudioClip unselect;



    [Header("Audio Clips Player")]
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip punch;
    [SerializeField] AudioClip[] walk;
    [SerializeField] AudioClip[] jump;
    [SerializeField] AudioClip flareShot;
    [SerializeField] AudioClip raceStop;
    [SerializeField] AudioClip raceGo;


    [Header("VariableToMethods")]
    private int lastPlayedIndex = -1;
    private int lastJumpIndex = -1;
    private bool isWalking = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void StartBackgroundMusic(AudioClip music)
    {
        musicSource.loop = true;
        musicSource.clip = music;
        musicSource.Play();
    }
    public void PlaySfxPlayer(AudioClip sfx)
    {
        sfxSourcePlayer.clip = sfx; 
        sfxSourcePlayer.loop = false;
        sfxSourcePlayer.Play();
    }
    public void PlaySfxEnvironment(AudioClip sfx)
    {
        sfxSourceEnvironment.PlayOneShot(sfx);
    }

    public void StartWalkingOnSand(Transform callerTransform)
    {
        if (!isWalking)
        {
            isWalking = true;
            StartCoroutine(PlayWalkOnSand(callerTransform));
        }
    }

    public void StopWalkingOnSand()
    {
        isWalking = false;
    }

    private IEnumerator PlayWalkOnSand(Transform callerTransform)
    {
        while (isWalking)
        {
            if (walk.Length == 0)
            {
                Debug.LogWarning("No audio clips in the array of Walk AudioClips.");
                yield break;
            }

            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, walk.Length);
            } while (randomIndex == lastPlayedIndex);

            lastPlayedIndex = randomIndex;

           AudioSource.PlayClipAtPoint(walk[randomIndex], callerTransform.position);

            yield return new WaitForSeconds(0.25f);
        }
    }

    public void PlayRandomJumpSound(Transform callerTransform)
    {
        if (jump.Length == 0)
        {
            Debug.LogWarning("No audio clips in the array of Jump AudioClips.");
            return;
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, jump.Length);
        } while (randomIndex == lastJumpIndex);

        lastJumpIndex = randomIndex;
        
        //AudioSource.PlayClipAtPoint(jump[randomIndex], callerTransform.position, 15.0f);
        sfxJump.clip = jump[randomIndex];
        sfxJump.loop = false;
        sfxJump.Play();
    }

    public void PlayPunchSound()
    {
        sfxPunch.clip = punch;
        sfxPunch.loop = false;
        sfxPunch.Play();
    }



    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SfxPlayer", Mathf.Log10(volume) * 20);
        audioMixer.SetFloat("SfxEnvironment", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void playSoundFlare()
    {
        sfxSourcePlayer.clip = flareShot;
        sfxSourcePlayer.loop = false;
        sfxSourcePlayer.Play();
    }

}
