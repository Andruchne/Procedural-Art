using Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Shape
{
    int Width;
    int Depth;
    GameObject[] prefabs = null;

    public void Initialize(int Width, int Depth, GameObject[] prefabs, Vector3 center = new Vector3())
    {
        this.Width = Width;
        this.Depth = Depth;
        this.prefabs = prefabs;
    }

    protected override void Execute()
    {
        if (Width <= 0 || Depth <= 0) return;

        // spawn the prefabs, randomly chosen
        int index = RandomInt(prefabs.Length); // choose a random prefab index

        

        GameObject ground = SpawnPrefab(prefabs[index]);
        ground.transform.localScale = new Vector3(Width, 2, Depth);
    }
}

