using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    private AudioSource[] mysounds;
    private AudioSource movement;
    private AudioSource transform;
    private AudioSource straight;
    private AudioSource die;
    // Start is called before the first frame update
    void Start()
    {
        mysounds = GetComponents<AudioSource>();
        movement = mysounds[0];
        transform = mysounds[1];
        straight = mysounds[2];
        die = mysounds[3];
    }

    public void PlayMove()
    {
        movement.Play();
    }
    public void PlayDie()
    {
        die.Play();
    }
    public void Playstraight()
    {
        straight.Play();
    }
    public void Playtransform()
    {
        transform.Play();
    }

}
