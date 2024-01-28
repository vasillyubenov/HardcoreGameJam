using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipLightAttack;
    [SerializeField] private AudioClip audioClipHeavyAttack;
    [SerializeField] private AudioClip audioClipSpecialAttack;
    [SerializeField] private AudioClip audioClipHurt;
    [SerializeField] private AudioClip audioClipDeath;

    [Header("Looping sources")]
    [SerializeField] private AudioSource audioSourceJump;
    [SerializeField] private AudioSource audioSourceDash;

    private static SoundManager instance;
    private AudioSource audioSource;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject soundManagerObject = new GameObject("SoundManager");
                    instance = soundManagerObject.AddComponent<SoundManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump()
    {
        audioSourceJump.PlayOneShot(audioSourceJump.clip);

    }

    public void PlayDash()
    {
        audioSourceDash.PlayOneShot(audioSourceDash.clip);
    }

    public void PlayMove(AudioSource walkingAudio)
    {
        if (!walkingAudio.isPlaying)
        {
            walkingAudio.Play();
        }
    }

    public void StopMove(AudioSource walkingAudio)
    {
        walkingAudio.Stop();
    }

    public void PlayLightAttack()
    {
        audioSource.PlayOneShot(audioClipLightAttack);
    }

    public void PlayHeavyAttack()
    {
        audioSource.PlayOneShot(audioClipHeavyAttack);
    }

    public void PlaySpecialAttack()
    {
        audioSource.PlayOneShot(audioClipSpecialAttack);
    }

    public void PlayHurt()
    {
        audioSource.PlayOneShot(audioClipHurt);
    }

    public void PlayDeath()
    {
        audioSource.PlayOneShot(audioClipDeath);
    }
}
