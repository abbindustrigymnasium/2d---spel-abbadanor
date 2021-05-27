using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public Vector3 throwLocation;
    public Vector3 targetLocation;
    public GameObject explosion;

    public float explosionTime;

    float timeToLive;
    ModifyTerrain modifyTerrain;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject ground = GameObject.FindGameObjectWithTag("ground");
        modifyTerrain = ground.GetComponent<ModifyTerrain>();
        timeToLive = explosionTime;
        targetLocation += new Vector3(0.5f, 0.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToLive > 0)
        {
            timeToLive -= Time.deltaTime;
        }
        else
        {
            modifyTerrain.Explode(targetLocation, 2);
            Instantiate(explosion, targetLocation, Quaternion.identity);
            Destroy(gameObject);
        }

        float t = Mathf.Clamp(timeToLive / explosionTime, 0, 1);
        Vector3 p = Vector3.Lerp(targetLocation, throwLocation, t);
        //Debug.Log("t: " + t + "; " + "p: " + p);
        transform.position = p;
    }
}
