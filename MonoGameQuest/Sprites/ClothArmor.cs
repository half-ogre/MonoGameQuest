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
            movementSpeed: Animation.DefaultMoveSpeed,
            idleAnimationSpeed: Animation.DefaultIdleSpeed,
            idleUpAnimation: new Animation(row: 5, length: 2),
            idleDownAnimation: new Animation(row: 8, length: 2),
            idleRightAnimation: new Animation(row: 2, length: 2),
            walkAnimationSpeed: Animation.DefaultWalkSpeed,
            walkUpAnimation: new Animation(row: 4, length: 4),
            walkDownAnimation: new Animation(row: 7, length: 4),
            walkRightAnimation: new Animation(row: 1, length: 4))
        {
        }
    }
}
