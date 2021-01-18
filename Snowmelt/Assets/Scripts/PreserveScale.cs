using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveScale : MonoBehaviour
{
    //I couldn't remember how to counteract the parent's localScale, thanks to 
    //https://stackoverflow.com/a/52430453 for reminding me
    //私は私を思い出させるために https://stackoverflow.com/a/52430453 に、感謝を親のlocalScale
    //に対抗する方法を覚えていませんでした

    private Vector3 originalScale;
    private Vector3 parentOriginalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        parentOriginalScale = transform.parent.localScale;
    }

    private void LateUpdate()
    {
        Vector3 currentParentScale = transform.parent.localScale;

        // Get the relative difference to the original scale
        float diffX = currentParentScale.x / parentOriginalScale.x;
        float diffY = currentParentScale.y / parentOriginalScale.y;
        float diffZ = currentParentScale.z / parentOriginalScale.z;

        // This inverts the scale differences
        var diffVector = new Vector3(1 / diffX, 1 / diffY, 1 / diffZ);

        // Apply the inverted differences to the original scale
        transform.localScale = Vector3.Scale(originalScale, diffVector);
    }
}
