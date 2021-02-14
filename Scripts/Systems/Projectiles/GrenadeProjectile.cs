using Godot;
using System;

using SpaceDoom.Systems.Combat;

public class GrenadeProjectile : RigidBody2D
{
    //Data
    private Vector2 Target { get; set; }
    //Packages
    public CombatEvent _CombatEvent { get; private set; }
    //Nodes
    private Timer DetTimer { get; set; }
}