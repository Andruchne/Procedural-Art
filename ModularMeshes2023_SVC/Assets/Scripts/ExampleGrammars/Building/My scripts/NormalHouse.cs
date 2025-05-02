using Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHouse : Shape
{
    // grammar rule probabilities:
    const float secondFloorChance = 0.5f;

    // shape parameters:
    [SerializeField]
    int Width;
    int Depth = 2;

    [SerializeField]
    GameObject[] stoneBase;
    [SerializeField]
    GameObject[] woodWalls;
    [SerializeField]
    GameObject[] stoneWalls;
    [SerializeField]
    GameObject[] roofStyle;

    int number;

    public void Initialize(int number, int Width, int Depth, GameObject[] stoneBase, GameObject[] woodWalls, GameObject[] stoneWalls, GameObject[] roofStyle)
    {
        this.number = number;
        this.Width = Width;
        this.Depth = Depth;
        this.stoneBase = stoneBase;
        this.woodWalls = woodWalls;
        this.stoneWalls = stoneWalls;
        this.roofStyle = roofStyle;
    }

    protected override void Execute()
    {
        if (number == 0) GenerateGround();
        else
        {
            GenerateWalls(woodWalls);
            GenerateRoof();
        }
    }

    void GenerateGround()
    {
        // Generate stone foundation...
        GenerateFoundation();

        float randomValue = RandomFloat();
        if (randomValue < secondFloorChance)
        {
            // Generate stone walls...
            GenerateWalls(stoneWalls);
            GenerateFloor(true);
        }
        else
        {
            // Generate wooden walls...
            GenerateWalls(woodWalls);
            GenerateRoof();
        }
    }

    void GenerateWalls(GameObject[] wallType)
    {
        // Create four walls:
        for (int i = 0; i < 4; i++)
        {
            Vector3 localPosition = new Vector3();
            switch (i)
            {
                case 0:
                    localPosition = new Vector3(-(Width - 1) * 0.5f, 0, 0); // left
                    break;
                case 1:
                    localPosition = new Vector3(0, 0, (Depth - 1) * 0.5f); // back
                    break;
                case 2:
                    localPosition = new Vector3((Width - 1) * 0.5f, 0, 0); // right
                    break;
                case 3:
                    localPosition = new Vector3(0, 0, -(Depth - 1) * 0.5f); // front
                    break;
            }
            SimpleRow newRow = CreateSymbol<SimpleRow>("wall", localPosition, Quaternion.Euler(0, i * 90, 0));
            newRow.Initialize(i % 2 == 1 ? Width : Depth, wallType);
            newRow.Generate();
        }
    }

    void GenerateRoof()
    {
        NormalRoof nextRoof = CreateSymbol<NormalRoof>("roof", new Vector3(0, 1, 0));
        nextRoof.Initialize(Width, Depth, roofStyle);
        nextRoof.Generate(buildDelay);
    }

    void GenerateFloor(bool increaseNumber = false)
    {
        // To allow proper number tracking in editor time...
        int n = number;
        if (increaseNumber) n++;

        NormalHouse nextStock = CreateSymbol<NormalHouse>("stock", new Vector3(0, 1, 0));
        nextStock.Initialize(n, Width, Depth, stoneBase, woodWalls, stoneWalls, roofStyle);
        nextStock.Generate(buildDelay);
    }

    void GenerateFoundation()
    {
        Ground ground = CreateSymbol<Ground>("stock", new Vector3(0, -1, 0));
        ground.Initialize(Width, Depth, stoneBase);
        ground.Generate(buildDelay);
    }
}