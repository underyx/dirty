using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DropTarget : MonoBehaviour
{
    public string dropType = "key";

    public virtual void Drop(Collider dragObject) {}
    
}
