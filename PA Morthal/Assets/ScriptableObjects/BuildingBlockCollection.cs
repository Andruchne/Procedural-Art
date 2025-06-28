using UnityEngine;

[CreateAssetMenu(fileName = "BuildingBlockCollection", menuName = "Scriptable Objects/BuildingBlockCollection")]
public class BuildingBlockCollection : ScriptableObject
{
    // Stone Ground
    public GameObject stoneGround;
    public GameObject stoneGroundPillar;
    public GameObject stoneGroundFullPillar;

    // Planks
    public GameObject unsnowedGroundPlank;
    public GameObject groundPlank;
    public GameObject groundPlankHalf;

    // Pillars
    public GameObject pillar;
    public GameObject thinPillar;

    // Stairs
    public GameObject stairs;
    public GameObject stoneStairs;

    // Walls
    public GameObject woodWall;
    public GameObject woodWallDoor;
    public GameObject woodWallCorner;
    public GameObject woodWallHalf;
    public GameObject woodAltWall;

    public GameObject stoneWall;
    public GameObject stoneWallCorner;
    public GameObject stoneWallDoor;

    // Roofs
    public GameObject highRoofLeft;
    public GameObject highRoof;
    public GameObject highRoofRight;
    public GameObject highestRoofCenter;
    public GameObject highRoofCenter;
    public GameObject highRoofCenterPillar;

    public GameObject darkRoofHigh;
    public GameObject darkRoofHighCenter;

    public GameObject flatRoofLeft;
    public GameObject flatRoof;
    public GameObject flatRoofRight;
    public GameObject flatRoofInnerCorner;
    public GameObject flatRoofCorner;
    public GameObject flatRoofPillar;

    public GameObject wideRoofLeft;
    public GameObject wideRoof;
    public GameObject wideRoofRight;

    // Deco
    public GameObject treeTrunkPile;
    public GameObject trunkHolder;
    public GameObject watermill;

    // Tree
    public GameObject branch1;
    public GameObject branch2;
    public GameObject branch3;

    public GameObject treeTrunk;
    public GameObject treeTop;
}
