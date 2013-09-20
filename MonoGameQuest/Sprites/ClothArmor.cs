using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameQuest.Sprites
{
    public class ClothArmor : PlayerCharacterSprite
    {
        public ClothArmor(ContentManager contentManager, Vector2 mapPosition, Map map) : base(
            contentManager: contentManager,
            spritesheetName: "clotharmor", 
            height: 32, 
            width: 32, 
            offsetX: -8, 
            offsetY: -12, 
            mapPosition: mapPosition, 
            mapTileHeight: map.TileHeight,
            mapTileWidth: map.TileWidth,
            movementLength: Animation.DefaultMoveLength,
            movementSpeed: Animation.DefaultMoveSpeed)
        {
            AddAnimation(new Animation(
                type: AnimationType.Idle,
                direction: Direction.Up,
                row: 5, 
                length: 2, 
                speed: Animation.DefaultIdleSpeed));

            AddAnimation(new Animation(
                type: AnimationType.Idle,
                direction: Direction.Down,
                row: 8, 
                length: 2, 
                speed: Animation.DefaultIdleSpeed));

            AddAnimation(new Animation(
                type: AnimationType.Idle,
                direction: Direction.Left,
                row: 2, 
                length: 2, 
                speed: Animation.DefaultIdleSpeed,
                flipHorizontally: true));

            AddAnimation(new Animation(
                type: AnimationType.Idle,
                direction: Direction.Right,
                row: 2, 
                length: 2, 
                speed: Animation.DefaultIdleSpeed));

            AddAnimation(new Animation(
                type: AnimationType.Walk,
                direction: Direction.Up,
                row: 4, 
                length: 4, 
                speed: Animation.DefaultWalkSpeed));

            AddAnimation(new Animation(
                type: AnimationType.Walk,
                direction: Direction.Down,
                row: 7, 
                length: 4, 
                speed: Animation.DefaultWalkSpeed));

            AddAnimation(new Animation(
                type: AnimationType.Walk,
                direction: Direction.Left,
                row: 1, 
                length: 4, 
                speed: Animation.DefaultWalkSpeed,
                flipHorizontally: true));

            AddAnimation(new Animation(
                type: AnimationType.Walk,
                direction: Direction.Right,
                row: 1, 
                length: 4, 
                speed: Animation.DefaultWalkSpeed));
        }
    }
}
