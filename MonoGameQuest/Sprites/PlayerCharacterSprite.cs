using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Sprites
{
    public abstract class PlayerCharacterSprite
    {
        readonly IDictionary<AnimationIdentifier, Animation> _animations;
        Animation _currentAnimation;
        int _currentAnimationIndex;
        int _currentAnimationTimeAtIndex;
        private readonly int _height;
        private readonly int _offsetX;
        private readonly int _offsetY;
        private readonly Texture2D _spritesheet;
        private readonly int _width;

        protected PlayerCharacterSprite(
            Texture2D spritesheet,
            int height,
            int width,
            int offsetX,
            int offsetY)
        {
            _spritesheet = spritesheet;
            _height = height;
            _width = width;
            _offsetX = offsetX;
            _offsetY = offsetY;

            _animations = new Dictionary<AnimationIdentifier, Animation>();
        }

        protected void AddAnimation(Animation animation)
        {
            if (animation == null)
                throw new ArgumentNullException("animation");

            if (animation.AnimationId == AnimationIdentifier.None)
                throw new ArgumentException("Animation does not have an identifier.", "animation");

            if (_animations.ContainsKey(animation.AnimationId))
                throw new ArgumentException("An animation with that identifier has already been added.", "animation");

            _animations.Add(animation.AnimationId, animation);
        }

        public void Draw(
            SpriteBatch spriteBatch,
            Vector2 position,
            int tileHeight,
            int tileWidth)
        {
            if (_currentAnimation == null)
                return;

            var sourceRectangle = new Rectangle(
                _currentAnimationIndex * _width,
                _currentAnimation.Row * _height,
                _width,
                _height);

            var computedPosition = new Vector2(
                (position.X * tileWidth) + _offsetX,
                (position.Y * tileHeight) + _offsetY);

            spriteBatch.Draw(_spritesheet, computedPosition, sourceRectangle, Color.White);
        }

        protected void ResetAnimation()
        {
            _currentAnimationIndex = 0;
            _currentAnimationTimeAtIndex = 0;
        }

        public void SetAnimation(AnimationIdentifier animationId)
        {
            if (animationId == AnimationIdentifier.None)
                _currentAnimation = null;
            else if (!_animations.ContainsKey(animationId))
                throw new ArgumentException("Sprite does not have the specified animation.", "animationId");
            else
                _currentAnimation = _animations[animationId];

            ResetAnimation();
        }

        public void Update(GameTime gameTime)
        {
            if (_currentAnimation == null)
                return;

            _currentAnimationTimeAtIndex += gameTime.ElapsedGameTime.Milliseconds;
            if (_currentAnimationTimeAtIndex > _currentAnimation.Speed)
            {
                _currentAnimationTimeAtIndex = 0;
                _currentAnimationIndex++;
                if (!(_currentAnimationIndex < _currentAnimation.Length))
                    _currentAnimationIndex = 0;
            }
        }
    }
}
