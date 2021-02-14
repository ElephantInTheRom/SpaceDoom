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
    private Vector2 Velocity { get; set; }
    private List<PhysicsBody2D> BodiesInRadius { get; set; }
    private float DetTime { get; set; }
    private bool Exploded { get; set; } = false;
    //Packages
    public CombatEvent _CombatEvent { get; private set; }
    //Nodes
    private Timer DetTimer { get; set; }
    private AnimatedSprite Sprite { get; set; }
    private Particles2D ExplosionParticles { get; set; }

    public void SetData(Vector2 target, CombatEvent comEvt, float detTime)
    {
        _CombatEvent = comEvt;
        StartPos = comEvt.Attacker.Position;
        DetTime = detTime;

        Velocity = new Vector2(target.x - StartPos.x, target.y - StartPos.y);
        if (!Velocity.IsNormalized()) { Velocity = Velocity.Normalized(); }
    }

    public override void _Ready()
    {
        base._Ready();

        BodiesInRadius = new List<PhysicsBody2D>();
        Sprite = GetNode<AnimatedSprite>("Sprite");
        DetTimer = GetNode<Timer>("DetTimer");
        ExplosionParticles = GetNode<Particles2D>("ExplosionParticles");

        Position = StartPos;
        Push();
        DetTimer.Start(DetTime);
    }

    public override void _Process(float delta)
    {
        if(Exploded && ExplosionParticles.Emitting == false) { QueueFree(); }
    }

    // - - - Behaviors - - - 
    private void Push()
    {
        AppliedForce = Velocity * 50;
    }

    private void Detonate()
    {
        foreach(var body in BodiesInRadius)
        {
            var target = body as IDamageable;
            target.ProcessCombatEvent(_CombatEvent);
        }
        //Play explosion effects
        Sprite.Hide();
        AppliedForce = Vector2.Zero;
        ExplosionParticles.Emitting = true;
        Exploded = true;
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
        GD.Print("Body collided!");
        //We cant scan for specific collision layers yet so this will have to do
        if(body.CollisionLayer != CollisionLayer) { Detonate(); }
    }
}