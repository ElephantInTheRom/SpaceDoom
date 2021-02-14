using Godot;
using System;
using System.Collections.Generic;

using SpaceDoom.Systems.Combat;
using SpaceDoom.Library.Abstract;

//Script for a grenade projectile. 
//This grenade works by applying a force in a certain direction, and sending a combat event to anything in its explosion radius
public class GrenadeProjectile : RigidBody2D
{
    //Data
    private Vector2 StartPos { get; set; }
    private Vector2 Target { get; set; }
    private List<PhysicsBody2D> BodiesInRadius { get; set; }
    private float DetTime { get; set; }
    //Packages
    public CombatEvent _CombatEvent { get; private set; }
    //Nodes
    private Timer DetTimer { get; set; }

    public void SetData(Vector2 target, CombatEvent comEvt, float detTime)
    {
        Target = target;
        _CombatEvent = comEvt;
        StartPos = comEvt.Attacker.Position;
        DetTime = detTime;
    }

    public override void _Ready()
    {
        base._Ready();

        BodiesInRadius = new List<PhysicsBody2D>();
        DetTimer = GetNode<Timer>("DetTimer");

        Position = StartPos;
        Push();
        DetTimer.Start(DetTime);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

    // - - - Behaviors - - - 
    private void Push()
    {
        LinearVelocity = Target.Normalized() * 300;
        GD.Print(LinearVelocity);
    }

    private void Detonate()
    {
        foreach(var body in BodiesInRadius)
        {
            var target = body as IDamageable;
            target.ProcessCombatEvent(_CombatEvent);
        }
        //Play explosion effects
        QueueFree();
    }

    // - - - Signals - - - 
    public void RadiusBodyEntered(PhysicsBody2D body)
    {
        if(body is IDamageable) { BodiesInRadius.Add(body); }
    }

    public void RadiusBodyExited(PhysicsBody2D body)
    {
        if(body is IDamageable && BodiesInRadius.Contains(body)) { BodiesInRadius.Remove(body); }
    }

    public void OnDetonationTimeout() => Detonate();

    public void OnBodyCollision(PhysicsBody2D body) 
    {
        //We cant scan for specific collision layers yet so this will have to do
        if(body.CollisionLayer != CollisionLayer) { Detonate(); }
    }
}