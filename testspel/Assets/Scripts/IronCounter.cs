using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IronCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public int IronCount;
public Text text;
    void Start()
    {
        IronCount = 21;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = IronCount.ToString();
    }
}
