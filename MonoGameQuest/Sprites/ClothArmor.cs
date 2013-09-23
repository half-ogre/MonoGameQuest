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
            AddAnimation(
                AnimationType.Idle,
                Direction.Up,
                spriteSheetRow: 5, 
                framesLength: 2, 
                frameDuration: Constants.DefaultIdleSpeed);

            AddAnimation(
                AnimationType.Idle,
                Direction.Down,
                spriteSheetRow: 8,
                framesLength: 2, 
                frameDuration: Constants.DefaultIdleSpeed);

            AddAnimation(
                AnimationType.Idle,
                Direction.Left,
                spriteSheetRow: 2,
                framesLength: 2, 
                frameDuration: Constants.DefaultIdleSpeed,
                flipHorizontally: true);

            AddAnimation(
                AnimationType.Idle,
                Direction.Right,
                spriteSheetRow: 2,
                framesLength: 2, 
                frameDuration: Constants.DefaultIdleSpeed);

            AddAnimation(
                AnimationType.Walk,
                Direction.Up,
                spriteSheetRow: 4,
                framesLength: 4, 
                frameDuration: Constants.DefaultWalkSpeed);

            AddAnimation(
                AnimationType.Walk,
                Direction.Down,
                spriteSheetRow: 7,
                framesLength: 4, 
                frameDuration: Constants.DefaultWalkSpeed);

            AddAnimation(
                AnimationType.Walk,
                Direction.Left,
                spriteSheetRow: 1,
                framesLength: 4, 
                frameDuration: Constants.DefaultWalkSpeed,
                flipHorizontally: true);

            AddAnimation(
                AnimationType.Walk,
                Direction.Right,
                spriteSheetRow: 1,
                framesLength: 4, 
                frameDuration: Constants.DefaultWalkSpeed);
        }
    }
}
