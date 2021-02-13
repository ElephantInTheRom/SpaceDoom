using Godot;
using System;

using SpaceDoom.Scenes;

public class TestScene01 : SceneBase
{
    private PackedScene PlayerScene { get; set; }
    private PackedScene EnemyScene { get; set; }

    public override void _Ready()
    {
        base._Ready();

        PlayerScene = ResourceLoader.Load<PackedScene>("res://Scenes/Entities/Player.tscn");
        EnemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Entities/Enemy.tscn");

        HitscanLayer = GetNode<YSort>("HitscanLayer");

        LoadChildAt((KinematicBody2D)PlayerScene.Instance(), new Vector2(600, 400));
        LoadChildAt((KinematicBody2D)EnemyScene.Instance(), new Vector2(620, 80));
    }
}
