using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private AudioSource _audioSource;

    public AudioClip BrickSound;
    public AudioClip WallSound;
    public AudioClip PaddleSound;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        string otherTag = other.gameObject.tag;
        switch(otherTag)
        {
        case "Wall":
            _audioSource.PlayOneShot(WallSound);
            break;

        case "Brick":
            _audioSource.PlayOneShot(BrickSound);
            break;

        case "Paddle":
            _audioSource.PlayOneShot(PaddleSound);
            break;

        default:
            break;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var velocity = m_Rigidbody.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }

        // NOTE: using a squashed-cylinder mesh-collider on the paddle to encourage non-vertical rebound vectors

        //max velocity
        if (velocity.magnitude > 3.0f)
        {
            velocity = velocity.normalized * 3.0f;
        }

        m_Rigidbody.velocity = velocity;
    }
}
