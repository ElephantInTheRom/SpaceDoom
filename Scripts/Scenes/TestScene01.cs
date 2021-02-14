using Godot;
using System;

using SpaceDoom.Scenes;

public class TestScene01 : SceneBase
{
    //Scenes
    private PackedScene PlayerScene { get; set; }
    private PackedScene EnemyScene { get; set; }
    //Data
    private Random rng = new Random();
    private int EnemyCount { get; set; } = 0;

    public override void _Ready()
    {
        base._Ready();

        PlayerScene = ResourceLoader.Load<PackedScene>("res://Scenes/Entities/Player.tscn");
        EnemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Entities/Enemy_Bee.tscn");

        HitscanLayer = GetNode<YSort>("HitscanLayer");

        LoadChildAt((KinematicBody2D)PlayerScene.Instance(), new Vector2(600, 400));
    }

    //Code for spawning enemy at a testing nodes
    private void SpawnEnemy()
    {
        Node2D spawner = GetNode<Node2D>($"EnemySpawnPositons/node{rng.Next(1,5)}");
        Vector2 offset = new Vector2(rng.Next(-100, 100), rng.Next(-100, 100));
        LoadChildAt((KinematicBody2D)EnemyScene.Instance(), spawner.Position + offset);
    }
}
