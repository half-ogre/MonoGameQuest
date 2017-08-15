using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Foundation
{
    /// <summary>
    /// Extension methods that draw 2D primitives.
    /// </summary>
    public static class PrimitiveExtensions
    {
        /// <summary>
        /// Draws a line between two positions.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the line</param>
        /// <param name="texture">The texture with which to draw the line</param>
        /// <param name="start">The line's start position</param>
        /// <param name="end">The line's end position</param>
        /// <param name="lineWidth">The line's width</param>
        public static void DrawLine(
            this SpriteBatch spriteBatch,
            Texture2D texture,
            Vector2 start,
            Vector2 end,
            int lineWidth = 1)
        {
            var length = Vector2.Distance(start, end);
            var edge = end - start;
            var angle = (float)Math.Atan2(edge.Y, edge.X);

            DrawLine(spriteBatch, texture, start, length, angle, lineWidth);
        }

        /// <summary>
        /// Draws a line starting from the specified position, at the specified angle, with the specified length.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch with which to draw the line</param>
        /// <param name="texture">The texture with which to draw the line</param>
        /// <param name="point">The point at which to start the line</param>
        /// <param name="length">The line's length</param>
        /// <param name="angle">The angle at which to draw the line</param>
        /// <param name="lineWidth">The line's width</param>
        public static void DrawLine(
            this SpriteBatch spriteBatch,
            Texture2D texture,
            Vector2 point, 
            float length, 
            float angle = 0, 
            int lineWidth = 1)
        {
            spriteBatch.Draw(
                texture,
                position: point,
                sourceRectangle: null,
                color: Color.White /* tint */,
                rotation: angle,
                origin: Vector2.Zero,
                scale: new Vector2(length, lineWidth),
                effects: SpriteEffects.None,
                layerDepth: 0);
        }
    }
}
