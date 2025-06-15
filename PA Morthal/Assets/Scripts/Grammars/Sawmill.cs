using UnityEngine;
using Demo;

public class Sawmill : Shape
{
    public int buildLength = -1;

    public float heightPerBlock = 1;

    public int maxLength = 5;
    public int minLength = 3;

    public BuildingBlockCollection blockCollection;

    float halfedLength = -1;
    int currentStage = 0;

    public void Initialize(int pBuildLength, int pMinLength, int pMaxLength, int pCurrentStage, BuildingBlockCollection pBlockCollection)
    {
        buildLength = pBuildLength;
        minLength = pMinLength;
        maxLength = pMaxLength;
        blockCollection = pBlockCollection;
        currentStage = pCurrentStage;
    }

    protected override void Execute()
    {
        if (buildLength < 0) {  buildLength = RandomInt(minLength, maxLength + 1); }
        else { buildLength = Mathf.Clamp(buildLength, minLength, maxLength); }

        halfedLength = (float)buildLength / 2.0f;

        float centerMargin = 1;

        switch (currentStage)
        {
            // Ground
            case 0:
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength; a++)
                        {
                            GameObject ground;

                            if (a < 1)
                            {
                                SpawnPrefab(blockCollection.stoneStairs, 
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                            }
                            else if (i == 0)
                            {
                                ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                PillaredGround pillarGround = ground.GetComponent<PillaredGround>();
                                pillarGround.SetCorner(false, false);
                                pillarGround.DeactivateUpperPillars();
                            }
                            else if (i == 1)
                            {
                                SpawnPrefab(blockCollection.stoneGround, 
                                    new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));

                                if (a == buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0f, currentPos + 1.5f), Quaternion.Euler(0, 90, 0));
                                }
                            }
                            else if (i == 2)
                            {
                                ground = SpawnPrefab(blockCollection.stoneGroundPillar, 
                                    new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 180, 0));
                                PillaredGround pillarGround = ground.GetComponent<PillaredGround>();
                                pillarGround.SetCorner(false, false);
                                pillarGround.DeactivateUpperPillars();
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
                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength; a++)
                        {
                            if (a > 0)
                            {
                                SpawnPrefab(blockCollection.groundPlank,
                                    new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));

                                if (i == 0)
                                {
                                    SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));

                                    if (a == Mathf.Floor((buildLength - 1) / 2))
                                    {
                                        SpawnPrefab(blockCollection.watermill,
                                        new Vector3(centerMargin + 1, 0, currentPos + 0.5f), Quaternion.Euler(0, 0, 0));
                                    }
                                }
                                else if (i == 1)
                                {
                                    SpawnPrefab(blockCollection.trunkHolder,
                                            new Vector3(centerMargin, 0.084f, currentPos + 1), Quaternion.Euler(0, 90, 0));

                                    if (a == buildLength - 1)
                                    {
                                        SpawnPrefab(blockCollection.groundPlankHalf,
                                            new Vector3(centerMargin, 0f, currentPos + 1), Quaternion.Euler(0, 270, 0));
                                    }
                                }
                                else if (i == 2)
                                {
                                    SpawnPrefab(blockCollection.pillar,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));

                                    if (a % 3 == 0)
                                    {
                                        SpawnPrefab(blockCollection.treeTrunkPile,
                                            new Vector3(centerMargin, 0.256f, currentPos - 0.5f), Quaternion.Euler(0, 90, 0));
                                    }
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
                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength; a++)
                        {
                            if (a > 0)
                            {
                                if (i == 0)
                                {
                                    SpawnPrefab(blockCollection.darkRoofHigh,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (i == 1)
                                {
                                    if (a == 1)
                                    {
                                        SpawnPrefab(blockCollection.woodAltWall,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                    }
                                    else if (a == buildLength - 1)
                                    {
                                        SpawnPrefab(blockCollection.woodAltWall,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                                    }
                                }
                                else if (i == 2)
                                {
                                    SpawnPrefab(blockCollection.darkRoofHigh,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
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
                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength; a++)
                        {
                            if (a > 0)
                            {
                                if (i == 1)
                                {
                                    SpawnPrefab(blockCollection.darkRoofHighCenter,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
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
        Sawmill remainingBuilding = CreateSymbol<Sawmill>("Stage", new Vector3(0, heightPerBlock, 0));
        remainingBuilding.Initialize(buildLength, minLength, maxLength,
            currentStage + 1, blockCollection);
        remainingBuilding.Generate(buildDelay);
    }

    public override void ResetDefault()
    {
        buildLength = -1;
    }
}
