using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameQuest
{
    public class Player : MonoGameQuestComponent
    {
        bool _leftMouseButtonWasPressed;
        readonly Stack<Action> _movement;
        Vector2? _movementNextDestination;
        int _movementTimeAtCurrentPosition;
        readonly Pathfinder _pathfinder;
        Direction? _pathInterruptingMove;
        PlayerSprite _sprite;

        public Player(MonoGameQuest game) : base(game)
        {
            _movement = new Stack<Action>();
            _pathfinder = new Pathfinder();

            Orientation = Direction.Down;
            Path = new Queue<Vector2>();
            UpdateOrder = Constants.UpdateOrder.Models;
        }

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

        private void HandleInput()
        {
            if (!HandleKeyboardInput())
                HandleMouseInput();
        }

        private bool HandleKeyboardInput()
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Move(Direction.Left);
                return true;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Move(Direction.Right);
                return true;
            }
            else if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                Move(Direction.Up);
                return true;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                Move(Direction.Down);
                return true;
            }

            return false;
        }

        private void HandleMouseInput()
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
        }

        public override void Initialize()
        {
            _sprite = new ClothArmor(Game, new Vector2(15, 222));
            Game.Components.Add(_sprite);
            
            base.Initialize();
        }

        public bool IsMoving { get { return _movementNextDestination.HasValue; } }

        public void Move(Vector2 destination)
        {
            // if the player is not currently traveling a path, use the current coordinate position:
            var origin = CoordinatePosition;
            
            // if the player is already traveling a path, use the path's next destination as the origin
            if (_movementNextDestination.HasValue)
                origin = _movementNextDestination.Value;
            
            // set a new path:
            Path = _pathfinder.FindPath(origin, destination);

            if (!IsMoving)
            {
                var firstDestination = Path.Dequeue();
                var nextDirection = CaclulateMovementDirection(CoordinatePosition, firstDestination);
                MoveOne(nextDirection);   
            }
        }

        public void Move(Direction direction)
        {
            // if already moving but not on a path, ignore the move, because we need to finish the current move first:
            if (IsMoving && Path.Count <= 0)
                return;

            if (IsMoving && Path.Count > 0)
            {
                _pathInterruptingMove = direction;
                Path.Clear();
            } else
            {
                MoveOne(direction);
            }
        }

        public void MoveOne(Direction direction)
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

            _movementNextDestination = new Vector2(newX, newY);

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

                    // this is the last frame of movement, so we need to do some house-keeping:
                    if (frameIndex == Constants.DefaultMoveLength - 1)
                    {
                        // clear the player's next movement destination:
                        _movementNextDestination = null;

                        // check if there was a path-interrupting move:
                        if (_pathInterruptingMove.HasValue)
                        {
                            var d = _pathInterruptingMove.Value;
                            _pathInterruptingMove = null;
                            MoveOne(d);
                        }
                        // check the path for a next movement destination, and start it:
                        else if (Path.Count > 0)
                        {
                            var destination = Path.Dequeue();
                            var nextDirection = CaclulateMovementDirection(CoordinatePosition, destination);
                            if (nextDirection != Direction.None)
                                MoveOne(nextDirection);
                        }
                    }
                });
            }
        }

        public Direction Orientation { get; protected set; }

        public Queue<Vector2> Path { get; private set; }

        public override void Update(GameTime gameTime)
        {
            HandleInput();

            UpdateMovement(gameTime);
        }

        private void UpdateMovement(GameTime gameTime)
        {
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
