using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Memory", menuName = "ScriptableObjects/Memory", order = 1)]
public class MemoryInformation : ScriptableObject {
    public int memoryId;
    [SerializeField] public EnJpString memoryText;
    public Texture2D photoTexture;
}