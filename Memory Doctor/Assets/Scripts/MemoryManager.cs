using UnityEngine;
using System.Linq;

// Spawns the memories based on the data from the previous scene
public class MemoryManager : MonoBehaviour
{
    [SerializeField] public MemoryDictionary MemoryCollection;
    public Transform MemoryParent;
    public GameObject MemoryPrefab;

    void Awake () {
        // Temporary, this data will come from the previous scene
        PlayerPrefs.SetString("MemoryStrings", "1,2");
    }
    void Start() {
        string memoryIdString = PlayerPrefs.GetString("MemoryStrings");
        int [] memoryIds = memoryIdString.Split(',').Select(int.Parse).ToArray();
        Vector3 spawnPosition = new Vector3(0,0,0);
        foreach(int memoryId in memoryIds) {
            Debug.Log(memoryId);
            GameObject memory = Instantiate(MemoryPrefab,spawnPosition,MemoryPrefab.transform.rotation, MemoryParent);
            memory.transform.localPosition = spawnPosition;
            memory.GetComponent<MemoryComponent>().Initialize(MemoryCollection[memoryId]);
            // TODO: Fix for vertical rows
            spawnPosition -= new Vector3(1.15f,0,0);
        }
    }
}