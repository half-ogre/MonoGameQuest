using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class Player : MonoGameQuestComponent
    {
        bool _leftMouseButtonWasPressed;
        readonly Pathfinder _pathfinder;
        PlayerSprite _sprite;

        public Player(MonoGameQuest game) : base(game)
        {
            _pathfinder = new Pathfinder();
            
            Path = new Queue<Vector2>();
            UpdateOrder = Constants.UpdateOrder.Models;
        }

        public override void Initialize()
        {
            _sprite = new ClothArmor(Game, new Vector2(15, 222));
            Game.Components.Add(_sprite);
            
            base.Initialize();
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
        }

        public Queue<Vector2> Path { get; private set; }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var leftMouseButtonIsPressed = mouseState.LeftButton == ButtonState.Pressed;
            if (!leftMouseButtonIsPressed && _leftMouseButtonWasPressed)
            {
                var mousePixelPosition = new Vector2(mouseState.X, mouseState.Y);
                var displayCoordinate = Game.Display.CalculateCoordinateFromPixelPosition(mousePixelPosition);
                var mapCoordinate = Game.Display.CalculateMapCoordinate(displayCoordinate);
                Move(mapCoordinate);
            }
            _leftMouseButtonWasPressed = leftMouseButtonIsPressed;
            
            var keyboardState = Keyboard.GetState();

            // if the sprite isn't already moving, accept new movement:
            if (!_sprite.IsMoving)
            {
                // move up:
                if (keyboardState.IsKeyDown(Keys.Up))
                    _sprite.Move(Direction.Up);
                // move down:
                else if (keyboardState.IsKeyDown(Keys.Down))
                    _sprite.Move(Direction.Down);
                // move the the left
                else if (keyboardState.IsKeyDown(Keys.Left))
                    _sprite.Move(Direction.Left);
                // move to the right:
                else if (keyboardState.IsKeyDown(Keys.Right))
                    _sprite.Move(Direction.Right);   
            }
        }
    }
}
