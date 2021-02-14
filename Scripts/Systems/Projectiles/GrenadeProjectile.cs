using Godot;
using System;
using System.Collections.Generic;

using SpaceDoom.Systems.Combat;

//Script for a grenade projectile. 
//This grenade works by applying a force in a certain direction, and sending a combat event to anything in its explosion radius
public class GrenadeProjectile : RigidBody2D
{
    //Data
    private Vector2 StartPos { get; set; }
    private Vector2 Target { get; set; }
    private List<PhysicsBody2D> BodiesInRadius { get; set; }
    //Packages
    public CombatEvent _CombatEvent { get; private set; }
    //Nodes
    private Timer DetTimer { get; set; }

    public void SetData(Vector2 target, CombatEvent comEvt)
    {
        Target = target;
        _CombatEvent = comEvt;
        StartPos = comEvt.Attacker.Position;
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

    // - - - Behaviors - - - 


    // - - - Signals - - - 

}