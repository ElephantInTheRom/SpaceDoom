using Godot;
using System;

//The class that controls the behavior of an entities healthbar
public class Healthbar : Polygon2D
{
    //Nodes
    private Polygon2D Overlay { get; set; }
    //Data
    private float MaxHealth { get; set; }
    private float CurrentHealth { get; set; }

    //Setting data
    public override void _Ready()
    {
        base._Ready();

        Overlay = GetNode<Polygon2D>("Overlay");
    }

    public void SetData(int maxHealth, int currentHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        UpdateHealthBar();
    }

    public void HealthChanged(int difference)
    {
        CurrentHealth += difference;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() => Overlay.Scale = new Vector2(CurrentHealth / MaxHealth, 1);
}