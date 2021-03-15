using Godot;
using System;
using System.Collections.Generic;


namespace SpaceDoom.Systems.Combat
{
    //Script that controls the weapon overlay for the player
    public class WeaponOverlay : Control
    {
        //Nodes
        private TextureRect WeaponIcon { get; set; }
        private TextureProgress LoadProgress { get; set; }
        //Data
        private PlayerWeaponManager RunningWeaponManager { get; set; }
        //Maps
        private static Dictionary<WeaponState, string> ColorMap { get; set; } //Maps a state to a rich text color tag
        = new Dictionary<WeaponState, string>()
        {
            {WeaponState.ready, "green" },
            {WeaponState.reloading, "yellow" },
            {WeaponState.firing, "red" },
            {WeaponState.disabled, "gray" }
        };
        private Dictionary<WeaponID, RichTextLabel> WeaponMap { get; set; } //Maps each weapon to its text display

        //Godot Methods
        public override void _Ready()
        {
            base._Ready();

            RunningWeaponManager = PlayerSingleton.PlayerScript.WeaponManager;

            WeaponMap = new Dictionary<WeaponID, RichTextLabel>()
        {
            { WeaponID.pl_lsrgun, GetNode<RichTextLabel>("WeaponList/LaserGun") },
            { WeaponID.pl_lsrbeam, GetNode<RichTextLabel>("WeaponList/LaserBeam") },
            { WeaponID.pl_grenade, GetNode<RichTextLabel>("WeaponList/Grenade") },
            { WeaponID.pl_pulsar, GetNode<RichTextLabel>("WeaponList/Pulsar") },
            { WeaponID.pl_crossbow, GetNode<RichTextLabel>("WeaponList/Crossbow") },
            { WeaponID.pl_flamethrower, GetNode<RichTextLabel>("WeaponList/Flamethrower") },
            { WeaponID.pl_shotgun, GetNode<RichTextLabel>("WeaponList/Shotgun") }
        }; //This must be instantiated here because of getnode

            UpdateAll();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            UpdateText();
            UpdateProgress();
        }

        //Methods used to update parts of the display
        public void UpdateAll()
        {
            UpdateText();
            UpdateIcon();
            UpdateProgress();
        }

        public void UpdateText()
        {
            foreach (var wep in RunningWeaponManager.EquippedWeapons)
            {
                var line = WeaponMap[wep.ID];

                if (wep == RunningWeaponManager.SelectedWeapon)
                {
                    line.BbcodeText = $"[b][color={ColorMap[wep.State]}]{wep.Name}[/color][/b]";
                }
                else
                {
                    line.BbcodeText = $"[color={ColorMap[wep.State]}]{wep.Name}[/color]";
                }

                if (wep.State == WeaponState.disabled) { line.Visible = false; }
                if (line.Visible == false && wep.State != WeaponState.disabled) { line.Visible = true; }
            }
        }

        public void UpdateProgress()
        {
            var remaining = RunningWeaponManager.SelectedWeapon.CooldownTimer.TimeLeft;
            //If there is no time left, skip these calculations
            if (remaining == 0)
            {
                LoadProgress.Value = 100;
                LoadProgress.TintProgress = new Color(0, 255, 0);
            }
            else
            {
                var total = RunningWeaponManager.SelectedWeapon.CooldownTime;
                var progress = total - remaining;
                var progressPercent = (progress * 100) / total;

                LoadProgress.Value = progressPercent;

                //Even though this is the use case for a switch, we are using older than C# 7 and cannot use relational patterns
                if (progressPercent > 66) { LoadProgress.TintProgress = new Color(0, 255, 0); }
                else if (progressPercent > 33) { LoadProgress.TintProgress = new Color(255, 255, 0); }
                else if (progressPercent < 33) { LoadProgress.TintProgress = new Color(255, 0, 0); }
            }
        }

        public void UpdateIcon()
        {
            GD.Print("Icon updated lol");
        }
    }
}