using UnityEngine;
using System.Collections;

public class droptarget : DropTarget {
    void Start () {

    }

    void Update () {

    }

    public void Drop(Collider dragObject) {
        if (dragObject.GetComponent<Draggable>().dropType == "key")
        {
            Debug.Log("key dropped on door!");
        }
        if (dragObject.GetComponent<Draggable>().dropType == "box")
        {
            Debug.Log("box dropped on door!");
        }
        Debug.Log(dragObject.name);
    }
}
