using Godot;
using System;

public class HitscanProjectile : AnimatedSprite
{
    //Hitscan projectiles need to travel along a given vector quickly and dissapear after its timer runs out

    //Data
    private float Speed { get; set; } = 3500;
    private Vector2 Velocity { get; set; }

    //Because this sprites rotation is offset by -90 degrees we correct it here
    public void SetDirection(Vector2 start, Vector2 target)
    {
        Velocity = new Vector2(target.x - start.x, target.y - start.y);
        if (!Velocity.IsNormalized()) { Velocity = Velocity.Normalized(); }

        Position = start;
        RotationDegrees = target.Angle();
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