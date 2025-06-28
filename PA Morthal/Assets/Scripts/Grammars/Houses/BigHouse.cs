using UnityEngine;
using Demo;

public class BigHouse : Shape
{
    public int buildLength = -1;

    public float heightPerBlock = 1;

    public int maxLength = 3;
    public int minLength = 2;

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
        if (buildLength < 0) { buildLength = RandomInt(minLength, maxLength + 1); }
        else { buildLength = Mathf.Clamp(buildLength, minLength, maxLength); }

        halfedLength = ((float)buildLength * 2.0f + 3) / 2.0f;

        float centerMargin = 0.5f;

        int adjBuildLength = buildLength * 2 + 3;

        switch (currentStage)
        {
            case 0:
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            GameObject ground;
                            PillaredGround pillarGround;
                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a > buildLength - 1 && a < adjBuildLength - buildLength)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGround,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 270, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.DeactivateBackPillar();
                                }
                                else
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateRightPillar();
                                }
                            }
                            else if (i == 1)
                            {
                                if (a == 0)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 90, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                    pillarGround.DeactivateRightPillar();
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 270, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                    pillarGround.DeactivateRightPillar();
                                }
                                else
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGround,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                            }
                            else
                            {
                                if (a == 0)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 90, 0));
                                }
                                else if (a > buildLength - 1 && a < adjBuildLength - buildLength)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGround,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 180, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.DeactivateLeftPillar();
                                }
                                else
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 180, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }

                    centerMargin = -1.5f;

                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = - halfedLength;

                        for (int a = 0; a < buildLength * 2 + 3; a++)
                        {
                            if (a >= buildLength && a < adjBuildLength - buildLength)
                            {
                                currentPos += 1;
                                continue;
                            }

                            GameObject ground;
                            PillaredGround pillarGround;
                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 90, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 0, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.DeactivateLeftPillar();
                                }
                                else
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 90, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                    pillarGround.DeactivateRightPillar();
                                }
                            }
                            else if (i == 1)
                            {
                                if (a == 0)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 180, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                    pillarGround.DeactivateRightPillar();
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 0, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                    pillarGround.DeactivateRightPillar();

                                    SpawnPrefab(blockCollection.stairs,
                                            new Vector3(currentPos + 1, 0, centerMargin), Quaternion.Euler(0, 0, 0));
                                }
                                else
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGround,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 0, 0));
                                }
                            }
                            else
                            {
                                if (a == 0)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 180, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 270, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.DeactivateBackPillar();
                                }
                                else
                                {
                                    ground = SpawnPrefab(blockCollection.stoneGroundPillar,
                                            new Vector3(currentPos, 0.5f, centerMargin), Quaternion.Euler(0, 270, 0));
                                    pillarGround = ground.GetComponent<PillaredGround>();
                                    pillarGround.SetCorner(false, false);
                                    pillarGround.DeactivateLeftPillar();
                                    pillarGround.DeactivateRightPillar();
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
                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.woodWallCorner,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWallCorner,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                                }
                                else if (a < buildLength || a > adjBuildLength - buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                            }
                            else if (i == 1)
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                                }
                            }
                            else
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.woodWallCorner,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWallCorner,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a < buildLength || a > adjBuildLength - buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }

                    centerMargin = -1.5f;

                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if (a >= buildLength && a < adjBuildLength - buildLength)
                            {
                                currentPos += 1;
                                continue;
                            }

                            if (i == 0)
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.woodWallCorner,
                                            new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWallCorner,
                                            new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 180, 0));
                                }
                                else if (a < buildLength || a > adjBuildLength - buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                            new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }
                            }
                            else if (i == 1)
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                            new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWallDoor,
                                            new Vector3(currentPos + 0.1f, 0, centerMargin), Quaternion.Euler(0, 180, 0));
                                }
                            }
                            else
                            {
                                if (a == 0)
                                {
                                    SpawnPrefab(blockCollection.woodWallCorner,
                                            new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWallCorner,
                                            new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                }
                                else if (a < buildLength || a > adjBuildLength - buildLength - 1)
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
                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if (i == 0)
                            {
                                float yRotation = 0;
                                if (a > buildLength) { yRotation = -90; }

                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoofCorner,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90 + yRotation, 0));
                                }
                                else if (a < buildLength || (a >= adjBuildLength - buildLength && a <= adjBuildLength - 1))
                                {
                                    SpawnPrefab(blockCollection.flatRoof,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a == buildLength || a == adjBuildLength - buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoofInnerCorner,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90 + yRotation, 0));
                                }
                            }
                            else if (i == 1)
                            {
                                float yRotation = 0;
                                if (a > buildLength) { yRotation = 180; }

                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoof,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90 + yRotation, 0));
                                }
                            }
                            else
                            {
                                float yRotation = 0;
                                if (a > buildLength) { yRotation = 90; }

                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoofCorner,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180 + yRotation, 0));
                                }
                                else if (a < buildLength || (a >= adjBuildLength - buildLength && a <= adjBuildLength - 1))
                                {
                                    SpawnPrefab(blockCollection.flatRoof,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                                else if (a == buildLength || a == adjBuildLength - buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoofInnerCorner,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180 + yRotation, 0));
                                }
                            }

                            // Place walls

                            if ((i == 0 || i == 2) && a == (adjBuildLength - 1) / 2)
                            {
                                SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                SpawnPrefab(blockCollection.woodWall,
                                            new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                            }
                            else if (i == 1 && a != (adjBuildLength - 1) / 2)
                            {
                                if (a == 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 270, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a == adjBuildLength - 2)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                                else if (a > 0 && a < adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 0, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 180, 0));
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }

                    centerMargin = -1.5f;

                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if (a >= buildLength && a < adjBuildLength - buildLength)
                            {
                                currentPos += 1;
                                continue;
                            }

                            if (i == 0)
                            {
                                float yRotation = 0;
                                if (a > buildLength) { yRotation = -90; }

                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoofCorner,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 180 + yRotation, 0));
                                }
                                else if (a == buildLength || a == adjBuildLength - buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoofInnerCorner,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90 + yRotation, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.flatRoof,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                }
                            }
                            else if (i == 1)
                            {
                                float yRotation = 0;
                                if (a > buildLength) { yRotation = 180; }

                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoof,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 180 + yRotation, 0));

                                    if (a == adjBuildLength - 1)
                                    {
                                        SpawnPrefab(blockCollection.highRoofCenterPillar,
                                                new Vector3(currentPos - 1 + 0.1f, 0, centerMargin), Quaternion.Euler(0, 0, 0));
                                        SpawnPrefab(blockCollection.highRoofCenterPillar,
                                                new Vector3(currentPos + 0.1f, 0, centerMargin), Quaternion.Euler(0, 0, 0));
                                    }
                                }
                            }
                            else
                            {
                                float yRotation = 0;
                                if (a > buildLength) { yRotation = 90; }

                                if (a == 0 || a == adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoofCorner,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270 + yRotation, 0));
                                }
                                else if (a == buildLength || a == adjBuildLength - buildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.flatRoofInnerCorner,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 180 + yRotation, 0));
                                }
                                else
                                {
                                    SpawnPrefab(blockCollection.flatRoof,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }
                            }

                            // Place walls
                            if (i == 1)
                            {
                                if (a == 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 0, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                }
                                else if (a == adjBuildLength - 2)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 180, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }
                                else if (a != (adjBuildLength - 1) / 2 && a > 0 && a < adjBuildLength - 1)
                                {
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 90, 0));
                                    SpawnPrefab(blockCollection.woodWall,
                                                new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 270, 0));
                                }
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
                    for (int i = 0; i < 2; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < adjBuildLength; a++)
                        {
                            if (i == 1)
                            {
                                if (a == (adjBuildLength - 1) / 2)
                                {
                                    SpawnPrefab(blockCollection.stoneGroundFullPillar,
                                        new Vector3(centerMargin, 0.5f, currentPos), Quaternion.Euler(0, 0, 0));
                                }
                                else if (a != 0 && a != adjBuildLength - 1)
                                {
                                    GameObject roof = SpawnPrefab(blockCollection.highestRoofCenter,
                                        new Vector3(centerMargin, 0, currentPos), Quaternion.Euler(0, 90, 0));
                                    roof.transform.localScale = new Vector3(0.91f, 1, 1);
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin -= 1;
                    }

                    centerMargin = -1.5f;

                    for (int i = 0; i < 3; i++)
                    {
                        float currentPos = -halfedLength;

                        for (int a = 0; a < buildLength * 2 + 3; a++)
                        {
                            if (i == 1)
                            {
                                if (a != 0 && a != adjBuildLength - 1 && a != (adjBuildLength - 1) / 2)
                                {
                                    GameObject roof = SpawnPrefab(blockCollection.highestRoofCenter,
                                        new Vector3(currentPos, 0, centerMargin), Quaternion.Euler(0, 0, 0));
                                    roof.transform.localScale = new Vector3(0.91f, 1, 1);
                                }
                            }

                            currentPos += 1;
                        }

                        centerMargin += 1;
                    }

                    TriggerNextSymbol();
                    break;
                }
            case 4:
                {
                    float yRotation = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        SpawnPrefab(blockCollection.woodWallHalf,
                            new Vector3(-0.5f, 0, -0.5f), Quaternion.Euler(0, yRotation, 0));
                        yRotation += 90;
                    }

                    SpawnPrefab(blockCollection.highestRoofCenter,
                            new Vector3(-0.5f, 0.5f, -0.5f), Quaternion.Euler(0, 0, 0));
                    yRotation += 90;

                    Submerge(0, 0.6f);
                    break;
                }
        }
    }

    private void TriggerNextSymbol()
    {
        BigHouse remainingBuilding = CreateSymbol<BigHouse>("Stage", new Vector3(0, heightPerBlock, 0));
        remainingBuilding.Initialize(buildLength, minLength, maxLength,
            currentStage + 1, blockCollection);
        remainingBuilding.Generate(buildDelay);
    }

    public override void ResetDefault()
    {
        buildLength = -1;
    }
}
