using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class SingleNoteBehavior : MonoBehaviour
{
    public GameObject notePrefab;
    public static float speed = 1f;

    [Header("Color Settings")]
    public Color[] noteColors;

    private static float ScrHeight, ScrWidth;
    
    void Start()
    {
        ScrHeight = Camera.main.orthographicSize * 2f;
        ScrWidth = ScrHeight * Screen.width / Screen.height;
        ScrWidth -= 1f;
    }

    float Scale2Screenspace(float min, float max, float target)
    {
        return ((target - min) * ScrHeight / (max - min)) - Camera.main.orthographicSize;
    }

    public static float CurrentXaxis()
    {
        return ((Time.time / speed) % (int)(ScrWidth + 1)) - ScrWidth / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        int min = 21, max = 108;
        for(int i = min; i <= max; ++i)
        {
            if (MidiMaster.GetKeyDown(i))
            {
                Vector3 _notePosition = new Vector3(CurrentXaxis(), Scale2Screenspace(min, max, i), -CurrentXaxis() / 100f);
                Transform _transform = Instantiate(notePrefab.transform, _notePosition, Quaternion.identity);
                _transform.localScale *= (MidiMaster.GetKey(i) * MidiMaster.GetKey(i) * 2.2f);
                print(MidiMaster.GetKey(i));
                _transform.GetComponent<DeleteNote>().id = i;
                for(int j = 0; j < _transform.childCount; ++j)
                {
                    _transform.GetChild(j).GetComponent<Renderer>().material.color = noteColors[i % 12];
                }
            }
            
        }

    }
}
