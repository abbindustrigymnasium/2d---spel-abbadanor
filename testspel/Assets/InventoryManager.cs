using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject slotObject1;
    public GameObject slotObject2;
    public GameObject slotObject3;

    public bool drillAvailable = true;
    public bool dynamiteAvailable = false;
    Button button1;
    Button button2;
    Button button3;

    ColorBlock color1;
    ColorBlock color2;
    ColorBlock color3;

    public enum Item {
        PICKAXE,
        DRILL,
        DYNAMITE
    }
    public Item itemSelected = Item.PICKAXE;
    void Start()
    {
        
    }

    void Awake() {
        button1 = slotObject1.GetComponent<Button>();
        button2 = slotObject2.GetComponent<Button>();
        button3 = slotObject3.GetComponent<Button>();

        color1 = slotObject1.GetComponent<Button>().colors;
        color2 = slotObject2.GetComponent<Button>().colors;
        color3 = slotObject3.GetComponent<Button>().colors;
    }

    // Update is called once per frame
    void Update()
    {
        if(itemSelected == Item.DRILL) {
            color2.normalColor = Color.blue;
            color1.normalColor = Color.white;
            color3.normalColor = dynamiteAvailable ? Color.white : Color.gray;
        } else if (itemSelected == Item.DYNAMITE) {
            color3.normalColor = Color.blue;
            color1.normalColor = Color.white;
            color2.normalColor = Color.white;
        } else {
            color1.normalColor = Color.blue;
            color2.normalColor = drillAvailable ? Color.white : Color.gray;
            color3.normalColor = dynamiteAvailable ? Color.white : Color.gray;
        }
        button1.colors = color1;
        button2.colors = color2;
        button3.colors = color3;
    }
}
