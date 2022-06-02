using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeButton : Interactable
{

    public int color;
    public Arcade arcade;

    private Color[] colors = { new Color(.5f,0,0), new Color(0, .5f, 0), new Color(0, 0, .5f) };
    private Color[] colorsHighlite = { new Color(1, 0, 0), new Color(0, 1, 0), new Color(0, 0, 1) };

    public override void hover(Transform player)
    {
        GetComponent<MeshRenderer>().material.color = colorsHighlite[color];
    }

    public void Update()
    {
        GetComponent<MeshRenderer>().material.color = colors[color];
    }

    public override void interact()
    {
        arcade.Rotate(color);
    }
}
