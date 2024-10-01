﻿using SFML.Graphics;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public delegate void ValueChangedEvent(Scene scene, int value);
    public delegate void BoolChangedEvent(Scene scene, bool value);

    public sealed class EventManager
    {
        public event ValueChangedEvent GainScore;
        public event ValueChangedEvent LoseHealth;
        public event ValueChangedEvent EatCandy;
        public event BoolChangedEvent GameOver;
        private bool isGameOver;
        
        private int scoreGained;
        private int healthLost;
        private int candyEaten;
        
        public void PublishGainScore(int amount) => scoreGained += amount;
        public void PublishLoseHealth(int amount) => healthLost += amount;
        public void PublishEatCandy(int amount) => candyEaten += amount;
        public void PublishGameOver(bool state) => isGameOver = state;

        public void HandleEvents(Scene scene)
        {
            if (scoreGained != 0)
            {
                GainScore?.Invoke(scene, scoreGained);
                scoreGained = 0;
            }
            
            if (healthLost != 0)
            {
                LoseHealth?.Invoke(scene, healthLost);
                healthLost = 0;
            }
            
            if (candyEaten != 0)
            {
                EatCandy?.Invoke(scene, candyEaten);
                candyEaten = 0;
            }
            
            GameOver?.Invoke(scene, isGameOver);
        }
    }
}
