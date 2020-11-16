using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class HolderBehavior : MonoBehaviour
{
    public int id;
    public float lifeBorn;
    public bool keyup = false;
    private float noteLeave;
    public float lifetime = 10f;
    // Start is called before the first frame update
    void Start()
    {
        lifeBorn = Time.time;
        foreach(GameObject holder in GameObject.FindGameObjectsWithTag("Holder"))
        {
            if(MidiMaster.GetKey(id) > 0 &&
                holder.GetComponent<HolderBehavior>().id == id && holder.GetComponent<HolderBehavior>().lifeBorn < lifeBorn)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
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
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
