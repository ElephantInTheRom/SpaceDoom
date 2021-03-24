 using Godot;
using System;

using SpaceDoom.Library.Abstract;

namespace SpaceDoom
{
    //The class that controls the behavior of an entities healthbar
    public class Healthbar : TextureProgress
    {
        //Nodes
        private Tween TweenAnimator { get; set; }

        //Health Data
        public double MaxHealth
        {
            get { return MaxValue; }
            set { MaxValue = value; }
        }

        public double CurrentHealth 
        { 
            get { return Value; } 
            set { Value = value; } //Now this is epic
        }

        private Color DefaultPTint { get; set; }

        //Godot methods
        public override void _Ready()
        {
            DefaultPTint = new Color(0, 210, 0, 255);
            TweenAnimator = GetNode<Tween>("Interpolater");

            base._Ready();
        }

        //InjectData (setup) Method
        public void InjectData(int maxHealth, int currentHealth)
        {
            MaxValue = maxHealth;
            Value = currentHealth;
        }

        public void InjectData(int maxHealth, int currentHealth, Enemy pairedEnemy)
        {
            MaxValue = maxHealth;
            Value = currentHealth;
            pairedEnemy.HealthChanged += HealthChanged;
        }

        //Methods to change health
        public void HealthChanged(int difference)
        {
            var previous = Value;
            //If the change in health is less than 0, set to 0. Otherwise calculate difference.
            var current = (Value + difference < 0) ? 0 : Value += difference;
            //This tween creates a smooth animation for the health bar.
            TweenAnimator.InterpolateProperty(this, "value", previous, current, 1f, delay: .3f);
        }
    }
}