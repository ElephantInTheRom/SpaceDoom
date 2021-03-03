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
    private Vector2 CollisionPoint { get; set; }
    private bool EndingAtColPoint { get; set; }

    //Because this sprites rotation is offset by -90 degrees we correct it here
    public void SetDirection(Vector2 start, Vector2 target, bool endattarget)
    {
        //Get the velocity of the projectile
        Velocity = new Vector2(target.x - start.x, target.y - start.y);
        if (!Velocity.IsNormalized()) { Velocity = Velocity.Normalized(); }
        //Determine whether it will disspear once it hits an end target
        if (endattarget) { CollisionPoint = target; }
        EndingAtColPoint = endattarget;
        //Set its position
        Position = start;
        //Set rotation
        var relTarget = target.RelativeTo(start);
        var theta = Math.Atan2(relTarget.y, relTarget.x) * (180 / Math.PI);
        RotationDegrees = (float)theta + 90; //We have to add 90 to this angle because the laser is offset incorrectly
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        var step = Velocity * Speed * delta;
        Position += step;

        if (Visible && EndingAtColPoint)
        {
            //This statement checks to see if the bullet has passed the collision point on its velocity vector
            //TODO: study dot products
            //"DistanceBetweenPoints < .5 * Step &&" could be added if it is not garunteed the node will pass the point
            if ((Position - CollisionPoint).Dot(Velocity) > 0.1)
            {
                Visible = false;
            }
        }
    }

    public void DespawnTimeout()
    {
        QueueFree();
    }
}