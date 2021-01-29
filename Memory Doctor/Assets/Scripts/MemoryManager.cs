using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// Spawns the memories based on the data from the previous scene
public class MemoryManager : MonoBehaviour
{
    [SerializeField] public MemoryDictionary MemoryCollection;
    public Transform MemoryParent;
    public GameObject MemoryPrefab;
    public Signal PhotoDroppedSignal;

    List<HashSet<int>> TrayPhotos;

    void Awake () {
        // Temporary, this data will come from the previous scene
        PlayerPrefs.SetString("MemoryStrings", "1,2,3");
        TrayPhotos = new List<HashSet<int>> {
            new HashSet<int>(), // The first and the main memory tray
            new HashSet<int>() // Second or the removed list
        };
        PhotoDroppedSignal.addListener(OnPhotoTrayChange);
    }
    void Start() {
        SpawnMemories();
    }

    void SpawnMemories () {
        string memoryIdString = PlayerPrefs.GetString("MemoryStrings");
        int [] memoryIds = memoryIdString.Split(',').Select(int.Parse).ToArray();
        Vector3 spawnPosition = new Vector3(0,0,0);
        foreach(int memoryId in memoryIds) {
            GameObject memory = Instantiate(MemoryPrefab,spawnPosition,MemoryPrefab.transform.rotation, MemoryParent);
            memory.transform.localPosition = spawnPosition;
            memory.GetComponent<MemoryComponent>().Initialize(MemoryCollection[memoryId]);
            // TODO: Fix for vertical rows
            spawnPosition -= new Vector3(1.15f,0,0);
        }
    }

    void OnPhotoTrayChange (SignalData data) {
        int trayId = data.get<int>("TrayId");
        int memoryId = data.get<int>("MemoryId");
        int otherTrayId = 1 - trayId;
        TrayPhotos[trayId].Add(memoryId);
        TrayPhotos[otherTrayId].Remove(memoryId);
        Debug.Log("Removed photo " +memoryId + " from tray: "+otherTrayId + " and placed in tray: " +trayId);

    }
}