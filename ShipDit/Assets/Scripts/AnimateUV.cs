using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUV : MonoBehaviour
{
    public float scrollSpeed=.5f;
    public bool U = false;
    public bool V = false;

    Material material;
    float offset;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset = Time.time * scrollSpeed % 1;
        if (U && V)
        {
            material.mainTextureOffset=new Vector2 (offset, offset);
        }
    }
}
