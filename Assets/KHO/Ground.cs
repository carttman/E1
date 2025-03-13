using System;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    private Texture2D gridTexture;
    
    private void Awake()
    {
        Debug.Assert(gridTexture);
        Material m = GetComponent<MeshRenderer>().material;
        Vector2 size = new Vector2(transform.localScale.x, transform.localScale.y);
        m.SetTexture("_BaseMap", gridTexture);
        m.SetTextureScale("_BaseMap", size);
    }
    
    
}
