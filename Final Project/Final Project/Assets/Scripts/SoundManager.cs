using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource explosionSFX;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource laserSFX;

    [SerializeField] private AudioSource gameMusic;
    [SerializeField] private AudioSource titleMusic;

    private void Awake()
    {
        instance = this;

        gameMusic.Play();
    }

    public void Explosion() => explosionSFX.Play();
    public void Hit() => hitSFX.Play();
    public void Laser() => laserSFX.Play();
}
