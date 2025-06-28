using Demo;
using UnityEngine;

public class PineTree : Shape
{
    public int height = -1;
    public int branchCountPerStage = -1;

    public float heightPerBranch = 0.5f;
    public float branchStartHeight = 1.0f;

    public int minHeight = 0;
    public int maxHeight = 2;

    public int minBranchCount = 8;
    public int maxBranchCount = 16;

    public float minBranchScale = 0.75f;
    public float maxBranchScale = 1.25f;

    public BuildingBlockCollection blockCollection;
    public GameObject LODTree;

    float branchScaleStep = 0.0f;

    float startHeight = 1.5f;
    float actualHeight = 0.0f;

    float currentHeight = 0.0f;

    int currentScaleStage = 0;

    float checkHeight = 0.0f;

    GameObject lod;


    public void Initialize(int pHeight, float pHeightPerBranch, float pBranchStartHeight, int pMinHeight, int pMaxHeight,
        float pMinBranchScale, float pMaxBranchScale, float pCurrentHeight, int pCurrentScaleStage, GameObject pLod,
        int pBranchCountPerStage, int pMinBranchCount, int pMaxBranchCount, BuildingBlockCollection pBlockCollection, float pCheckHeight)
    {
        height = pHeight;

        heightPerBranch = pHeightPerBranch;
        branchStartHeight = pBranchStartHeight;

        minHeight = pMinHeight;
        maxHeight = pMaxHeight;

        minBranchScale = pMinBranchScale;
        maxBranchScale = pMaxBranchScale;

        currentHeight = pCurrentHeight;
        currentScaleStage = pCurrentScaleStage;

        branchCountPerStage = pBranchCountPerStage;

        minBranchCount = pMinBranchCount;
        maxBranchCount = pMaxBranchCount;

        blockCollection = pBlockCollection;

        checkHeight = pCheckHeight;

        lod = pLod;
    }

    protected override void Execute()
    {
        CheckInput();

        // Spawn Tree Trunk
        if (checkHeight == 0.0f)
        {
            GameObject trunk = SpawnPrefab(blockCollection.treeTrunk, new Vector3(0, actualHeight, 0));
            trunk.transform.localScale = new Vector3(0.2f, actualHeight, 0.2f);

            GameObject treeTop = SpawnPrefab(blockCollection.treeTop, new Vector3(0, actualHeight * 2, 0));
            treeTop.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            lod = SpawnPrefab(LODTree, new Vector3(0, actualHeight, 0));
            lod.transform.localScale = new Vector3(actualHeight, actualHeight, actualHeight);

            currentHeight = branchStartHeight - heightPerBranch;

            TriggerNextSymbol();
        }
        else if (checkHeight < branchStartHeight && checkHeight <= actualHeight * 2)
        {
            TriggerNextSymbol();
        }
        // Spawn Branches per Stage
        else if (checkHeight <= actualHeight * 2)
        {
            for (int i = 0; i < branchCountPerStage; i++)
            {
                float t = (float)i / (float)branchCountPerStage;

                GameObject branch = SpawnPrefab(GetRandomBranch(), new Vector3(0, 0, 0), GetLoopedRotation(120, t));
                float applyScale = maxBranchScale - branchScaleStep * currentScaleStage;
                branch.transform.localScale = new Vector3(applyScale, applyScale, applyScale);

                Branch bObject = branch.GetComponent<Branch>();
                branch.transform.position = branch.transform.position + bObject.GetDirectionTo(transform.position) * bObject.GetDistanceTo(transform.position);
            }
            
            currentScaleStage++;
            TriggerNextSymbol();
        }
        // Create top of Pine Tree
        else if (checkHeight == actualHeight * 2 + heightPerBranch)
        {
            for (int i = 0; i < 8; i++)
            {
                SpawnBranch(i, 140);
            }

            TriggerNextSymbol();
        }
        else if (checkHeight == actualHeight * 2 + heightPerBranch * 2)
        {
            for (int i = 0; i < 8; i++)
            {
                SpawnBranch(i, 160);
            }

            LODReplacer replacer = Root.GetComponent<LODReplacer>();
            if (replacer == null) { replacer = Root.AddComponent<LODReplacer>(); }

            replacer.SetHideAllChildren(true);
            replacer.SetReplacement(lod);

            if (Application.isPlaying) { replacer.Setup(); }
            lod.SetActive(false);
        }
    }

    // Used for upper branches
    private void SpawnBranch(int i, float angle)
    {
        float t = (float)i / 8.0f;

        GameObject branch = SpawnPrefab(GetRandomBranch(), new Vector3(0, 0, 0), GetLoopedRotation(angle, t));
        float applyScale = maxBranchScale - branchScaleStep * currentScaleStage;
        branch.transform.localScale = new Vector3(applyScale, applyScale, applyScale);

        Branch bObject = branch.GetComponent<Branch>();
        branch.transform.position = branch.transform.position + bObject.GetDirectionTo(transform.position) * bObject.GetDistanceTo(transform.position);
    }

    private GameObject GetRandomBranch()
    {
        int index = RandomInt(0, 2);
        GameObject branch = blockCollection.branch1;

        switch (index)
        {
            case 1:
                {
                    branch = blockCollection.branch2;
                    break;
                }
            case 2:
                {
                    branch = blockCollection.branch3;
                    break;
                }
        }

        return branch;
    }

    private Quaternion GetLoopedRotation(float angle, float t)
    {
        t = Mathf.Repeat(t, 1f);

        Quaternion q0 = Quaternion.Euler(0, 0, -angle); 
        Quaternion q1 = Quaternion.Euler(-angle, 0, 0); 
        Quaternion q2 = Quaternion.Euler(0, 0, angle);  
        Quaternion q3 = Quaternion.Euler(angle, 0, 0); 

        if (t < 0.25f)
        {
            float segmentT = t / 0.25f;
            return Quaternion.Slerp(q0, q1, segmentT);
        }
        else if (t < 0.5f)
        {
            float segmentT = (t - 0.25f) / 0.25f;
            return Quaternion.Slerp(q1, q2, segmentT);
        }
        else if (t < 0.75f)
        {
            float segmentT = (t - 0.5f) / 0.25f;
            return Quaternion.Slerp(q2, q3, segmentT);
        }
        else
        {
            float segmentT = (t - 0.75f) / 0.25f;
            return Quaternion.Slerp(q3, q0, segmentT);
        }
    }

    private void CheckInput()
    {
        // Randomize or Clamp target height
        if (height < 0)
        {
            height = RandomInt(minHeight, maxHeight + 1);
            actualHeight = startHeight + (heightPerBranch * height);
        }
        else
        {
            height = Mathf.Clamp(height, minHeight, maxHeight);
            actualHeight = startHeight + heightPerBranch * height;
        }

        if (branchCountPerStage < 0) { branchCountPerStage = RandomInt(minBranchCount, maxBranchCount + 1); }
        else { branchCountPerStage = Mathf.Clamp(branchCountPerStage, minBranchCount, maxBranchCount); }

        float totalScaleValue = maxBranchScale - minBranchScale;
        float branchAmount = ((actualHeight * 2) - branchStartHeight) / heightPerBranch;
        branchScaleStep = totalScaleValue / branchAmount;
    }

    private void TriggerNextSymbol()
    {
        PineTree remainingBuilding = CreateSymbol<PineTree>("Stage", new Vector3(0, heightPerBranch, 0));
        remainingBuilding.Initialize(height, heightPerBranch, branchStartHeight, minHeight, maxHeight, 
            minBranchScale, maxBranchScale, currentHeight + heightPerBranch, currentScaleStage, lod,
            branchCountPerStage, minBranchCount, maxBranchCount, blockCollection, checkHeight + heightPerBranch);
        remainingBuilding.Generate(buildDelay);
    }

    public override void ResetDefault()
    {
        height = -1;
        branchCountPerStage = -1;

        LODReplacer replacer = GetComponent<LODReplacer>();
        if (replacer != null)
        {
            replacer.ResetValues();
        }
    }
}
