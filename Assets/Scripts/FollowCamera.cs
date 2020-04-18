using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public GameObject player;

    public Vector3 offset = new Vector3(0, 0, -10);



    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, playerPos, 5f * Time.deltaTime);

        //Vector3 newCameraPos =  player.transform.position + offset;
        //transform.position = newCameraPos;
    }
}
