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
    public Signal ScreenChangeSignal;
    public Signal SurgeryFinishedSignal;

    HashSet<int> DiscardedPhotos;
    List<int> unpleasantMemories;
    List<int> pleasantMemories;
    List<int> mostUnpleasantMemory;
    List<int> allMemories;

    void Awake () {
        // Temporary, this data will come from the previous scene
        PlayerPrefs.SetString("UnpleasantMemories", "1,2");
        PlayerPrefs.SetString("PleasantMemories", "3,4");
        PlayerPrefs.SetString("MostUnpleasantMemory", "5");
        DiscardedPhotos = new HashSet<int>();
        PhotoDroppedSignal.addListener(OnPhotoTrayChange);
    }
    void Start() {
        SpawnMemories();
    }

    void SpawnMemories () {
        unpleasantMemories = PlayerPrefs.GetString("UnpleasantMemories").Split(',').Select(int.Parse).ToList<int>();
        pleasantMemories = PlayerPrefs.GetString("PleasantMemories").Split(',').Select(int.Parse).ToList<int>();
        mostUnpleasantMemory = PlayerPrefs.GetString("MostUnpleasantMemory").Split(',').Select(int.Parse).ToList<int>();
        allMemories = unpleasantMemories.Union(pleasantMemories).Union(mostUnpleasantMemory).ToList();
        Vector3 spawnPosition = new Vector3(0,0,0);
        foreach(int memoryId in allMemories) {
            GameObject memory = Instantiate(MemoryPrefab,spawnPosition,MemoryPrefab.transform.rotation, MemoryParent);
            memory.transform.localPosition = spawnPosition;
            memory.GetComponent<MemoryComponent>().Initialize(MemoryCollection[memoryId]);
            // TODO: Fix for vertical rows
            spawnPosition -= new Vector3(1.15f,0,0);
        }
        
    }

    public void checkWinCondition () {

        int mostUnpleasantMemoryCount = 0;
        int unpleasantMemoryCount = 0;
        int pleasantMemoryCount = 0;

        int grade = 0;
        int patientRemarkIndex = 0;

        foreach(int trayItem in DiscardedPhotos) {
            Debug.Log(trayItem);
            if(mostUnpleasantMemory.Contains(trayItem)) {
                mostUnpleasantMemoryCount++;
            } else if (unpleasantMemories.Contains(trayItem)) {
                unpleasantMemoryCount++;
            } else if(pleasantMemories.Contains(trayItem)) {
                pleasantMemoryCount++;
            }
        }

        Debug.Log(mostUnpleasantMemoryCount);
        Debug.Log(pleasantMemoryCount);
        Debug.Log(DiscardedPhotos.Count);

        if(mostUnpleasantMemoryCount == mostUnpleasantMemory.Count 
            && pleasantMemoryCount == 0) {
            if(unpleasantMemoryCount == unpleasantMemories.Count) {
                grade = 0; // grade A
            } else if(unpleasantMemoryCount >= 1 && unpleasantMemoryCount < unpleasantMemories.Count) {
                grade = 1; // grade B
                patientRemarkIndex = 1;
            } else if (unpleasantMemoryCount == 0) {
                grade = 2; // grade C
                patientRemarkIndex = 2;
            }
        } else if (mostUnpleasantMemoryCount == 0) {
            if(DiscardedPhotos.Count == pleasantMemoryCount) {
                grade = 3; // grade d
                patientRemarkIndex = 5;
            } else if (pleasantMemoryCount < DiscardedPhotos.Count) {
                grade = 2; // grade c
                patientRemarkIndex = 3;
            }
        } else if (DiscardedPhotos.Count == 0) {
            grade = 3; // grade d
            patientRemarkIndex = 4;
        } else {
            grade = 3; // grade d
            patientRemarkIndex = 4;
        }

        SignalData resultData = new SignalData();
        resultData.set("Grade", grade);
        resultData.set("PatientRemarkIndex", patientRemarkIndex);
        SurgeryFinishedSignal.fire(resultData);

        SignalData newScreenIndex = new SignalData();
        newScreenIndex.set("ScreenIndex", 3);
        ScreenChangeSignal.fire(newScreenIndex);
    }

    void OnPhotoTrayChange (SignalData data) {
        bool AddedOrRemoved = data.get<bool>("AddedOrRemoved");
        int memoryId = data.get<int>("MemoryId");
        if(AddedOrRemoved) {
            DiscardedPhotos.Add(memoryId);
            Debug.Log("Memory "+memoryId.ToString() + " Added to tray");
        } else {
            DiscardedPhotos.Remove(memoryId);
            Debug.Log("Memory "+memoryId.ToString() + " removed from tray");
        }
    }
}