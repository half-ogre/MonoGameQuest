using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameQuest.Sprites
{
    public class ClothArmor : PlayerCharacterSprite
    {
        public ClothArmor(MonoGameQuest game, ContentManager contentManager, Vector2 position, Map map) : base(
            game: game,
            contentManager: contentManager,
            spriteSheetName: "clotharmor", 
            height: 32, 
            width: 32, 
            offsetX: -8, 
            offsetY: -12, 
            position: position, 
            map: map,
            movementLength: Constants.DefaultMoveLength,
            movementSpeed: Constants.DefaultMoveSpeed)
        {
            AddAnimation(new Animation(
                sprite: this,
                type: AnimationType.Idle,
                direction: Direction.Up,
                row: 5, 
                length: 2, 
                speed: Constants.DefaultIdleSpeed));

            AddAnimation(new Animation(
                sprite: this,
                type: AnimationType.Idle,
                direction: Direction.Down,
                row: 8, 
                length: 2, 
                speed: Constants.DefaultIdleSpeed));

            AddAnimation(new Animation(
                sprite: this,
                type: AnimationType.Idle,
                direction: Direction.Left,
                row: 2, 
                length: 2, 
                speed: Constants.DefaultIdleSpeed,
                flipHorizontally: true));

            AddAnimation(new Animation(
                sprite: this,
                type: AnimationType.Idle,
                direction: Direction.Right,
                row: 2, 
                length: 2, 
                speed: Constants.DefaultIdleSpeed));

            AddAnimation(new Animation(
                sprite: this,
                type: AnimationType.Walk,
                direction: Direction.Up,
                row: 4, 
                length: 4, 
                speed: Constants.DefaultWalkSpeed));

            AddAnimation(new Animation(
                sprite: this,
                type: AnimationType.Walk,
                direction: Direction.Down,
                row: 7, 
                length: 4, 
                speed: Constants.DefaultWalkSpeed));

            AddAnimation(new Animation(
                sprite: this,
                type: AnimationType.Walk,
                direction: Direction.Left,
                row: 1, 
                length: 4, 
                speed: Constants.DefaultWalkSpeed,
                flipHorizontally: true));

            AddAnimation(new Animation(
                sprite: this,
                type: AnimationType.Walk,
                direction: Direction.Right,
                row: 1, 
                length: 4, 
                speed: Constants.DefaultWalkSpeed));
        }
    }
}
