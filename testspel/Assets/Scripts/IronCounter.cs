using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IronCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public static int IronCount;
    public Text text;
    GameObject endMenu;
    void Start()
    {
        endMenu = GameObject.Find("EndMenu");
        endMenu.SetActive(false);
        IronCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = IronCount.ToString();
        if (IronCount >= 100) endMenu.SetActive(true);
    }
}
