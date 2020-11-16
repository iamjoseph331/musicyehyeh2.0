using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class ParticleBehavior : MonoBehaviour
{
    public int id;
    public float lifeBorn;
    private float keyleave = 0f;
    public bool keyup = false;
    public float lifetime = 1f;

    private void Start()
    {
        lifeBorn = Time.time;
        foreach (var particles in GameObject.FindGameObjectsWithTag("Particle"))
        {
            if (lifeBorn > particles.GetComponent<ParticleBehavior>().lifeBorn &&
                particles.GetComponent<ParticleBehavior>().id == id && !particles.GetComponent<ParticleBehavior>().keyup)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (keyup && Time.time - keyleave > lifetime)
        {
            Destroy(gameObject);         
        }
        if (!keyup && MidiMaster.GetKey(id) == 0f)
        {
            keyup = true;
            keyleave = Time.time;
        }
    }
}
