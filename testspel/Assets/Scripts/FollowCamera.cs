using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform backgroundPos;
 void Update () {
     backgroundPos.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
 }
}
