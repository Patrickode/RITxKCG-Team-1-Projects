using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraySensor : MonoBehaviour {
    public Signal PhotoDroppedSignal;
    public int TrayId;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.gameObject.layer == LayerMask.NameToLayer ("Interactive")) {
            MemoryComponent memory = other.gameObject.GetComponent<MemoryComponent>();
            if(memory != null) {
                // Debug.Log(memory.MemoryInformation.memoryId);
                SignalData data = new SignalData();
                data.set("AddedOrRemoved", true);
                data.set("MemoryId", memory.MemoryInformation.memoryId);
                PhotoDroppedSignal.fire(data);
            }
        }
    }

        private void OnTriggerExit(Collider other) {
        if(other.gameObject.gameObject.layer == LayerMask.NameToLayer ("Interactive")) {
            MemoryComponent memory = other.gameObject.GetComponent<MemoryComponent>();
            if(memory != null) {
                // Debug.Log(memory.MemoryInformation.memoryId);
                SignalData data = new SignalData();
                data.set("AddedOrRemoved", false);
                data.set("MemoryId", memory.MemoryInformation.memoryId);
                PhotoDroppedSignal.fire(data);
            }
        }
    }
}