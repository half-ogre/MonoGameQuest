using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameQuest.Sprites
{
    public class ClothArmor : PlayerCharacterSprite
    {
        public ClothArmor(MonoGameQuest game, ContentManager contentManager, Vector2 coordinatePosition, Map map) : base(
            game: game,
            contentManager: contentManager,
            spriteSheetName: "clotharmor", 
            pixelHeight: 32, 
            pixelWidth: 32, 
            pixelOffsetX: -8, 
            pixelOffsetY: -12, 
            coordinatePosition: coordinatePosition, 
            map: map,
            movementLength: Constants.DefaultMoveLength,
            movementSpeed: Constants.DefaultMoveSpeed)
        {
            AddAnimation(new Animation(
                spriteSheet: SpriteSheet,
                type: AnimationType.Idle,
                direction: Direction.Up,
                spriteSheetRow: 5, 
                framePixelWidth: PixelWidth,
                framePixelHeight: PixelHeight,
                framesLength: 2, 
                frameDuration: Constants.DefaultIdleSpeed));

            AddAnimation(new Animation(
                spriteSheet: SpriteSheet,
                type: AnimationType.Idle,
                direction: Direction.Down,
                spriteSheetRow: 8,
                framePixelWidth: PixelWidth,
                framePixelHeight: PixelHeight,
                framesLength: 2, 
                frameDuration: Constants.DefaultIdleSpeed));

            AddAnimation(new Animation(
                spriteSheet: SpriteSheet,
                type: AnimationType.Idle,
                direction: Direction.Left,
                spriteSheetRow: 2,
                framePixelWidth: PixelWidth,
                framePixelHeight: PixelHeight,
                framesLength: 2, 
                frameDuration: Constants.DefaultIdleSpeed,
                flipHorizontally: true));

            AddAnimation(new Animation(
                spriteSheet: SpriteSheet,
                type: AnimationType.Idle,
                direction: Direction.Right,
                spriteSheetRow: 2,
                framePixelWidth: PixelWidth,
                framePixelHeight: PixelHeight,
                framesLength: 2, 
                frameDuration: Constants.DefaultIdleSpeed));

            AddAnimation(new Animation(
                spriteSheet: SpriteSheet,
                type: AnimationType.Walk,
                direction: Direction.Up,
                spriteSheetRow: 4,
                framePixelWidth: PixelWidth,
                framePixelHeight: PixelHeight,
                framesLength: 4, 
                frameDuration: Constants.DefaultWalkSpeed));

            AddAnimation(new Animation(
                spriteSheet: SpriteSheet,
                type: AnimationType.Walk,
                direction: Direction.Down,
                spriteSheetRow: 7,
                framePixelWidth: PixelWidth,
                framePixelHeight: PixelHeight,
                framesLength: 4, 
                frameDuration: Constants.DefaultWalkSpeed));

            AddAnimation(new Animation(
                spriteSheet: SpriteSheet,
                type: AnimationType.Walk,
                direction: Direction.Left,
                spriteSheetRow: 1,
                framePixelWidth: PixelWidth,
                framePixelHeight: PixelHeight,
                framesLength: 4, 
                frameDuration: Constants.DefaultWalkSpeed,
                flipHorizontally: true));

            AddAnimation(new Animation(
                spriteSheet: SpriteSheet,
                type: AnimationType.Walk,
                direction: Direction.Right,
                spriteSheetRow: 1,
                framePixelWidth: PixelWidth,
                framePixelHeight: PixelHeight,
                framesLength: 4, 
                frameDuration: Constants.DefaultWalkSpeed));
        }
    }
}
