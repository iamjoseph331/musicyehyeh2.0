using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MidiJack;

public class KeyActor : MonoBehaviour
{
    public Transform canvas;
    public GameObject minorGradient;
    private Transform currentColor;

    [Header("Color For Minor")]
    public Color minor;
    private bool major = true;

    private void Start()
    {
        currentColor = canvas.GetChild(1);
    }

    public void OnChooseKey(int key)
    {
        //canvas.gameObject.SetActive(false);
        canvas.GetChild(key * 2 + 1).gameObject.SetActive(true);
        currentColor = canvas.GetChild(key * 2 + 1);
        for(int i = 0; i < 13; ++i)
        {
            canvas.GetChild(i * 2).gameObject.SetActive(false);
        }
    }

    public void OnChooseBlack()
    {
        for(int i = 0; i < canvas.childCount; ++i)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetMajor()
    {
        major = transform.GetComponentInChildren<Dropdown>().value == 0;
    }

    private int []switchCount = new int[12];
    private float lastswitched = 0, last = 0;
    private int switchMaj = 0, switchMin = 0, switchNull = 0;

    private void Update()
    {
        for(int i = 24; i < 36; ++i)
        {
            if(MidiMaster.GetKeyDown(i))
            {
                if(Time.time - lastswitched > 1 || i != last)
                {
                    switchCount[i - 24] = 0;
                }
                switchCount[i - 24] += 1;
                if(switchCount[i - 24] >= 5)
                {
                    currentColor.gameObject.SetActive(false);
                    currentColor = canvas.GetChild(2 * (i - 24) + 1);
                    currentColor.gameObject.SetActive(true);
                }
                lastswitched = Time.time;
                last = i;
            }
        }
        if (MidiMaster.GetKeyDown(21))
        {
            if (Time.time - lastswitched > 1 || 21 != last)
            {
                switchMaj = 0;
            }
            switchMaj += 1;
            if (switchMaj >= 5)
            {
                minorGradient.SetActive(false);
            }
            lastswitched = Time.time;
            last = 21;
        }
        else if (MidiMaster.GetKeyDown(22))
        {
            if (Time.time - lastswitched > 1 || 22 != last)
            {
                switchMin = 0;
            }
            switchMin += 1;
            if (switchMin >= 5)
            {
                minorGradient.SetActive(true);
            }
            lastswitched = Time.time;
            last = 22;
        }
        else if (MidiMaster.GetKeyDown(108))
        {
            if (Time.time - lastswitched > 1 || 108 != last)
            {
                switchNull = 0;
            }
            switchNull += 1;
            if (switchNull >= 5)
            {
                currentColor.gameObject.SetActive(false);
            }
            lastswitched = Time.time;
            last = 108;
        }
    }
}
