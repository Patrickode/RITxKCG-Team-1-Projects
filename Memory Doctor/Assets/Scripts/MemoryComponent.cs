using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holder object to link a gameObject to memory information
// allows easy retrieval of memory data on interaction
public class MemoryComponent : MonoBehaviour
{
    [HideInInspector] public MemoryInformation MemoryInformation;
    public Renderer meshRenderer;

    // Initialize a memory prefab with the texture and text. The id will be used to determine
    // whether this should have been removed or not
    public void Initialize (MemoryInformation memoryInformation) {
        this.MemoryInformation = memoryInformation;
        meshRenderer.material.SetTexture("_MainTex", MemoryInformation.photoTexture);
    }
}
