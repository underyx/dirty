using UnityEngine;
using System.Collections;

public class DropTargetDoor : DropTarget {
    void Start () {

    }

    void Update () {

    }

    public override void Drop(Collider dragObject) {
        Debug.Log("key dropped on door!");
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
