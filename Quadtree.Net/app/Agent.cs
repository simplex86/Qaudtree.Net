using System;
using System.Drawing;

namespace SimpleX
{
    class Agent : IQuadObject
    {
        public float x { get; set; }
        public float y { get; set; }
        public float w { get; private set; }
        public float h { get; private set; }

        public Vector direction { get; set; } = Vector.zero;
        public float speed { get; private set; } = 0;

        public Color color { get; set; } = Color.Red;

        public Agent(float x, float y, float w, float h, float s)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.speed = s;
        }
    }
}
