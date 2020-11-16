using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class TorusBehavior : MonoBehaviour
{
    public int id;
    public float lifeBorn;
    public bool keyup = false;
    private float noteLeave;
    public float lifetime = 1f;

    private void Start()
    {
        lifeBorn = Time.time;
        foreach (var donut in GameObject.FindGameObjectsWithTag("Torus"))
        {
            if(lifeBorn > donut.GetComponent<TorusBehavior>().lifeBorn && 
                donut.GetComponent<TorusBehavior>().id == id && !donut.GetComponent<TorusBehavior>().keyup)
            {
                Destroy(gameObject);
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(keyup)
        {
            if(Time.time > noteLeave + lifetime)
            {
                Destroy(gameObject);
            }
        }
        if(!keyup &&　MidiMaster.GetKey(id) == 0f)
        {
            keyup = true;
            noteLeave = Time.time;
        }
        
    }
}
