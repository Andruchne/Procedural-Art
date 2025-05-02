using Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoof : Shape
{
    // shape parameters:
    int Width;
    int Depth;

    GameObject[] roofStyle;

    // (offset) values for the next layer:
    int newWidth;
    int newDepth;

    public void Initialize(int Width, int Depth, GameObject[] roofStyle)
    {
        this.Width = Width;
        this.Depth = Depth;
        this.roofStyle = roofStyle;
    }


    protected override void Execute()
    {
        if (Width == 0 || Depth == 0) return;

        newWidth = Width;
        newDepth = Depth;

        CreateFlatRoofPart();
        //CreateNextPart();
    }

    void CreateFlatRoofPart()
    {
        SimpleRow flatRoof;
        
        flatRoof = CreateSymbol<SimpleRow>("roofStrip", new Vector3(0, 0, -0.5f), Quaternion.Euler(new Vector3(0, 90, 0)));
        flatRoof.Initialize(Width, roofStyle);
        flatRoof.Generate();
        

        /*
        // Randomly create two roof strips in depth direction or in width direction:
        int side = RandomInt(2);

        switch (side)
        {
            // Add two roof strips in depth direction
            case 0:
                for (int i = 0; i < 2; i++)
                {
                    flatRoof = CreateSymbol<SimpleRow>("roofStrip", new Vector3((Width - 1) * (i - 0.5f), 0, 0));
                    flatRoof.Initialize(Depth, roofStyle);
                    flatRoof.Generate();
                }
                newWidth -= 2;
                break;
            // Add two roof strips in width direction
            case 1:
                for (int i = 0; i < 2; i++)
                {
                    flatRoof = CreateSymbol<SimpleRow>("roofStrip", new Vector3(0, 0, (Depth - 1) * (i - 0.5f)));
                    flatRoof.Initialize(Width, roofStyle, new Vector3(1, 0, 0));
                    flatRoof.Generate();
                }
                newDepth -= 2;
                break;
        }
        */
    }

    void CreateNextPart()
    {
        // randomly continue with a roof or a stock:
        if (newWidth <= 0 || newDepth <= 0) return;

        NormalRoof nextRoof = CreateSymbol<NormalRoof>("roof");
        nextRoof.Initialize(newWidth, newDepth, roofStyle);
        nextRoof.Generate(buildDelay);
    }
}
