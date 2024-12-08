using BombMan.Source.Components.GamePlay.Items;
using BombMan.Source.Core.IO;
using BombMan.Source.Core.Shared;
using Microsoft.Xna.Framework;
using System;

namespace BombMan.Source.Components.GamePlay.Characters.Heroes
{
    public class Hero(Vector2 initialPosition, int width, int height, float speed, int health) : DynamicObject(initialPosition, width, height, speed)
    {

        public event Action<Vector2, Hero> OnPlaceBomb;
        public Bomb LastPlacedBomb { get; set; }

        public int Health { get; set; } = health;
        private readonly int _imageHeight = 26;
        private readonly int _imageWidth = 18;
        private TimeSpan _collisionCooldown = TimeSpan.Zero;
        private const float CollisionCooldownDuration = 1f; // 1 second cooldown

        // Enum for hero states
        private enum HeroState
        {
            MovingDownRightFoot,
            MovingDownNeutral,
            MovingDownLeftFoot,
            MovingRightRightFoot,
            MovingRightNeutral,
            MovingRightLeftFoot,
            MovingUpRightFoot,
            MovingUpNeutral,
            MovingUpLeftFoot,
            MovingLeftRightFoot,
            MovingLeftNeutral,
            MovingLeftLeftFoot
        }

        private HeroState _currentState = HeroState.MovingDownNeutral;
        private TimeSpan _idleTime = TimeSpan.Zero;
        private const float IdleThreshold = 3f; // 3 seconds
        private TimeSpan _animationTime = TimeSpan.Zero;
        private const float AnimationInterval = 0.2f; // 200 milliseconds

        public override void LoadContent()
        {
            Texture = Art.HeroImages;
        }

        // Update method with controller parameter
        public void Update(Controller controller)
        {
            base.Update();

            // Decrement the collision cooldown timer
            if (_collisionCooldown > TimeSpan.Zero)
            {
                _collisionCooldown -= Resource.UpdateGameTime.ElapsedGameTime;
            }

            // Handle input
            Vector2 input = Vector2.Zero;

            if (controller.IsUpPressed() || Resource.InputManager.IsMoveUp()) input.Y -= 1;
            if (controller.IsDownPressed() || Resource.InputManager.IsMoveDown()) input.Y += 1;
            if (controller.IsLeftPressed() || Resource.InputManager.IsMoveLeft()) input.X -= 1;
            if (controller.IsRightPressed() || Resource.InputManager.IsMoveRight()) input.X += 1;

            // Detect bomb placement
            if (controller.IsEnterPressed() || Resource.InputManager.IsEnterPressed() || Resource.InputManager.IsSpacePressed())
            {
                OnPlaceBomb?.Invoke(Position, this);
            }

            // Normalize input for diagonal movement
            if (input.Length() > 0)
            {
                input.Normalize();
                SetVelocity(input);
            }
            else
            {
                Stop();
                SetNeutralState();
            }

            // Update hero state based on input
            if (input != Vector2.Zero)
            {
                _idleTime = TimeSpan.Zero;
                _animationTime += Resource.UpdateGameTime.ElapsedGameTime;

                if (_animationTime.TotalSeconds > AnimationInterval)
                {
                    _animationTime = TimeSpan.Zero;
                    UpdateAnimationState(input);
                }
            }
            else
            {
                _idleTime += Resource.UpdateGameTime.ElapsedGameTime;

                if (_idleTime.TotalSeconds > IdleThreshold)
                {
                    _currentState = HeroState.MovingDownNeutral;
                }
            }
        }

        // Update method without controller parameter
        public override void Update()
        {
            base.Update();

            // Decrement the collision cooldown timer
            if (_collisionCooldown > TimeSpan.Zero)
            {
                _collisionCooldown -= Resource.UpdateGameTime.ElapsedGameTime;
            }

            // Handle input
            Vector2 input = Vector2.Zero;

            if (Resource.InputManager.IsMoveUp()) input.Y -= 1;
            if (Resource.InputManager.IsMoveDown()) input.Y += 1;
            if (Resource.InputManager.IsMoveLeft()) input.X -= 1;
            if (Resource.InputManager.IsMoveRight()) input.X += 1;

            // Detect bomb placement
            if (Resource.InputManager.IsEnterPressed() || Resource.InputManager.IsSpacePressed())
            {
                OnPlaceBomb?.Invoke(Position, this);
            }

            // Normalize input for diagonal movement
            if (input.Length() > 0)
            {
                input.Normalize();
                SetVelocity(input);
            }
            else
            {
                Stop();
                SetNeutralState();
            }

            // Update hero state based on input
            if (input != Vector2.Zero)
            {
                _idleTime = TimeSpan.Zero;
                _animationTime += Resource.UpdateGameTime.ElapsedGameTime;

                if (_animationTime.TotalSeconds > AnimationInterval)
                {
                    _animationTime = TimeSpan.Zero;
                    UpdateAnimationState(input);
                }
            }
            else
            {
                _idleTime += Resource.UpdateGameTime.ElapsedGameTime;

                if (_idleTime.TotalSeconds > IdleThreshold)
                {
                    _currentState = HeroState.MovingDownNeutral;
                }
            }
        }

        public bool CanTakeDamage()
        {
            return _collisionCooldown <= TimeSpan.Zero;
        }

        public void TakeDamage()
        {
            if (CanTakeDamage())
            {
                Health -= 1;
                _collisionCooldown = TimeSpan.FromSeconds(CollisionCooldownDuration);
                Art.HeroOuchSound?.Play();
            }
        }

        private void SetNeutralState()
        {
            _currentState = _currentState switch
            {
                HeroState.MovingLeftLeftFoot => HeroState.MovingLeftNeutral,
                HeroState.MovingLeftRightFoot => HeroState.MovingLeftNeutral,
                HeroState.MovingRightLeftFoot => HeroState.MovingRightNeutral,
                HeroState.MovingRightRightFoot => HeroState.MovingRightNeutral,
                HeroState.MovingUpLeftFoot => HeroState.MovingUpNeutral,
                HeroState.MovingUpRightFoot => HeroState.MovingUpNeutral,
                HeroState.MovingDownLeftFoot => HeroState.MovingDownNeutral,
                HeroState.MovingDownRightFoot => HeroState.MovingDownNeutral,
                _ => _currentState
            };
        }

        private void UpdateAnimationState(Vector2 input)
        {
            if (input.X < 0)
            {
                _currentState = _currentState switch
                {
                    HeroState.MovingLeftNeutral => HeroState.MovingLeftLeftFoot,
                    HeroState.MovingLeftLeftFoot => HeroState.MovingLeftRightFoot,
                    HeroState.MovingLeftRightFoot => HeroState.MovingLeftNeutral,
                    _ => HeroState.MovingLeftNeutral
                };
            }
            else if (input.X > 0)
            {
                _currentState = _currentState switch
                {
                    HeroState.MovingRightNeutral => HeroState.MovingRightLeftFoot,
                    HeroState.MovingRightLeftFoot => HeroState.MovingRightRightFoot,
                    HeroState.MovingRightRightFoot => HeroState.MovingRightNeutral,
                    _ => HeroState.MovingRightNeutral
                };
            }
            else if (input.Y < 0)
            {
                _currentState = _currentState switch
                {
                    HeroState.MovingUpNeutral => HeroState.MovingUpLeftFoot,
                    HeroState.MovingUpLeftFoot => HeroState.MovingUpRightFoot,
                    HeroState.MovingUpRightFoot => HeroState.MovingUpNeutral,
                    _ => HeroState.MovingUpNeutral
                };
            }
            else if (input.Y > 0)
            {
                _currentState = _currentState switch
                {
                    HeroState.MovingDownNeutral => HeroState.MovingDownLeftFoot,
                    HeroState.MovingDownLeftFoot => HeroState.MovingDownRightFoot,
                    HeroState.MovingDownRightFoot => HeroState.MovingDownNeutral,
                    _ => HeroState.MovingDownNeutral
                };
            }
        }

        // Get the source rectangle for the current state
        public override Rectangle GetSourceRectangle()
        {
            return _currentState switch
            {
                HeroState.MovingDownRightFoot => new Rectangle(2, 46, _imageWidth, _imageHeight),
                HeroState.MovingDownNeutral => new Rectangle(19, 46, _imageWidth, _imageHeight),
                HeroState.MovingDownLeftFoot => new Rectangle(36, 46, _imageWidth, _imageHeight),
                HeroState.MovingRightRightFoot => new Rectangle(2, 71, _imageWidth, _imageHeight),
                HeroState.MovingRightNeutral => new Rectangle(19, 71, _imageWidth, _imageHeight),
                HeroState.MovingRightLeftFoot => new Rectangle(36, 71, _imageWidth, _imageHeight),
                HeroState.MovingUpRightFoot => new Rectangle(2, 96, _imageWidth, _imageHeight),
                HeroState.MovingUpNeutral => new Rectangle(19, 96, _imageWidth, _imageHeight),
                HeroState.MovingUpLeftFoot => new Rectangle(36, 96, _imageWidth, _imageHeight),
                HeroState.MovingLeftRightFoot => new Rectangle(2, 121, _imageWidth, _imageHeight),
                HeroState.MovingLeftNeutral => new Rectangle(19, 121, _imageWidth, _imageHeight),
                HeroState.MovingLeftLeftFoot => new Rectangle(36, 121, _imageWidth, _imageHeight),
                _ => new Rectangle(19, 46, _imageWidth, _imageHeight), // Default to MovingDownNeutral
            };
        }
    }
}
