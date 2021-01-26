using Godot;
using System;

using SpaceDoom.Library;

public class Player : KinematicBody2D
{
    //Data
    [Export]
    private int Speed { get; set; }
    //Scripts
    private AnimationController AnimationController { get; set; }
    //Events

    //Nodes
    private AnimatedSprite Sprite { get; set; }


    //Godot methdos
    public override void _Ready()
    {
        base._Ready();

        Sprite = GetNode<AnimatedSprite>("Sprite");

        AnimationController = new AnimationController(Sprite);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        ProcessInput(delta); //Check for all inputs in the given frame

        RotateToLookAt(GetViewport().GetMousePosition()); //Force the ship to face towards the mouse cursor
    }

    // - - - Methods for pointing and targeting - - - \\
    private void RotateToLookAt(Vector2 target)
    {
        Vector2 hypotenuse;
        hypotenuse.x = Position.x - target.x;
        hypotenuse.y = Position.y - target.y;

        float angleToTarget = Mathf.Atan2(hypotenuse.y, hypotenuse.x);
        RotationDegrees = (angleToTarget * (180f / Mathf.Pi)) - 90f;
    }

    // - - - Movement & physics handling - - - \\
    private bool Moving { get; set; } = false;

    private void ProcessInput(float delta)
    {
        Moving = false;
        if (Input.IsActionPressed("move_up")) { ProcessMovement(Direction.Up, delta); }
        if (Input.IsActionPressed("move_down")) { ProcessMovement(Direction.Down, delta); }
        if (Input.IsActionPressed("move_left")) { ProcessMovement(Direction.Left, delta); }
        if (Input.IsActionPressed("move_right")) { ProcessMovement(Direction.Right, delta); }
        //If none of these return true this freame, it is okay to assume we are not moving.
        if (Input.IsActionPressed("mouse_left")) { }
        if (Input.IsActionPressed("mouse_right")) { }
        if (Input.IsActionPressed("space")) { }
        if (Input.IsActionPressed("interact")) { }
    }

    private void ProcessMovement(Direction dir, float delta)
    {
        Moving = true;
        //Move and collide along the vector translated from the direction, times speed, normalize with delta
        MoveAndCollide(Maps.DirVector[dir] * Speed * delta);
    }
}
