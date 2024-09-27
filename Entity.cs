﻿using SFML.Graphics;
using SFML.System;

namespace Pacman
{
    class Entity
    {
        public bool Dead;

        protected Sprite sprite;

        private string textureName;

        protected Entity(string textureName)
        {
            this.textureName = textureName;
            sprite = new Sprite();
        }
        
        public Vector2f Position
        {
            get => sprite.Position;
            set => sprite.Position = value;
        }

        public virtual FloatRect Bounds => sprite.GetGlobalBounds();
        
        public virtual bool Solid => false;
        
        public virtual void Create(Scene scene)
        {
            sprite.Texture = scene.Assets.LoadTexture(textureName);
        }

        public virtual void Destroy(Scene scene) {}

        public virtual void Update(Scene scene, float deltaTime)
        {
            foreach (Entity found in scene.FindIntersects(Bounds))
            {
                CollideWith(scene, found);
            }
        }
        
        public virtual void Render(RenderTarget target)
        {
            target.Draw(sprite);
        }

        protected virtual void CollideWith(Scene scene, Entity other) {}
    }
}
