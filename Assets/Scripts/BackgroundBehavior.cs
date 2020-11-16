using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
    void Start()
    {
        float _height = Camera.main.orthographicSize * 2.0f;
        float _width = _height * Screen.width / Screen.height;
        transform.localScale = new Vector3(_width, _height, 1);
    }

    public Transform canvas;
    public bool BGwhite = false;

    public void SetWhite()
    {
        BGwhite = true;
    }

    private void Update()
    {
        for(int i = 0; i < canvas.childCount; ++i)
        {
            if(canvas.GetChild(i).gameObject.activeSelf == true)
            {
                transform.GetComponent<Renderer>().material.color = Color.white;
                break;
            }
            if(i == canvas.childCount - 1)
            {
                transform.GetComponent<Renderer>().material.color = BGwhite ? Color.white : Color.black;
            }
        }
    }

}
