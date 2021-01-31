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

    List<HashSet<int>> TrayPhotos;
    List<int> unpleasantMemories;
    List<int> pleasantMemories;
    List<int> mostUnpleasantMemory;
    List<int> allMemories;

    void Awake () {
        // Temporary, this data will come from the previous scene
        PlayerPrefs.SetString("UnpleasantMemories", "1,2");
        PlayerPrefs.SetString("PleasantMemories", "3,4");
        PlayerPrefs.SetString("MostUnpleasantMemory", "5");
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

        string grade = "A";
        int patientRemarkIndex = 0;

        foreach(int trayItem in TrayPhotos[1]) {
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
        Debug.Log(TrayPhotos[1].Count);

        if(mostUnpleasantMemoryCount == mostUnpleasantMemory.Count 
            && pleasantMemoryCount == 0) {
            if(unpleasantMemoryCount == unpleasantMemories.Count) {
                grade = "A"; // grade A
            } else if(unpleasantMemoryCount >= 1 && unpleasantMemoryCount < unpleasantMemories.Count) {
                grade = "B"; // grade B
                patientRemarkIndex = 1;
            } else if (unpleasantMemoryCount == 0) {
                grade = "C"; // grade C
                patientRemarkIndex = 2;
            }
        } else if (mostUnpleasantMemoryCount == 0) {
            if(TrayPhotos[1].Count == pleasantMemoryCount) {
                grade = "D"; // grade d
                patientRemarkIndex = 5;
            } else if (pleasantMemoryCount < TrayPhotos[1].Count) {
                grade = "C"; // grade c
                patientRemarkIndex = 3;
            }
        } else if (TrayPhotos[1].Count == 0) {
            grade = "D"; // grade d
            patientRemarkIndex = 4;
        } else {
            grade = "D"; // grade d
            patientRemarkIndex = 4;
        }

        SignalData resultData = new SignalData();
        resultData.set("Grade", grade);
        resultData.set("PatientRemarkIndex", patientRemarkIndex);
        SurgeryFinishedSignal.fire(resultData);

        SignalData newScreenIndex = new SignalData();
        newScreenIndex.set("ScreenIndex", 2);
        ScreenChangeSignal.fire(newScreenIndex);
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