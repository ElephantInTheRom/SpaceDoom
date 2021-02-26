using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;
using SpaceDoom.Library.Extensions;
using SpaceDoom.Library.Abstract;
using SpaceDoom.Systems.Combat;

public class BeamProjectile : TextureRect
{
    //Beam projectiles need to appear along a vector and then slowly fade out
    //The amount of damage delivered should fall off with the fading out of the beam
    //Damage gets delivered to entities that collide with its area. 

    public CombatEvent _CombatEvent { get; private set; }

    //Sets the shape of the beam
    public void SetBeam(Vector2 start, Vector2 target, CombatEvent combatEvent)
    {
        _CombatEvent = combatEvent;
        RectPosition = start;
        //Set rotation
        var relTarget = target.RelativeTo(start);
        var theta = Math.Atan2(relTarget.y, relTarget.x) * (180 / Math.PI);
        RectRotation = (float)theta;
    }

    //Fades the node out over time
    public override void _Process(float delta)
    {
        base._Process(delta);

        //Make more transparent as time goes on
        Modulate = new Color(Modulate, (float)(Modulate.a - .02));

        if(Modulate.a <= 0) { QueueFree(); }
    }

    //Signals for bodies colliding
    public void BodyEntered(Node body)
    {
        if(body is IDamageable && Modulate.a > .5)
        {
            var hit = body as IDamageable;
            hit.ProcessCombatEvent(_CombatEvent);            
        }
    }

    public void BodyExited(Node body)
    {

    }
}