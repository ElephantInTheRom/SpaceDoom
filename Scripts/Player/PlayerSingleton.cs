using System;
using Godot;

public static class PlayerSingleton
{
    private static PackedScene PlayerScene { get; set; }
    public static KinematicBody2D PlayerInstance { get; private set; }
    public static Player PlayerScript { get; private set; }
    public static PlayerWeaponManager WeaponMgr { get { return PlayerScript.WeaponManager; } private set { } }

    static PlayerSingleton()
    {
        PlayerScene = ResourceLoader.Load<PackedScene>("res://Scenes/Player/Player.tscn");
        PlayerInstance = (KinematicBody2D)PlayerScene.Instance();
        PlayerScript = (Player)PlayerInstance;
    }
}