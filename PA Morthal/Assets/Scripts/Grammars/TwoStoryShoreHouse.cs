using Demo;
using UnityEngine;

public class TwoStoryShoreHouse : Shape
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

        if (doorPlacementIndex == -1 || doorPlacementIndex < 0 || doorPlacementIndex > buildLength - 2)
        { doorPlacementIndex = RandomInt(0, buildLength - 2); }

        halfedLength = (float)buildLength / 2.0f;

        float centerMargin = 0.5f;

        switch (currentStage)
        {
            case 0:
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength; a++)
                        {
                            if (i == 0)
                            {
                                if (a == buildLength - 1)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 270, 0));
                                }
                                else if (a > 0)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                    PillaredGround pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                }
                                else
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                }

                                if (a - 1 == doorPlacementIndex)
                                {
                                    SpawnPrefab(blockCollection.stairs,
                                        new Vector3(centerMargin + 3, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }

                                // Place pillars

                                GameObject pillar;
                                PillarBlock pBlock;
                                for (int b = 0; b < 2; b++)
                                {
                                    if (a == 0)
                                    {
                                        if (b == 0)
                                        {
                                            pillar = SpawnPrefab(blockCollection.pillar,
                                            new Vector3(centerMargin, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                            pBlock = pillar.GetComponent<PillarBlock>();
                                            pBlock.DeactivateRightPillar();
                                        }

                                        pillar = SpawnPrefab(blockCollection.pillar,
                                            new Vector3(centerMargin + 1 + b, 0, currentPos - 1), Quaternion.Euler(0, 0, 0));
                                        pBlock = pillar.GetComponent<PillarBlock>();
                                        pBlock.DeactivateLeftPillar();
                                    }
                                    else if (a == buildLength - 1 && b == 0)
                                    {
                                        pillar = SpawnPrefab(blockCollection.pillar,
                                            new Vector3(centerMargin, 0, currentPos + 1), Quaternion.Euler(0, 270, 0));
                                        pBlock = pillar.GetComponent<PillarBlock>();
                                        pBlock.DeactivateLeftPillar();
                                    }

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                            new Vector3(centerMargin + 1 + b, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    if (a != buildLength - 1) { pBlock.DeactivateLeftPillar(); }
                                }
                            }
                            else if (i == 1)
                            {
                                if (a == buildLength - 1)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 180, 0));
                                    PillaredGround pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(true, false);
                                }
                                else if (a > 0)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 180, 0));
                                    PillaredGround pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                }
                                else
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 90, 0));
                                    PillaredGround pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(true, false);
                                    pillarGround.DeactivateLeftPillar();
                                }

                                // Place pillars
                                GameObject pillar;
                                PillarBlock pBlock;

                                if (a == 0)
                                {
                                   SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));

                                    SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin - 1, 0, currentPos - 1), Quaternion.Euler(0, 180, 0));
                                }
                                else if (a == buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0, currentPos + 1), Quaternion.Euler(0, 270, 0));

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin - 1, 0, currentPos + 1), Quaternion.Euler(0, 270, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.DeactivateRightPillar();
                                }

                                pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin - 1, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                pBlock = pillar.GetComponent<PillarBlock>();
                                pBlock.DeactivateLeftPillar();

                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }
                    TriggerNextSymbol();
                    break;
                }
            case 1:
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength; a++)
                        {
                            if (i == 0)
                            {
                                if (a == buildLength - 1)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                                }
                                else if (a - 1 == doorPlacementIndex)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneWallDoor,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                                else if (a > 0)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneWall,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                                else
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                            }
                            else if (i == 1)
                            {
                                if (a == buildLength - 1)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a > 0)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneWall,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.stoneWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                }
                            }

                            // Place pillars

                            GameObject pillar;
                            PillarBlock pBlock;

                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    pillar = SpawnPrefab(blockCollection.pillar,
                                    new Vector3(centerMargin + 1, 0, currentPos - 1), Quaternion.Euler(0, 0, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.DeactivateLeftPillar();

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                    new Vector3(centerMargin + 1, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.DeactivateLeftPillar();

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                    new Vector3(centerMargin, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.DeactivateLeftPillar();
                                }

                                pillar = SpawnPrefab(blockCollection.pillar,
                                    new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                pBlock = pillar.GetComponent<PillarBlock>();
                                if (a != buildLength - 1) { pBlock.DeactivateLeftPillar(); }
                            }
                            else if (i == 1 && a == 0)
                            {
                                pillar = SpawnPrefab(blockCollection.pillar,
                                    new Vector3(centerMargin - 1, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                pBlock = pillar.GetComponent<PillarBlock>();
                                pBlock.DeactivateLeftPillar();

                                pillar = SpawnPrefab(blockCollection.pillar,
                                new Vector3(centerMargin, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                pBlock = pillar.GetComponent<PillarBlock>();
                                pBlock.DeactivateLeftPillar();

                                pillar = SpawnPrefab(blockCollection.pillar,
                                new Vector3(centerMargin - 1, 0, currentPos - 1), Quaternion.Euler(0, 180, 0));
                                pBlock = pillar.GetComponent<PillarBlock>();
                                pBlock.DeactivateLeftPillar();
                            }

                            // Place planks
                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin + 2, 0, currentPos - 1), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a - 1 == doorPlacementIndex)
                                {
                                    SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a < buildLength)
                                {
                                    SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin, 0, currentPos + 1), Quaternion.Euler(0, 0, 0));
                                }

                                SpawnPrefab(blockCollection.groundPlank,
                                    new Vector3(centerMargin + 2, 0, currentPos), Quaternion.Euler(0, 0, 0));
                            }
                            else if (i == 1)
                            {
                                if (a == buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin, 0, currentPos + 1), Quaternion.Euler(0, 0, 0));
                                    SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin - 1, 0, currentPos + 1), Quaternion.Euler(0, 0, 0));
                                    SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin - 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }

                                // Place stairs
                                if (a == buildLength - 2)
                                {
                                    SpawnPrefab(blockCollection.stairs,
                                        new Vector3(centerMargin - 1, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }
                    TriggerNextSymbol();
                    break;
                }
            case 2:
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength; a++)
                        {
                            if (i == 0)
                            {
                                if (a == buildLength - 1)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                                }
                                else if (a - 1 == doorPlacementIndex)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.woodWallDoor,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                                else if (a > 0)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.woodWall,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                                else
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                            }
                            else if (i == 1)
                            {
                                if (a == buildLength - 1)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a > 0)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.woodWall,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.woodWallCorner,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                }
                            }

                            // Place pillars
                            GameObject pillar;
                            PillarBlock pBlock;

                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, false);
                                    pBlock.SetPillarType(true, true);
                                    pBlock.DeactivateLeftPillar();

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin + 1, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, false);
                                    pBlock.SetPillarType(true, true);
                                    pBlock.DeactivateLeftPillar();

                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin + 1, 0, currentPos - 1), Quaternion.Euler(0, 0, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, false);
                                    pBlock.SetPillarType(true, true);
                                    pBlock.DeactivateLeftPillar();
                                }
                                else if (a == buildLength - 1)
                                {
                                    pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                    pBlock = pillar.GetComponent<PillarBlock>();
                                    pBlock.SetFence(true, false);
                                    pBlock.DeactivateLeftPillar();
                                }

                                pillar = SpawnPrefab(blockCollection.pillar,
                                    new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                pBlock = pillar.GetComponent<PillarBlock>();
                                pBlock.SetFence(true, false);
                                pBlock.DeactivateLeftPillar();
                            }
                            else if (i == 1 && a == 0)
                            {
                                pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                pBlock = pillar.GetComponent<PillarBlock>();
                                pBlock.SetFence(true, false);
                                pBlock.SetPillarType(true, true);
                                pBlock.DeactivateLeftPillar();

                                pillar = SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin - 1, 0, currentPos - 1), Quaternion.Euler(0, 90, 0));
                                pBlock = pillar.GetComponent<PillarBlock>();
                                pBlock.SetFence(true, false);
                                pBlock.SetPillarType(true, true);
                                pBlock.DeactivateLeftPillar();
                            }

                            // Place planks
                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin, 0, currentPos - 1), Quaternion.Euler(0, 0, 0));
                                    SpawnPrefab(blockCollection.groundPlank,
                                        new Vector3(centerMargin + 1, 0, currentPos - 1), Quaternion.Euler(0, 0, 0));
                                }

                                SpawnPrefab(blockCollection.groundPlank,
                                    new Vector3(centerMargin + 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                            }
                            else if (i == 1 && a == 0)
                            {
                                SpawnPrefab(blockCollection.groundPlank,
                                    new Vector3(centerMargin - 1, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                SpawnPrefab(blockCollection.groundPlank,
                                    new Vector3(centerMargin, 0, currentPos - 1), Quaternion.Euler(0, 0, 0));
                                SpawnPrefab(blockCollection.groundPlank,
                                    new Vector3(centerMargin - 1, 0, currentPos - 1), Quaternion.Euler(0, 0, 0));
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }
                    TriggerNextSymbol();
                    break;
                }
            case 3:
                {
                    float yRotation = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength; a++)
                        {
                            if (a == buildLength - 1)
                            {
                                if (i == 0)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.highRoofRight,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0 + yRotation, 0));
                                }
                                else
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.highRoofLeft,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0 + yRotation, 0));
                                }
                            }
                            else if (a > 0)
                            {
                                GameObject ground = SpawnPrefab(blockCollection.highRoof,
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0 + yRotation, 0));
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.highRoofLeft,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0 + yRotation, 0));
                                }
                                else
                                {
                                    GameObject ground = SpawnPrefab(blockCollection.highRoofRight,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0 + yRotation, 0));
                                }
                            }

                            // Place front roof
                            if (i == 0)
                            {
                                GameObject roof;
                                if (a == 0)
                                {
                                   roof = SpawnPrefab(blockCollection.wideRoofLeft,
                                        new Vector3(centerMargin + 0.7f, 0, currentPos + 0.0001f), Quaternion.Euler(0, 0, 0));
                                }

                                else if (a == buildLength - 1)
                                {
                                    roof = SpawnPrefab(blockCollection.wideRoofRight,
                                    new Vector3(centerMargin + 0.7f, 0, currentPos - 0.0001f), Quaternion.Euler(0, 0, 0));
                                }

                                else
                                {
                                    roof = SpawnPrefab(blockCollection.wideRoof,
                                    new Vector3(centerMargin + 0.7f, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }

                                roof.transform.localScale = new Vector3(1.5f, 1, 1);
                            }

                            currentPos += 1;
                        }

                        yRotation += 180;
                        centerMargin -= 1;
                    }
                    TriggerNextSymbol();
                    break;
                }
        }


    }

    private void TriggerNextSymbol()
    {
        TwoStoryShoreHouse remainingBuilding = CreateSymbol<TwoStoryShoreHouse>("Stage", new Vector3(0, heightPerBlock, 0));
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
