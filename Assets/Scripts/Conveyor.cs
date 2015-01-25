using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class SpawnInfo
{
    public Draggable Item;
    public float Chance;
}

public class Conveyor : MonoBehaviour
{
    public List<SpawnInfo> Spawnables;
    private System.Random random;
    private Transform convTrans;
    private Bounds meshBounds;
    private float agg;
    
    public int ItemCount = 3;

    public void Start()
    {
        childDrag = new List<Draggable>(64);
        random = new Random();
        convTrans = transform.FindChild("Mesh");
        meshBounds = convTrans.GetComponent<MeshFilter>().mesh.bounds;
    }

    public void CalculateChances()
    {
        agg = 0;
        foreach (var spawnable in Spawnables)
        {
            if (spawnable.Item == null)
                continue;
            agg += spawnable.Chance;
        }

    }

    public SpawnInfo GetNextItem()
    {
        float chance = (float) (Math.Abs(random.NextDouble())%agg);
        float cnt = 0;
        for (int i = 0; i < Spawnables.Count; i++)
        {
            cnt += Spawnables[i].Chance;
            if (chance < cnt)
                return Spawnables[i];
        }
        return Spawnables[Spawnables.Count - 1];
    }

    private List<Draggable> childDrag;

 //   private List<float> _targetPos;
    public float ConveyorSpeed;

    public float ItemOffset=3;
    public Vector3 SpawnPos;

    public float GetTargetPos(int i)
    {
        var pos = meshBounds.min.x * convTrans.lossyScale.x+0.6f;
      //  pos.y += meshBounds.extents.y;
        pos +=  ItemOffset*i;
        return pos;
    }

    private void Update()
    {
        SpawnNewItems();
        for (int i = 0; i < childDrag.Count; i++)
        {
            float targetPos = GetTargetPos(i);
            var drag = childDrag[i];
            if (!drag.BeingDragged)
            {
                float spd = ConveyorSpeed*Time.deltaTime;
                Vector3 position = drag.transform.localPosition;
                position.x -= spd;
                position.z = -0.03f;
                position.y = 0;
                if (position.x< targetPos)
                {
                    position.x = targetPos;
                }
                drag.transform.localPosition = position;
            }
        }      
        
    }

    public void SpawnNewItems()
    {
        int count = ItemCount - childDrag.Count;
        Vector3 spawnPos = SpawnPos;
        for (int i = 0; i < count; i++)
        {
            SpawnInfo info = GetNextItem();
            if (info.Item == null)
                continue;
            Draggable go = (Draggable)GameObject.Instantiate(info.Item);

            go.transform.parent = transform;
            go.transform.localPosition = spawnPos + Vector3.back*0.03f;
            go.transform.localRotation = Quaternion.identity;
            spawnPos.x += ItemOffset;
            childDrag.Add(go);
        }
        
    }

    public void ItemRemove(Draggable item)
    {
        childDrag.Remove(item);
    }
}

