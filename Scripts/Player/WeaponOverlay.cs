using Godot;
using System;
using System.Collections.Generic;

using SpaceDoom.Systems.Combat;

//Script that controls the weapon overlay for the player
public class WeaponOverlay : Control
{
    //Nodes
    private RichTextLabel Weaponlist { get; set; }
    private AnimatedSprite WeaponIcon { get; set; }
    //Data
    private Dictionary<WeaponState, string> ColorMap { get; set; } //Maps a state to a rich text color tag
    private PlayerWeaponManager RunningWeaponManager { get; set; }
    
    public override void _Ready()
    {
        base._Ready();

        Weaponlist = GetNode<RichTextLabel>("WeaponSelectLabel");
        WeaponIcon = GetNode<AnimatedSprite>("SelectedWeaponSprite");

        RunningWeaponManager = PlayerSingleton.PlayerScript.WeaponManager;

        ColorMap = new Dictionary<WeaponState, string>() 
        {
            {WeaponState.ready, "green" },
            {WeaponState.reloading, "yellow" },
            {WeaponState.firing, "red" },
            {WeaponState.disabled, "gray" }
        };
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        Weaponlist.BbcodeText = ConstructBbcode();
    }

    private string ConstructBbcode()
    {
        string bbcode = "";
        foreach(var wpn in RunningWeaponManager.EquippedWeapons)
        {
            if(wpn == RunningWeaponManager.SelectedWeapon)
                bbcode += $"[u][color={ColorMap[wpn.State]}]>{wpn.Name}[/color][/u]\n";
            else
                bbcode += $"[color={ColorMap[wpn.State]}]{wpn.Name}[/color]\n";
        }
        return bbcode;
    }
}