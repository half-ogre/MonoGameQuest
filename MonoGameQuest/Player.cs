using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class Player : MonoGameQuestComponent
    {
        bool _leftMouseButtonWasPressed;
        readonly Stack<Action> _movement;
        int _movementTimeAtCurrentPosition;
        readonly Pathfinder _pathfinder;
        PlayerSprite _sprite;

        public Player(MonoGameQuest game) : base(game)
        {
            _movement = new Stack<Action>();
            _pathfinder = new Pathfinder();

            Orientation = Direction.Down;
            Path = new Queue<Vector2>();
            UpdateOrder = Constants.UpdateOrder.Models;
        }

        public override void Initialize()
        {
            _sprite = new ClothArmor(Game, new Vector2(15, 222));
            Game.Components.Add(_sprite);
            
            base.Initialize();
        }

        public bool IsMoving { get { return _movement.Count > 0; } }

        private Direction CaclulateMovementDirection(
            Vector2 origin,
            Vector2 destination)
        {
            if (origin.X < destination.X)
                return Direction.Right;
            if (origin.X > destination.X)
                return Direction.Left;
            if (origin.Y < destination.Y)
                return Direction.Down;
            if (origin.Y > destination.Y)
                return Direction.Up;
            return Direction.None;
        }

        public Vector2 CoordinatePosition { get { return _sprite.CoordinatePosition; } }

        public void Move(Vector2 destination)
        {
            // if the player is not currently traveling a path, use the current coordinate position:
            var origin = CoordinatePosition;
            
            // if the player is already traveling a path, use the path's next destination as the origin
            if (Path.Count > 0)
                origin = Path.Peek();
            
            // set a new path:
            Path = _pathfinder.FindPath(origin, destination);

            var firstDestination = Path.Dequeue();
            var nextDirection = CaclulateMovementDirection(CoordinatePosition, firstDestination);
            Move(nextDirection);   
        }

        public void Move(Direction direction)
        {
            var xDelta = 0f;
            var yDelta = 0f;

            if (direction == Direction.Up)
                yDelta = -1f;
            if (direction == Direction.Down)
                yDelta = 1f;
            if (direction == Direction.Left)
                xDelta = -1f;
            if (direction == Direction.Right)
                xDelta = 1f;

            var newX = CoordinatePosition.X + xDelta;
            var newY = CoordinatePosition.Y + yDelta;

            // don't let the sprite move off the map:
            if (newX < 0 || newX > Game.Map.CoordinateWidth - 1 || newY < 0 || newY > Game.Map.CoordinateHeight - 1)
                return;

            _movementTimeAtCurrentPosition = _movementTimeAtCurrentPosition % Constants.DefaultMoveSpeed;
            _movement.Clear();

            if (Orientation != direction)
            {
                Orientation = direction;

                // start a new walk animation for the new orientation
                _sprite.Animate(AnimationType.Walk, Orientation);
            }

            for (var n = Constants.DefaultMoveLength - 1; n >= 0; n--)
            {
                var frameIndex = n;

                _movement.Push(() =>
                {
                    _sprite.CoordinatePosition = new Vector2(
                        CoordinatePosition.X + (xDelta / Constants.DefaultMoveLength),
                        CoordinatePosition.Y + (yDelta / Constants.DefaultMoveLength));

                    // if this is the last frame of movement, check the path for the next destination:
                    if (frameIndex == Constants.DefaultMoveLength - 1)
                    {
                        if (Path.Count > 0)
                        {
                            var destination = Path.Dequeue();
                            var nextDirection = CaclulateMovementDirection(CoordinatePosition, destination);
                            Move(nextDirection);
                        }
                    }
                });
            }
        }

        public Direction Orientation { get; protected set; }

        public Queue<Vector2> Path { get; private set; }

        public override void Update(GameTime gameTime)
        {
            // check for the player pressing the left mouse button, to move:
            var mouseState = Mouse.GetState();
            var leftMouseButtonIsPressed = mouseState.LeftButton == ButtonState.Pressed;
            if (!leftMouseButtonIsPressed && _leftMouseButtonWasPressed)
            {
                var mousePixelPosition = new Vector2(mouseState.X, mouseState.Y);
                var displayCoordinate = Game.Display.CalculateCoordinateFromPixelPosition(mousePixelPosition);
                var mapCoordinate = Game.Display.CalculateMapCoordinateFromDisplayCoordinate(displayCoordinate);
                Move(mapCoordinate);
            }
            _leftMouseButtonWasPressed = leftMouseButtonIsPressed;
            
            // stash IsMoving because it might change when the top of the movement stack is popped, and we need to know the value when the update started
            var wasMoving = IsMoving;

            if (IsMoving)
            {
                _movementTimeAtCurrentPosition += gameTime.ElapsedGameTime.Milliseconds;

                if (_movementTimeAtCurrentPosition > Constants.DefaultMoveSpeed / Constants.DefaultMoveLength)
                {
                    _movementTimeAtCurrentPosition = 0;
                    _movement.Pop()();
                }
            }

            // start the walking animation if the player is moving and isn't already walking:
            if (wasMoving && (_sprite.CurrentAnimation == null || _sprite.CurrentAnimation.Type != AnimationType.Walk))
                _sprite.Animate(AnimationType.Walk, Orientation);

            // stop walking if the sprite has stopped moving:
            if (!wasMoving && (_sprite.CurrentAnimation == null || _sprite.CurrentAnimation.Type == AnimationType.Walk))
                _sprite.Animate(AnimationType.Idle, Orientation);

            // if not moving and not already idling, start idling:
            if (!wasMoving && (_sprite.CurrentAnimation == null || _sprite.CurrentAnimation.Type != AnimationType.Idle))
                _sprite.Animate(AnimationType.Idle, Orientation);

            if (_sprite.CurrentAnimation == null)
                _sprite.Animate(AnimationType.Idle, Orientation);
        }
    }
}
