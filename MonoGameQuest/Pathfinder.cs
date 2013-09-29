using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameQuest
{
    public class Pathfinder
    {
        public Queue<Vector2> FindPath(Vector2 start, Vector2 end)
        {
            var path = new Queue<Vector2>();

            var xDistance = (int)end.X - (int)start.X;
            var yDistance = (int)end.Y - (int)start.Y;

            var xStep = xDistance > 0 ? 1 : -1;
            var xSteps = 0;
            while (xSteps != xDistance)
            {
                xSteps += xStep;
                path.Enqueue(new Vector2(start.X + xSteps, start.Y));
            }

            var yStep = yDistance > 0 ? 1 : -1;
            var ySteps = 0;
            while (ySteps != yDistance)
            {
                ySteps += yStep;
                path.Enqueue(new Vector2(end.X, start.Y + ySteps));
            }

            return path;
        }
    }
}
