using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 0.5f * Time.time);
    }
}
