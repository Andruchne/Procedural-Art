using Demo;
using UnityEngine;

public class MiddleLargeHouse : Shape
{
    public int buildLength = -1;

    public float heightPerBlock = 1;

    public int maxLength = 5;
    public int minLength = 3;

    public int doorPlacementIndex = -1;

    public BuildingBlockCollection blockCollection;

    float halfedLength = -1;
    int currentStage = 0;

    public void Initialize(int pBuildLength, int pMinLength, int pMaxLength, int pCurrentStage, int pDoorIndex, BuildingBlockCollection pBlockCollection)
    {
        buildLength = pBuildLength;
        minLength = pMinLength;
        maxLength = pMaxLength;
        blockCollection = pBlockCollection;
        currentStage = pCurrentStage;
        doorPlacementIndex = pDoorIndex;
    }

    protected override void Execute()
    {
        if (buildLength < 0) { buildLength = RandomInt(minLength, maxLength + 1); }
        else { buildLength = Mathf.Clamp(buildLength, minLength, maxLength); }

        if (doorPlacementIndex == -1 || doorPlacementIndex < 0 || doorPlacementIndex >= buildLength)
        { doorPlacementIndex = RandomInt(0, buildLength); }

        halfedLength = ((float)buildLength + 6.0f) / 2.0f;

        float centerMargin = 1.5f;
        int adjBuildLength = buildLength + 6;

        switch (currentStage)
        {
            case 0:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if (((i == 1 || i == 2) && a > 0 && a < adjBuildLength - 1) ||
                                ((i == 0 || i == 3) && a > 1 && a < 4))
                            {
                                SpawnPrefab(blockCollection.stoneGround,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                            }
                            else
                            {
                                GameObject ground;
                                PillaredGround pillarGround;
                                if (i == 0)
                                {
                                    if (a > 0 && a < adjBuildLength - 1)
                                    {
                                        ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                        pillarGround = ground.GetComponent<PillaredGround>();
                                        pillarGround.SetCorner(false, false);
                                        pillarGround.DeactivateLeftPillar();

                                        // Stairs
                                        if (a == doorPlacementIndex + 5) 
                                        {
                                            SpawnPrefab(blockCollection.stairs,
                                            new Vector3(centerMargin + 2, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                        }
                                    }
                                    else
                                    {
                                        float yRotation = 0;
                                        if (a == adjBuildLength - 1) { yRotation = 270; }
                                        ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0 + yRotation, 0));
                                        pillarGround = ground.GetComponent<PillaredGround>();
                                        if (a == 0) { pillarGround.DeactivateLeftPillar(); }
                                        else 
                                        { 
                                            pillarGround.DeactivateRightPillar();

                                            // Stairs
                                            SpawnPrefab(blockCollection.stairs,
                                                new Vector3(centerMargin + 1, 0, currentPos + 1), Quaternion.Euler(0, 270, 0));
                                        }
                                    }

                                    // Pillars
                                    if (a > 3 && a != doorPlacementIndex + 5)
                                    {
                                        GameObject pillar = SpawnPrefab(blockCollection.pillar,
                                                new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                        PillarBlock pBlock = pillar.GetComponent<PillarBlock>();
                                        pBlock.DeactivateRightPillar();
                                        if (a == adjBuildLength - 1 || a == doorPlacementIndex + 4) { pBlock.DeactivateLeftPillar(); }
                                    }
                                }
                                else if (i == 1 || i == 2)
                                {
                                    float yRotation = 0;
                                    if (a == adjBuildLength - 1) { yRotation = 180; }
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 90 + yRotation, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateRightPillar();
                                    if ((i == 2 && a == adjBuildLength - 1) || (i == 1 && a == 0)) { pillarGround.DeactivateLeftPillar(); }
                                }
                                else
                                {
                                    if (a > 0 && a < adjBuildLength - 1)
                                    {
                                        ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 180, 0));
                                        pillarGround = ground.GetComponent<PillaredGround>();
                                        pillarGround.SetCorner(false, false);
                                        pillarGround.DeactivateLeftPillar();
                                    }
                                    else
                                    {
                                        float yRotation = 0;
                                        if (a == adjBuildLength - 1) { yRotation = 90; }
                                        ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 90 + yRotation, 0));
                                        pillarGround = ground.GetComponent<PillaredGround>();
                                        if (a == adjBuildLength - 1) { pillarGround.DeactivateLeftPillar(); }
                                    }
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }

                    centerMargin = -0.5f * adjBuildLength + 2;
                    int sideLength = 8;

                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -3.5f;

                        for (int a = 0; a < sideLength; a++)
                        {
                            if (a > 1 && a < 6)
                            {
                                currentPos += 1;
                                continue;
                            }

                            GameObject ground;
                            PillaredGround pillarGround;
                            if (i == 0)
                            {
                                if (a > 0 && a < sideLength - 1)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 90, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                }
                                else
                                {
                                    float yRotation = 0;
                                    if (a == sideLength - 1) { yRotation = 270; }
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 90 + yRotation, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    if (a == 0) { pillarGround.DeactivateBackPillar(); }
                                    pillarGround.DeactivateLeftPillar();
                                }
                            }
                            else
                            {
                                if (a > 0 && a < sideLength - 1)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 270, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                }
                                else
                                {
                                    float yRotation = 0;
                                    if (a == sideLength - 1) { yRotation = 90; }
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 180 + yRotation, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    if (a == sideLength - 1) { pillarGround.DeactivateLeftPillar(); }
                                }
                            }
                            currentPos += 1;
                        }

                        centerMargin += 1;
                    }

                    TriggerNextSymbol();
                    break;
                }
            case 1:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if (((i == 1 || i == 2) && a > 0 && a < adjBuildLength - 1) ||
                                ((i == 0 || i == 3) && a > 1 && a < 4))
                            {
                                currentPos += 1;
                                continue;
                            }

                            if (i == 0)
                            {
                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    float yRotation = 0;
                                    if (a == adjBuildLength - 1) { yRotation = -90; }

                                    SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180 + yRotation, 0));
                                }
                                else
                                {
                                    if (a == doorPlacementIndex + 5)
                                    {
                                        SpawnPrefab(blockCollection.woodWallDoor,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                    }
                                    else
                                    {
                                        SpawnPrefab(blockCollection.woodWall,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                    }
                                }

                                // Place planks
                                if (i == 0 && a > 3)
                                {
                                    SpawnPrefab(blockCollection.groundPlank,
                                            new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }

                                // Place pillars
                                if (i == 0 && a > 3)
                                {
                                    GameObject pillar = SpawnPrefab(blockCollection.pillar,
                                            new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    PillarBlock pBlock = pillar.GetComponent<PillarBlock>();
                                    if (a != doorPlacementIndex + 5) 
                                    {
                                        pBlock.SetPillarType(true, true);
                                        pBlock.SetFence(true, true);
                                        if (a != doorPlacementIndex + 4) pBlock.DeactivateRightPillar();
                                    }
                                    if (a == doorPlacementIndex + 4) { pBlock.DeactivateLeftPillar(); }
                                }
                            }
                            else if (i == 3)
                            {
                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    float yRotation = 0;
                                    if (a == adjBuildLength - 1) { yRotation = 90; }

                                    SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270 + yRotation, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                            }
                            else
                            {
                                float yRotation = 0;
                                if (a == adjBuildLength - 1) { yRotation = 180; }

                                SpawnPrefab(blockCollection.woodWall,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270 + yRotation, 0));
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }

                    centerMargin = -0.5f * adjBuildLength + 2;
                    int sideLength = 8;

                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -3.5f;

                        for (int a = 0; a < sideLength; a++)
                        {
                            if (a > 1 && a < 6)
                            {
                                currentPos += 1;
                                continue;
                            }

                            if (i == 0)
                            {
                                if (a == 0 || a == sideLength - 1)
                                {
                                    float yRotation = 0;
                                    if (a == sideLength - 1) { yRotation = -90; }

                                    SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270 + yRotation, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }
                            }
                            else
                            {
                                if (a == 0 || a == sideLength - 1)
                                {
                                    float yRotation = 0;
                                    if (a == sideLength - 1) { yRotation = 90; }

                                    SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 0 + yRotation, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin += 1;
                    }

                    TriggerNextSymbol();
                    break;
                }
            case 2:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if ((i == 1 || i == 2) && a > 0 && a < adjBuildLength - 1)
                            {
                                if (i == 1 && (a == 1 || a == adjBuildLength - 2))
                                {
                                    float yRotation = 0;
                                    if (a == adjBuildLength - 2) { yRotation = -90; }

                                    SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180 + yRotation, 0));
                                }
                                else if (i == 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                                else if (i == 2 && (a == 1 || a == adjBuildLength - 2))
                                {
                                    float yRotation = 0;
                                    if (a == adjBuildLength - 2) { yRotation = 90; }

                                    SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, -90 + yRotation, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }

                                currentPos += 1;
                                continue;
                            }

                            if (i == 0)
                            {
                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    float yRotation = 0;
                                    if (a == adjBuildLength - 1) { yRotation = -90; }

                                    SpawnPrefab(blockCollection.flatRoofCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90 + yRotation, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.flatRoof,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }

                                if (a == doorPlacementIndex + 5)
                                {
                                    SpawnPrefab(blockCollection.highRoofCenterPillar,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    SpawnPrefab(blockCollection.highRoofCenterPillar,
                                        new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                            }
                            else if (i == 3)
                            {
                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    float yRotation = 0;
                                    if (a == adjBuildLength - 1) { yRotation = 90; }

                                    SpawnPrefab(blockCollection.flatRoofCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180 + yRotation, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.flatRoof,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                            }
                            else
                            {
                                float yRotation = 0;
                                if (a == adjBuildLength - 1) { yRotation = 180; }
                                
                                SpawnPrefab(blockCollection.flatRoof,
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90 + yRotation, 0));
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }

                    centerMargin = -0.5f * adjBuildLength + 2;
                    int sideLength = 8;

                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -3.5f;

                        for (int a = 0; a < sideLength; a++)
                        {
                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.highRoofLeft,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                }
                                else if (a == sideLength - 1)
                                {
                                    SpawnPrefab(blockCollection.highRoofRight,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.highRoof,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                }
                            }
                            else
                            {
                                GameObject roof;
                                if (a == 0)
                                {
                                    roof = SpawnPrefab(blockCollection.highRoofRight,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }
                                else if (a == sideLength - 1)
                                {
                                    roof = SpawnPrefab(blockCollection.highRoofLeft,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }
                                else
                                {
                                    roof = SpawnPrefab(blockCollection.highRoof,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }

                                ToppedRoof rBlock = roof.GetComponent<ToppedRoof>();
                                rBlock.HideTop();
                            }

                            currentPos += 1;
                        }

                        centerMargin += 1;
                    }

                    TriggerNextSymbol();
                    break;
                }
            case 3:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if ((i == 1 || i == 2) && a > 0 && a < adjBuildLength - 1)
                            {
                                if (i == 1)
                                {
                                    if (a == 1)
                                    {
                                        SpawnPrefab(blockCollection.highRoofLeft,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    }
                                    else if (a == adjBuildLength - 2)
                                    {
                                        SpawnPrefab(blockCollection.highRoofRight,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    }
                                    else
                                    {
                                        SpawnPrefab(blockCollection.highRoof,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    }
                                }
                                else if (i == 2)
                                {
                                    GameObject roof;
                                    if (a == 1)
                                    {
                                        roof = SpawnPrefab(blockCollection.highRoofRight,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                    }
                                    else if (a == adjBuildLength - 2)
                                    {
                                        roof = SpawnPrefab(blockCollection.highRoofLeft,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                    }
                                    else
                                    {
                                        roof = SpawnPrefab(blockCollection.highRoof,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                    }
                                    ToppedRoof rBlock = roof.GetComponent<ToppedRoof>();
                                    rBlock.HideTop();
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }

                    break;
                }
        }
    }

    private void TriggerNextSymbol()
    {
        MiddleLargeHouse remainingBuilding = CreateSymbol<MiddleLargeHouse>("Stage", new Vector3(0, heightPerBlock, 0));
        remainingBuilding.Initialize(buildLength, minLength, maxLength,
            currentStage + 1, doorPlacementIndex, blockCollection);
        remainingBuilding.Generate(buildDelay);
    }

    public override void ResetDefault()
    {
        doorPlacementIndex = -1;
        buildLength = -1;
    }
}
