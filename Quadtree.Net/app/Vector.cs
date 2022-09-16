using System;

namespace SimpleX
{
    struct Vector
    {
        public float x;
        public float y;

        public readonly static Vector zero  = new Vector(0, 0);
        public readonly static Vector one   = new Vector(1, 1);
        public readonly static Vector left  = new Vector(-1, 0);
        public readonly static Vector right = new Vector(1, 0);
        public readonly static Vector up    = new Vector(0, -1);
        public readonly static Vector down  = new Vector(0, 1);

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        // 向量的模
        public float magnitude
        {
            get { return (float)Math.Sqrt(x * x + y * y); }
        }

        // 获得归一化后的单位向量
        public Vector normalized
        {
            get
            {
                var m = magnitude;
                var u = x / m;
                var v = y / m;
                return new Vector(u, v);
            }
        }

        // 向量点乘
        public static float Dot(ref Vector a, ref Vector b)
        {
            return a.x * b.x + a.y * b.y;
        }

        // 反射向量
        public static Vector Reflect(ref Vector input, ref Vector normal)
        {
            var I = input;
            var N = normal.normalized;
            var R = I - 2 * Vector.Dot(ref I, ref N) * N;

            return R.normalized;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }

        public static Vector operator *(Vector v, float k)
        {
            return new Vector(v.x * k, v.y * k);
        }

        public static Vector operator *(float k, Vector v)
        {
            return v * k;
        }
    }
}
