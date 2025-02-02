using System;
using Godot;
using SpaceDoom.Library.Extensions;
using SpaceDoom.Library.Abstract;
using SpaceDoom.Systems.Combat;

namespace SpaceDoom.Enemies
{
    public class Enemy_Fly : Enemy, IAttacker
    {
        //Nodes
        private Tween PointLabelTween { get; set; }
        private Label PointsLabel { get; set; }
        private Timer AttackTimer { get; set; }
        //Data
        public bool Dead { get; protected set; } = false;
        private Random rng { get; set; }
        //Path

        //Godot methods
        public override void _Ready()
        {
            base._Ready();

            rng = new Random();

            Sprite = GetNode<AnimatedSprite>("Sprite");
            PointLabelTween = GetNode<Tween>("PointLabel/Tween");
            PointsLabel = GetNode<Label>("PointLabel");
            AttackTimer = GetNode<Timer>("AttackTimer");

            ProjectileScene = GD.Load<PackedScene>("res://Scenes/Projectiles/EnemyLaser.tscn");
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
        }

        //Init
        public void InjectData(float attDelay, float bulletSpeed)
        {
            AttackDelay = attDelay;
            BulletSpeed = bulletSpeed;
            AttackTimer.Start(attDelay);
        }

        // - - - Signals - - - \\
        public void AttackTimeout()
        {
            //Decide whether or not to attack the player (TODO: migrate this into a state machine!)
            var bias = (rng.NextDouble()+ AggressionBias).Clamp(0, 1);
            if(bias > 0.7 && CanSeePlayer()) 
            {
                AttackPlayer();
            }
        }

        // - - - Targeting and Attacking - - - \\
        [Export] public double AggressionBias { 
            get { return AggressionBias; } 
            set { AggressionBias = value.Clamp(0, 1); } 
        } //How aggresive the enemy is, from zero to one
        [Export] public float AttackDelay { get; private set; } //How often the enemy will try to attack
        [Export] public float BulletSpeed { get; private set; } //How fast its attacks can go
        [Export] public int Damage { get; private set; } //How much damage the enemy does
        private PackedScene ProjectileScene { get; set; }
        private CombatEvent _CombatEvent { get; set; }

        public override void ProcessCombatEvent(CombatEvent comEvent)
        {
            if (!Dead) { base.ProcessCombatEvent(comEvent); }
        }

        public void ProcessCombatReply(CombatReply comReply)
        {
            throw new NotImplementedException();
        }

        private void AttackPlayer()
        {
            var instance = ProjectileScene.Instance();
            var instScript = instance as EnemyLaser;
            instScript.InjectData(new CombatEvent(Damage, this), GlobalPosition, 
                                    PlayerSingleton.PlayerInstance.GlobalPosition, BulletSpeed);
        }

        // - - - Behavior - - - \\
        public override void TriggerDeath(int finalBlow)
        {
            Dead = true;

            PointsLabel.Show();
            PointLabelTween.InterpolateProperty(PointsLabel, "rect_position", null, new Vector2(-16, 150), 4);

            Sprite.Play("death");

            _invokeDealthAction(finalBlow);
        }

        public void SpriteAnimationFinished()
        {
            if (Sprite.Animation == "death")
            {
                QueueFree();
            }
        }
    }
}