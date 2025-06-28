using Demo;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using System;

public enum PositionType
{
    TwoSided,
    LeftSide,
    RightSide,
    Random
}

public class SplineVillage : Shape
{
    public PositionType positionType;

    public float minDistance = 5;
    public float maxDistance = 7;

    // How many building need to be placed, to have a chance to place this
    public int bigHousePerCount = 6;
    int currentBigHousePerCount = 0;

    public int middleLargeHousePerCount = 5;
    int currentMiddleLargeHousePerCount = 0;

    public int twoStoryShoreHousePerCount = 4;
    int currentTwoStoryShoreHousePerCount = 0;

    public int simpleHousePerCount = 1;
    int currentSimpleHousePerCount = 1;

    public VillageBuildingData villageBuildingData;

    List<Shape> buildingsInQueue = new List<Shape>();
    int currentQueueIndex = 0;

    private void Setup()
    {
        currentBigHousePerCount = bigHousePerCount;
        currentMiddleLargeHousePerCount = middleLargeHousePerCount;
        currentTwoStoryShoreHousePerCount = twoStoryShoreHousePerCount;
        currentSimpleHousePerCount = simpleHousePerCount;
    }

    protected override void Execute()
    {
        Setup();
        SplineContainer splineContainer = GetComponent<SplineContainer>();
        BezierKnot[] knots = splineContainer.Spline.Knots.ToArray();

        switch (positionType)
        {
            case PositionType.TwoSided:
                {
                    for (int i = 0; i < knots.Length; i++)
                    {
                        Quaternion rotation = knots[i].Rotation;
                        Vector3 pos = knots[i].Position;

                        for (int a = 0; a < 2; a++)
                        {
                            // For left and right direction
                            Vector3 direction = Vector3.right;
                            if (a == 1) { direction *= -1; }

                            float distance = RandomFloat(minDistance, maxDistance);

                            Vector3 rotatedVector = rotation * direction;
                            Vector3 housePos = pos + rotatedVector * distance;

                            // Convert local housePos to world space
                            Vector3 worldHousePos = transform.TransformPoint(housePos);

                            float rayHeight = 100;
                            float rayLength = 200;
                            Vector3 rayStart = worldHousePos + Vector3.up * rayHeight;

                            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayLength))
                            {
                                if (hit.collider.tag != "Water")
                                {
                                    AddBuildingToQueue();
                                    // Convert back to local space
                                    housePos = transform.InverseTransformPoint(hit.point);
                                    RemoveTypeFromQueue(SpawnBuilding(housePos, pos));
                                }
                            }
                        }
                    }
                    break;
                }
            case PositionType.LeftSide:
                {
                    Vector3 direction = Vector3.left;
                    PositionBuildings(splineContainer, knots, direction);

                    break;
                }
            case PositionType.RightSide:
                {
                    Vector3 direction = Vector3.right;
                    PositionBuildings(splineContainer, knots, direction);

                    break;
                }
            case PositionType.Random:
                {
                    for (int i = 0; i < knots.Length; i++)
                    {
                        Quaternion rotation = knots[i].Rotation;
                        Vector3 pos = knots[i].Position;

                        int randomIndex = RandomInt(0, 3);

                        if (randomIndex == 0)
                        {
                            Vector3 direction = Vector3.left;

                            float distance = RandomFloat(minDistance, maxDistance);

                            Vector3 rotatedVector = rotation * direction;
                            Vector3 housePos = pos + rotatedVector * distance;

                            // Convert local housePos to world space
                            Vector3 worldHousePos = transform.TransformPoint(housePos);

                            float rayHeight = 100;
                            float rayLength = 200;
                            Vector3 rayStart = worldHousePos + Vector3.up * rayHeight;

                            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayLength))
                            {
                                if (hit.collider.tag != "Water")
                                {
                                    AddBuildingToQueue();
                                    // Convert back to local space
                                    housePos = transform.InverseTransformPoint(hit.point);
                                    RemoveTypeFromQueue(SpawnBuilding(housePos, pos));
                                }
                            }
                        }
                        else if (randomIndex == 1)
                        {
                            Vector3 direction = Vector3.right;

                            float distance = RandomFloat(minDistance, maxDistance);

                            Vector3 rotatedVector = rotation * direction;
                            Vector3 housePos = pos + rotatedVector * distance;

                            // Convert local housePos to world space
                            Vector3 worldHousePos = transform.TransformPoint(housePos);

                            float rayHeight = 100;
                            float rayLength = 200;
                            Vector3 rayStart = worldHousePos + Vector3.up * rayHeight;

                            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayLength))
                            {
                                if (hit.collider.tag != "Water")
                                {
                                    AddBuildingToQueue();
                                    // Convert back to local space
                                    housePos = transform.InverseTransformPoint(hit.point);
                                    RemoveTypeFromQueue(SpawnBuilding(housePos, pos));
                                }
                            }
                        }
                        else
                        {
                            for (int a = 0; a < 2; a++)
                            {
                                // For left and right direction
                                Vector3 direction = Vector3.right;
                                if (a == 1) { direction *= -1; }

                                float distance = RandomFloat(minDistance, maxDistance);

                                Vector3 rotatedVector = rotation * direction;
                                Vector3 housePos = pos + rotatedVector * distance;

                                // Convert local housePos to world space
                                Vector3 worldHousePos = transform.TransformPoint(housePos);

                                float rayHeight = 100;
                                float rayLength = 200;
                                Vector3 rayStart = worldHousePos + Vector3.up * rayHeight;

                                if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayLength))
                                {
                                    if (hit.collider.tag != "Water")
                                    {
                                        AddBuildingToQueue();
                                        // Convert back to local space
                                        housePos = transform.InverseTransformPoint(hit.point);
                                        RemoveTypeFromQueue(SpawnBuilding(housePos, pos));
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
        }

        ResetData();
    }

    private void PositionBuildings(SplineContainer splineContainer, BezierKnot[] knots, Vector3 direction)
    {
        for (int i = 0; i < knots.Length; i++)
        {
            Quaternion rotation = knots[i].Rotation;
            Vector3 pos = knots[i].Position;

            float distance = RandomFloat(minDistance, maxDistance);

            Vector3 rotatedVector = rotation * direction;
            Vector3 housePos = pos + rotatedVector * distance;

            // Convert local housePos to world space
            Vector3 worldHousePos = transform.TransformPoint(housePos);

            float rayHeight = 100;
            float rayLength = 200;
            Vector3 rayStart = worldHousePos + Vector3.up * rayHeight;

            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, rayLength))
            {
                if (hit.collider.tag != "Water")
                {
                    AddBuildingToQueue();
                    // Convert back to local space
                    housePos = transform.InverseTransformPoint(hit.point);
                    RemoveTypeFromQueue(SpawnBuilding(housePos, pos));
                }
            }
        }
    }

    private void AddBuildingToQueue()
    {
        currentQueueIndex++;

        if (currentQueueIndex >= currentSimpleHousePerCount)
        {
            buildingsInQueue.Add(villageBuildingData.simpleHouse.GetComponent<SimpleHouse>());
            currentSimpleHousePerCount += simpleHousePerCount;
        }
        if (currentQueueIndex >= currentTwoStoryShoreHousePerCount)
        {
            buildingsInQueue.Add(villageBuildingData.twoStoryShoreHouse.GetComponent<TwoStoryShoreHouse>());
            currentTwoStoryShoreHousePerCount += twoStoryShoreHousePerCount;
        }
        if (currentQueueIndex >= currentMiddleLargeHousePerCount)
        {
            buildingsInQueue.Add(villageBuildingData.middleLargeHouse.GetComponent<MiddleLargeHouse>());
            currentMiddleLargeHousePerCount += middleLargeHousePerCount;
        }
        if (currentQueueIndex >= currentBigHousePerCount)
        {
            buildingsInQueue.Add(villageBuildingData.bigHouse.GetComponent<BigHouse>());
            currentBigHousePerCount += bigHousePerCount;
        }
    }

    private void RemoveTypeFromQueue(Type type)
    {
        for (int i = buildingsInQueue.Count - 1; i >= 0; i--)
        {
            if (type.IsInstanceOfType(buildingsInQueue[i]))
            {
                buildingsInQueue.RemoveAt(i);
            }
        }
    }

    private Type SpawnBuilding(Vector3 spawnPos, Vector3 lookAtPos)
    {
        if (buildingsInQueue.Count <= 0) { return null; }

        int randomIndex = RandomInt(0, buildingsInQueue.Count);

        GameObject building = SpawnPrefab(buildingsInQueue[randomIndex].gameObject, spawnPos);

        // Get x and z direction
        Vector3 direction = (lookAtPos - spawnPos);
        // Ignore y difference
        direction.y = 0;
        direction.Normalize();

        // Make the right vector point towards point
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        building.transform.rotation = Quaternion.Euler(0, -angle, 0);

        Shape shape = building.GetComponent<Shape>();
        shape.Generate();

        return buildingsInQueue[randomIndex].GetType();
    }

    public override void ResetDefault()
    {
        ResetData();
    }

    private void ResetData()
    {
        buildingsInQueue.Clear();
        currentQueueIndex = 0;

        currentBigHousePerCount = 0;
        currentMiddleLargeHousePerCount = 0;
        currentTwoStoryShoreHousePerCount = 0;
        currentSimpleHousePerCount = 0;
    }
}
