using Godot;
using System;
using SpaceDoom.Library.Extensions;

public class HitscanProjectile : AnimatedSprite
{
    //Hitscan projectiles need to travel along a given vector quickly and dissapear after its timer runs out

    //Data
    [Export]
    private float Speed { get; set; }

    private Vector2 Velocity { get; set; }

    //Because this sprites rotation is offset by -90 degrees we correct it here
    public void SetDirection(Vector2 start, Vector2 target)
    {
        Velocity = new Vector2(target.x - start.x, target.y - start.y);
        if (!Velocity.IsNormalized()) { Velocity = Velocity.Normalized(); }

        Position = start;
        //Set rotation
        var relTarget = target.RelativeTo(start);
        var theta = Math.Atan2(relTarget.y, relTarget.x) * (180 / Math.PI);
        RotationDegrees = (float)theta + 90; //We have to add 90 to this angle because the laser is offset incorrectly
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        Position += Velocity * Speed * delta;
    }

    public void DespawnTimeout()
    {
        QueueFree();
    }
}