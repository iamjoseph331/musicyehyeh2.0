using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class DeleteNote : MonoBehaviour
{
    private float NoteBorn;
    private float NoteLeave;
    private bool flag = false;
    public int id;

    public float transfer;
    public float lifetime;

    private float deltaAlpha;

    private void Start()
    {
        NoteBorn = Time.time;
        transform.position += new Vector3(0, 0, 10);
    }
    
    void Update()
    {
        if (!flag && MidiMaster.GetKey(id) == 0f)
        {
            flag = true;
            NoteLeave = Time.time;
            deltaAlpha = transform.GetComponentInChildren<Renderer>().material.color.a / lifetime;
        }

        if (!flag && MidiMaster.GetKey(id) > 0f)
        {
            if (Time.time < NoteBorn + transfer)
            {
                transform.GetChild(2).position = transform.GetChild(0).position;
                transform.GetChild(1).position = transform.GetChild(0).position - new Vector3(0, 0, 0.0001f);
                Vector3 _oriscale = transform.GetChild(1).localScale;
                transform.GetChild(1).localScale = new Vector3(_oriscale.x, 0f, _oriscale.z);
            }
            else
            {
                if((SingleNoteBehavior.CurrentXaxis() - 0.5f) > transform.GetChild(0).position.x)
                {
                    transform.GetChild(2).position =
                        new Vector3((SingleNoteBehavior.CurrentXaxis() - 0.5f), transform.position.y, transform.position.z);
                }
                transform.GetChild(1).position = (transform.GetChild(0).position + transform.GetChild(2).position) / 2 - new Vector3(0, 0, 0.0001f);
                Vector3 _oriscale = transform.GetChild(1).localScale;
                transform.GetChild(1).localScale = new Vector3(_oriscale.x, (transform.GetChild(2).position.x - transform.GetChild(0).position.x) / (2 * transform.localScale.y), _oriscale.z);
            }
        }
        
        if(flag && Time.time > NoteLeave + lifetime)
        {
            Destroy(gameObject);
        }
        var colors = transform.GetComponentsInChildren<Renderer>();
        foreach(Renderer ren in colors)
        {
            ren.material.color = new Vector4( ren.material.color.r, ren.material.color.g, 
                                              ren.material.color.b, ren.material.color.a - Time.deltaTime * deltaAlpha);
        }
    }
}
