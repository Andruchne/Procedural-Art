using Demo;
using System;
using UnityEngine;

public class SimpleHouse : Shape
{
    public enum FrontType
    {
        SimpleFront,
        WorkFront
    }

    public bool enableTypeSelection;
    public FrontType houseType;

    public int additionalWorkLength = 2;

    public int buildLength = -1;

    public float heightPerBlock = 1;

    public int maxLength = 5;
    public int minLength = 3;

    public int doorPlacementIndex = -1;

    public BuildingBlockCollection blockCollection;

    float halfedLength = -1;
    int currentStage = 0;
    int additional = 0;
    int length = -1;

    public void Initialize(int pBuildLength, int pMinLength, int pMaxLength, FrontType pHouseType, bool pEnableType,
        float pheightPerBlock, int pDoorPlacementIndex, int pCurrentStage, BuildingBlockCollection pBlockCollection)
    {
        buildLength = pBuildLength;
        minLength = pMinLength;
        maxLength = pMaxLength;
        heightPerBlock = pheightPerBlock;
        currentStage = pCurrentStage;
        blockCollection = pBlockCollection;
        doorPlacementIndex = pDoorPlacementIndex;
        houseType = pHouseType;
        enableTypeSelection = pEnableType;
    }

    protected override void Execute()
    {
        if (!enableTypeSelection)
        {
            int optionsCount = Enum.GetValues(typeof(FrontType)).Length;
            int index = RandomInt(0, optionsCount);
            houseType = (FrontType)index;
            enableTypeSelection = true;
        }

        if (buildLength < 0) { buildLength = RandomInt(minLength, maxLength + 1); }
        else { buildLength = Mathf.Clamp(buildLength, minLength, maxLength); }

        length = buildLength;

        // Randomize door placement
        if (doorPlacementIndex == -1 || doorPlacementIndex < 0 || doorPlacementIndex > length - 2)
        { doorPlacementIndex = RandomInt(0, length - 2);}

        // To make space for work exterior
        if (houseType == FrontType.WorkFront) 
        {
            additional = additionalWorkLength;
            length += additional;
        }

        halfedLength = (float)length / 2.0f;

        switch (currentStage)
        {
            // Ground
            case 0:
                {
                    float centerMargin = 0.5f;
                    float yRotation = 0;

                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < length; a++)
                        {
                            // Adjust the corner blocks rotation
                            float extraRotation = 0;
                            if (i == 0) { if (a == length - 1) { extraRotation = 90; } }
                            else { if (a == 0) { extraRotation = 90; } }

                            GameObject ground = SpawnPrefab(blockCollection.stoneGroundPillar, 
                                new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, yRotation - extraRotation, 0));
                            currentPos += 1;

                            // Hide unnecessary pillars (prevents z-fighting)
                            PillaredGround pillarGround = ground.GetComponent<PillaredGround>();
                            if (a > 0 && a < length -1)
                            {
                                pillarGround.SetCorner(false, false);
                                pillarGround.DeactivateRightPillar();
                            }
                            else if (i == 0 && a == length - 1) { pillarGround.DeactivateBackPillar(); }
                            else if (i == 1 && a == 0) 
                            { 
                                pillarGround.DeactivateLeftPillar();
                                pillarGround.DeactivateBackPillar();
                            }
                            else if (i == 1 && a == length - 1) { pillarGround.SetCorner(true, false); }

                            if (houseType == FrontType.WorkFront && i == 0 && a == 1) { pillarGround.DeactivateUpperPillars(); }
                        }

                        centerMargin *= -1;
                        yRotation += 180;
                    }

                    switch (houseType)
                    {
                        case FrontType.SimpleFront:
                            {
                                float currentPos = -halfedLength;
                                for (int i = 0; i < length; i++)
                                {
                                    GameObject pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(1.5f, 0, currentPos), Quaternion.Euler(0, 0, 0));

                                    // Hide unnecessary pillars
                                    PillarBlock pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.DeactivateRightPillar();
                                    if (currentPos == halfedLength - 1) { pBlock.DeactivateLeftPillar(); }

                                    if (currentPos == -halfedLength + 1 * doorPlacementIndex) { pBlock.DeactivateLeftPillar(); }
                                    else if (currentPos == -halfedLength + 1 + 1 * doorPlacementIndex) { pBlock.DeactivateLeftPillar(); }

                                    currentPos += 1;
                                }

                                // Spawn stairs
                                GameObject stairsLeft = SpawnPrefab(blockCollection.stairs,
                                    new Vector3(1.5f, 0, -halfedLength - 1), Quaternion.Euler(0, 90, 0));
                                Stairs lStair = stairsLeft.GetComponent<Stairs>();
                                lStair.DeactivateLeftPillar();

                                GameObject stairsRight = SpawnPrefab(blockCollection.stairs,
                                    new Vector3(1.5f, 0, halfedLength), Quaternion.Euler(0, 270, 0));
                                Stairs rStair = stairsRight.GetComponent<Stairs>();
                                rStair.DeactivateRightPillar();

                                GameObject stairsFront = SpawnPrefab(blockCollection.stairs,
                                    new Vector3(2.5f, 0, (-halfedLength + 1) + 1 * doorPlacementIndex), Quaternion.Euler(0, 0, 0));
                                break;
                            }
                        case FrontType.WorkFront:
                            float currentPos2 = -halfedLength;
                            for (int i = 0; i < length - 1; i++)
                            {
                                if (i > 1)
                                {
                                    GameObject pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(1.5f, 0, currentPos2), Quaternion.Euler(0, 0, 0));

                                    // Hide unnecessary pillars
                                    PillarBlock pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.DeactivateRightPillar();

                                    if (currentPos2 == -halfedLength + 1 * doorPlacementIndex + additional) { pBlock.DeactivateLeftPillar(); }
                                    else if (currentPos2 == -halfedLength + 1 + 1 * doorPlacementIndex + additional) { pBlock.DeactivateLeftPillar(); }
                                }

                                currentPos2 += 1;
                            }

                            GameObject stairsLeft2 = SpawnPrefab(blockCollection.stairs,
                                    new Vector3(2.5f, 0, -halfedLength + 1), Quaternion.Euler(0, 0, 0));
                            Stairs lStair2 = stairsLeft2.GetComponent<Stairs>();

                            GameObject stairsRight2 = SpawnPrefab(blockCollection.stairs,
                                new Vector3(1.5f, 0, halfedLength), Quaternion.Euler(0, 270, 0));
                            Stairs rStair2 = stairsRight2.GetComponent<Stairs>();
                            rStair2.DeactivateRightPillar();

                            GameObject stairsFront2 = SpawnPrefab(blockCollection.stairs,
                                new Vector3(2.5f, 0, (-halfedLength + 1) + 1 * doorPlacementIndex + additional), Quaternion.Euler(0, 0, 0));
                            break;
                    }

                    TriggerNextSymbol();
                    break;
                }
            // Walls
            case 1:
                {
                    float centerMargin = 0.5f;
                    float yRotation = 180;

                    bool isDoor = true;
                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -halfedLength + additional;

                        for (int a = 0; a < length - additional; a++)
                        {
                            float extraRotation = 0;
                            if (i == 0) { if (a == length - 1 - additional) { extraRotation = -90; } }
                            else { if (a == 0) { extraRotation = -90; } }

                            if (a == 0 || a == length - 1 - additional)
                            {
                                GameObject cornerWall = SpawnPrefab(blockCollection.woodWallCorner, 
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation + extraRotation, 0));
                            }
                            else if (isDoor && doorPlacementIndex == a - 1)
                            {
                                GameObject wallDoor = SpawnPrefab(blockCollection.woodWallDoor, 
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation + extraRotation, 0));
                            }
                            else
                            {
                                GameObject wall = SpawnPrefab(blockCollection.woodWall, 
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation + extraRotation, 0));
                            }

                            if (houseType == FrontType.SimpleFront && i == 0)
                            {
                                // Spawn Planks
                                GameObject plank = SpawnPrefab(blockCollection.groundPlank,
                                    new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));

                                // Spawn Pillars
                                GameObject pillar = SpawnPrefab(blockCollection.pillar,
                                    new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                PillarBlock pBlock = pillar.GetComponent<PillarBlock>();
                                pBlock.SetPillarType(true, true);
                                if (currentPos != -halfedLength + 1 + (1 * doorPlacementIndex)) { pBlock.SetFence(true, true); }
                                else
                                {
                                    GameObject thinPillar = SpawnPrefab(blockCollection.thinPillar,
                                    new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }

                                if (currentPos > -halfedLength) { pBlock.DeactivateRightPillar(); }
                            }

                            currentPos += 1;
                        }

                        if (houseType == FrontType.WorkFront && i == 0)
                        {
                            float currentPos2 = -halfedLength;
                            for (int a = 0; a < length; a++)
                            {
                                // Spawn Planks
                                if (a > 0)
                                {
                                    GameObject plank = SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin + 1, 0, currentPos2), Quaternion.Euler(0, 0, 0));
                                }
                                if (a < 2)
                                {
                                    GameObject plank = SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin, 0, currentPos2), Quaternion.Euler(0, 0, 0));
                                    GameObject plank2 = SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(-centerMargin, 0, currentPos2), Quaternion.Euler(0, 0, 0));
                                }

                                // Spawn Pillars
                                if (a == 0)
                                {
                                    // Pillars in work environment
                                    GameObject pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0, currentPos2), Quaternion.Euler(0, 0, 0));
                                    PillarBlock pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, true);
                                    pBlock.DeactivateRightPillar();

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0, currentPos2), Quaternion.Euler(0, 90, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, true);

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(-centerMargin, 0, currentPos2), Quaternion.Euler(0, 90, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, true);
                                    pBlock.DeactivateLeftPillar();

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(-centerMargin, 0, currentPos2), Quaternion.Euler(0, 180, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, true);
                                    pBlock.DeactivateLeftPillar();

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(-centerMargin, 0, currentPos2 + 1), Quaternion.Euler(0, 180, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, true);
                                    pBlock.DeactivateLeftPillar();
                                }
                                else if (a > 1)
                                {
                                    GameObject pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin + 1, 0, currentPos2), Quaternion.Euler(0, 0, 0));
                                    PillarBlock pBlock = pillar.GetComponent<PillarBlock>();
                                    if (a != 2) { pBlock.DeactivateRightPillar(); }
                                }

                                currentPos2 += 1;
                            }
                        }

                        isDoor = false;
                        centerMargin *= -1;
                        yRotation += 180;
                    }
                    TriggerNextSymbol();
                    break;
                }
            // Roof
            case 2:
                {
                    float centerMargin = 0.5f;
                    float yRotation = 0;

                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -halfedLength + additional;

                        for (int a = 0; a < length - additional; a++)
                        {
                            GameObject roof;
                            if (i == 0)
                            {
                                if (a == 0) { roof = SpawnPrefab(blockCollection.highRoofLeft, 
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0)); }

                                else if (a == length - 1 - additional) { roof = SpawnPrefab(blockCollection.highRoofRight,
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0)); }

                                else { roof = SpawnPrefab(blockCollection.highRoof,
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0)); }
                                
                                if (houseType == FrontType.WorkFront)
                                {
                                    if (a == 0) { roof = SpawnPrefab(blockCollection.wideRoofLeft,
                                        new Vector3(centerMargin + 0.7f, 0, currentPos + 0.0001f), Quaternion.Euler(0, yRotation, 0)); }

                                    else if (a == length - 1 - additional) { roof = SpawnPrefab(blockCollection.wideRoofRight,
                                        new Vector3(centerMargin + 0.7f, 0, currentPos - 0.0001f), Quaternion.Euler(0, yRotation, 0)); }

                                    else { roof = SpawnPrefab(blockCollection.wideRoof,
                                        new Vector3(centerMargin + 0.7f, 0, currentPos), Quaternion.Euler(0, yRotation, 0));}

                                    roof.transform.localScale = new Vector3(1.5f, 1, 1);
                                }
                            }
                            else
                            {
                                if (a == 0) { roof = SpawnPrefab(blockCollection.highRoofRight,
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0)); }

                                else if (a == length - 1 - additional) { roof = SpawnPrefab(blockCollection.highRoofLeft,
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0)); }

                                else { roof = SpawnPrefab(blockCollection.highRoof,
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0)); }
                            }

                            // Hide unnecessary pillars
                            ToppedRoof tRoof = roof.GetComponent<ToppedRoof>();
                            if (i == 1) { tRoof.HideTop(); }
                            
                            if (houseType == FrontType.SimpleFront && i == 0)
                            {
                                if (currentPos == -halfedLength + 1 + (1 * doorPlacementIndex))
                                {
                                    roof = SpawnPrefab(blockCollection.highRoofCenter,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    roof = SpawnPrefab(blockCollection.highRoofCenter,
                                        new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                            }

                            currentPos += 1;
                        }

                        currentPos = -halfedLength;

                        // Roof for work space
                        for (int a = 0; a < 2; a++)
                        {
                            GameObject roof;
                            if (a == 0)
                            {
                                if (i == 0)
                                {
                                    roof = SpawnPrefab(blockCollection.flatRoofLeft,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0));
                                }
                                else
                                {
                                    roof = SpawnPrefab(blockCollection.flatRoofRight,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0));
                                    ToppedRoof tRoof = roof.GetComponent<ToppedRoof>();
                                    tRoof.HideTop();
                                }
                            }
                            else
                            {
                                roof = SpawnPrefab(blockCollection.flatRoofPillar,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, yRotation, 0));
                                if (i == 1)
                                {
                                    ToppedRoof tRoof = roof.GetComponent<ToppedRoof>();
                                    tRoof.HideTop();
                                }
                            }
                            currentPos += 1;
                        }

                        centerMargin *= -1;
                        yRotation += 180;
                    }
                    break;
                }
        }
    }

    private void TriggerNextSymbol()
    {
        SimpleHouse remainingBuilding = CreateSymbol<SimpleHouse>("Stage", new Vector3(0, heightPerBlock, 0));
        remainingBuilding.Initialize(buildLength, minLength, maxLength, houseType, enableTypeSelection, heightPerBlock, 
            doorPlacementIndex, currentStage + 1, blockCollection);
        remainingBuilding.Generate(buildDelay);
    }

    public override void ResetDefault()
    {
        enableTypeSelection = false;
        doorPlacementIndex = -1;
        buildLength = -1;
    }
}
