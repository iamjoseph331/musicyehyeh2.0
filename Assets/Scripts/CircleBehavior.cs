using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class CircleBehavior : MonoBehaviour
{
    public int id;
    public float lifeBorn;
    public bool keyup = false;
    private float noteLeave;
    public float lifetime = 1f;

    private void Start()
    {
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        lifeBorn = Time.time;
        foreach (var circle in GameObject.FindGameObjectsWithTag("Circle"))
        {
            if (lifeBorn > circle.GetComponent<CircleBehavior>().lifeBorn &&
                circle.GetComponent<CircleBehavior>().id == id && !circle.GetComponent<CircleBehavior>().keyup)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (keyup)
        {
            if (Time.time > noteLeave + lifetime)
            {
                Destroy(gameObject);
            }
        }
        if (!keyup && MidiMaster.GetKey(id) == 0f)
        {
            keyup = true;
            noteLeave = Time.time;
        }
    }
}
