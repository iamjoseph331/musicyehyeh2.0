using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMist : MonoBehaviour
{
    void Start()
    {
        float _height = Camera.main.orthographicSize * 2.0f;
        float _width = _height * Screen.width / Screen.height;
        transform.localScale = new Vector3(_width, _height, 1);
    }
}
