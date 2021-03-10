using Godot;
using System;

using SpaceDoom.Scenes;

public class CombatTestScene : SceneBase
{
    //Scenes
    private PackedScene GUIScene { get; set; }
    private PackedScene EnemyScene { get; set; }
    //Nodes
    private Timer TestTimer { get; set; }
    private Label TestLabel { get; set; }
    private AudioStreamPlayer MusicBackground { get; set; }
    //Data
    private Random rng = new Random();
    private int EnemyCount { get; set; } = 0;

    public override void _Ready()
    {
        base._Ready();


    }

    public override void _Process(float delta)
    {
        base._Process(delta);


    }
}