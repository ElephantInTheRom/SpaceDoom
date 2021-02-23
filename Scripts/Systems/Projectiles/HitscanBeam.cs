using Godot;
using System;
using SpaceDoom.Library.Extensions;

public class HitscanBeam : TextureRect
{
    //Beam projectiles need to appear along a vector and then slowly fade out
    //hitscan damage will be held in script by the weapon that fired it.
    public void SetDirection(Vector2 start, Vector2 target)
    {
        RectPosition = start;
        //Set rotation
        var relTarget = target.RelativeTo(start);
        var theta = Math.Atan2(relTarget.y, relTarget.x) * (180 / Math.PI);
        RectRotation = (float)theta;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        //Make more transparent as time goes on
        Modulate = new Color(Modulate, (float)(Modulate.a - .01));

        if(Modulate.a <= 0) { QueueFree(); }
    }
}