using Godot;
using System;

using SpaceDoom.Scenes;
using SpaceDoom.Library.Abstract;

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
    [Export] private int EnemyMax { get; set; } = 15;

    public override void _Ready()
    {
        base._Ready();

        TestTimer = GetNode<Timer>("Timer");
        TestLabel = GetNode<Label>("TestLabel");

        GUIScene = GD.Load<PackedScene>("res://Scenes/Player/WeaponOverlay.tscn");
        EnemyScene = GD.Load<PackedScene>("res://Scenes/Entities/Enemy_Bee.tscn");

        LoadPlayer(new Vector2(1024, 800));

        for(var e = EnemyMax; e > 0; e--)
        {
            CreateEnemy();
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        TestLabel.Text = Engine.GetFramesPerSecond().ToString();

        if (Input.IsActionJustPressed("Fullscreen")) { OS.WindowFullscreen = !OS.WindowFullscreen; }
    }

    //Spawning enemies for combat testing
    public void CreateEnemy()
    {
        if(EnemyCount >= EnemyMax) { return; }
        var offset = new Vector2(128 + rng.Next(0, 1664), 128 + rng.Next(0, 448));
        var instance = EnemyScene.Instance();
        var script = instance as Enemy;
        script.EnemyDied += EnemyDown;
        LoadChildAt(script, offset);
        EnemyCount++;
    }

    public void EnemyDown() => EnemyCount--;
}