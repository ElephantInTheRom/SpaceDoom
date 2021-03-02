using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;

using SpaceDoom.Library;
using SpaceDoom.Library.Abstract;
using SpaceDoom.Library.Extensions;
using SpaceDoom.Systems.Combat;
using SpaceDoom.Systems.Movement;
using SpaceDoom.Scenes;

public class Player : KinematicBody2D, IAttacker
{
    //Stats
    [Export]
    private int Speed { get; set; }

    //Data
    public Physics2DDirectSpaceState SpaceState { get; private set; }

    //Scripts
    private AnimationController AnimationController { get; set; }
    public PlayerWeaponManager WeaponManager { get; private set; }
    
    //Events

    //Nodes
    private SceneBase CurrentSceneBase { get; set; }
    public YSort ProjectileLayer { get; private set; }
    private AnimatedSprite Sprite { get; set; }
    private Line2D TestLine { get; set; }

    //Godot methdos
    public override void _Ready()
    {
        base._Ready();

        //Initialize nodes
        CurrentSceneBase = GetNode<SceneBase>("/root/Main");
        ProjectileLayer = CurrentSceneBase.GetNode<YSort>("FriendlyProjectiles");
        Sprite = GetNode<AnimatedSprite>("Sprite");
        HitscanRaycast = GetNode<RayCast2D>("HitscanCast");
        TestLine = GetNode<Line2D>("Line2D");

        //Controllers
        AnimationController = new AnimationController(Sprite);
        WeaponManager = GetNode<PlayerWeaponManager>("WeaponManager");
        WeaponManager.SetLayers(ProjectileLayer, ProjectileLayer);

        //Initialize Movement
        DirectionsThisFrame = new DirectionQueue();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        RotateToLookAt(GetLocalMousePosition()); //Force the ship to face towards the mouse cursor
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        SpaceState = GetWorld2d().DirectSpaceState; //Set the space state

        ProcessMovement(delta);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        //Inputs that do not need to be checked every frame can be put here to save on performance
        if (Input.IsActionJustPressed("mouse_left")) { FireWeapon(); }
        if (Input.IsActionJustPressed("mouse_right")) { }
        if (Input.IsActionJustPressed("space")) { }
        if (Input.IsActionJustPressed("interact")) { }
        //Weapon hot keys
        if (Input.IsActionJustPressed("LaserGun")) { WeaponManager.EquipWeapon(WeaponID.pl_lsrgun); }
        if (Input.IsActionJustPressed("LaserBeam")) { WeaponManager.EquipWeapon(WeaponID.pl_lsrbeam); }
        if (Input.IsActionJustPressed("Grenade")) { WeaponManager.EquipWeapon(WeaponID.pl_grenade); }
        if (Input.IsActionJustPressed("Pulsar")) { WeaponManager.EquipWeapon(WeaponID.pl_pulsar); }
        if (Input.IsActionJustPressed("Crossbow")) { WeaponManager.EquipWeapon(WeaponID.pl_crossbow); }
        if (Input.IsActionJustPressed("Flamethrower")) { WeaponManager.EquipWeapon(WeaponID.pl_flamethrower); }
        if (Input.IsActionJustPressed("Shotgun")) { WeaponManager.EquipWeapon(WeaponID.pl_shotgun); }
    }

    // - - - Combat and targeting - - - \\
    public List<Weapon> EquippedWeapons { get; protected set; } 
    public RayCast2D HitscanRaycast { get; set; }

    //Returned from the damageable entity if it was sucessfully hit
    private void FireWeapon()
    {
        if (WeaponManager.SelectedWeapon.Loaded) 
        {
            WeaponManager.SelectedWeapon.FireWeapon(this, GetViewport().GetMousePosition());
        }
    }

    //Send out a complex raycast
    public IDamageable SendComplexCast(float range = 5000, float angleOffset = 0)
    {
        float theta = (RotationDegrees + angleOffset).NormalizeRotation();
        
        var destination = GlobalPosition.GetDistantPoint(theta + angleOffset, range);

        var result = SpaceState.IntersectRay(GlobalPosition, destination, new Godot.Collections.Array { this } 
                                             );

        Print(result);
        if(result.Count > 0)
        {
            foreach(var entry in result)
            {
                
            }
        }

        return null;
    }

    //Returned to this class when a damage event was successful
    public void ProcessCombatReply(CombatReply comReply)
    {
        
    }

    private void RotateToLookAt(Vector2 targetPos)
    {
        //Difference between Atan and Atan2 is Atan 2 allows for calculations across all quadrants
        //You must add this calculation to the rotation instead of setting it because once the ship is
        //facing the target the angle is 0
        //Rotation += GetLocalMousePosition().Angle();
        var theta = Math.Atan2(targetPos.y, targetPos.x) * (180 / Math.PI);
        RotationDegrees += (float)theta;
        //Correct the accuracy of the angle (this is to make sure it stays between 0 and 360)
        RotationDegrees = RotationDegrees.NormalizeRotation();
    }

    // - - - Movement & physics handling - - - \\
    private bool FrameMoving { get; set; } = false; //Are we moving in the current frame?
    private Direction FrameDirection { get; set; } //Which direction are we facing in the current frame?
    private DirectionQueue DirectionsThisFrame { get; set; }

    private void ProcessMovement(float delta)
    {
        FrameMoving = false;
        DirectionsThisFrame.Clear();
        //Realistically, you can only move 2 directions at a time.
        if (Input.IsActionPressed("move_up")) { DirectionsThisFrame.Enqueue(Direction.Up); }
        if (Input.IsActionPressed("move_down")) { DirectionsThisFrame.Enqueue(Direction.Down); }
        if (Input.IsActionPressed("move_left")) { DirectionsThisFrame.Enqueue(Direction.Left); }
        if (Input.IsActionPressed("move_right")) { DirectionsThisFrame.Enqueue(Direction.Right); }
        //If none of these return true this frame, it is okay to assume we are not moving.
        FrameDirection = DirectionsThisFrame.RealDirection;
        if(FrameDirection != Direction.Idle) 
        { 
            FrameMoving = true;
            //Move and collide along the vector translated from the direction, times speed, normalize with delta
            MoveAndCollide(Maps.DirVector[FrameDirection] * Speed * delta);
        }
        //Any extra movement abilities
        if (Input.IsActionJustPressed("dash")) { Dash(delta); }
    }

    private void Dash(float delta)
    {
        MoveAndCollide(Maps.DirVector[FrameDirection] * (Speed * 40) * delta);
    }
}