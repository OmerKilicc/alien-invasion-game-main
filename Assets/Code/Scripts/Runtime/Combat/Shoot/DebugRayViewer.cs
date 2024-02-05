using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRayViewer : MonoBehaviour
{
    [SerializeField] float weaponRange = 50f;              // Distance in Unity units over which the Debug.DrawRay will be drawn
    [SerializeField] Camera fpsCam;                                // Holds a reference to the first person camera

    void Update()
    {
        // Create a vector at the center of our camera's viewport
        Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // Draw a line in the Scene View  from the point lineOrigin in the direction of _fpsCam.transform.forward * _weaponRange, using the color green
        Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);
    }
}
