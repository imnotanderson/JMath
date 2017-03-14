
namespace JMath
{
    public struct Vector2
    {
        public static Vector2 down = new Vector2(0, -1);
        public static Vector2 up = new Vector2(0, 1);
        public static Vector2 left = new Vector2(1, 0);
        public static Vector2 right = new Vector2(-1, 0);
        public static Vector2 zero = new Vector2(0, 0);
        public static Vector2 one = new Vector2(1, 1);

        public Float x, y;
        public Vector2 normalized
        {
            get
            {
                var len = (x * x + y * y).Sqrt();
                Vector2 v = this;
                v.x /= len;
                v.y /= len;
                return v;
            }
        }
        public UnityEngine.Vector2 Val
        {
            get
            {
                return new UnityEngine.Vector2(x.Val, y.Val);
            }
        }


        public Vector2(float x, float y) : this(new Float(x), new Float(y)) { }

        public Vector2(Float x, Float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator *(float f, Vector2 v)
        {
            return v * f;
        }

        public static Vector2 operator *(Vector2 v, float f)
        {
            var newF = new Float(f);
            return v * newF;
        }

        public static Vector2 operator *(Float f, Vector2 v)
        {
            return v * f;
        }

        public static Vector2 operator *(Vector2 v, Float f)
        {
            v.x *= f;
            v.y *= f;
            return v;
        }

        public static Vector2 operator *(int i, Vector2 v1)
        {
            return v1 * i;
        }

        public static Vector2 operator *(Vector2 v1, int i)
        {
            var v = new Vector2();
            v.x = v1.x * i;
            return v;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            var v = new Vector2();
            v.x = v1.x + v2.x;
            v.y = v1.y + v2.y;
            return v;
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            var v = new Vector2();
            v.x = v1.x - v2.x;
            v.y = v1.y - v2.y;
            return v;
        }
        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }
    }

    /// <summary>
    /// range abs(val)< int.maxVal/PRECISION --
    /// </summary>
    public struct Float
    {
        public const int PRECISION = 10000;
        long iVal;

        public static Float zero = new Float(0);
        public float Val
        {
            get
            {
                return (float)iVal / PRECISION;
            }
        }

        public Float(float a)
        {
            iVal = (int)(a * PRECISION);
        }

        public Float(int a) : this(a, 1)
        {
        }
        public Float(long a, int b)
        {
            this.iVal = PRECISION / b;
            this.iVal *= a;
        }
        public Float(int a, int b)
        {
            this.iVal = PRECISION / b;
            this.iVal *= a;
        }

        public static Float operator +(Float v1, Float v2)
        {
            v1.iVal += v2.iVal;
            return v1;
        }
        public static Float operator -(Float v2)
        {
            v2.iVal = -v2.iVal;
            return v2;
        }

        public Float Sqrt()
        {
            Float f = this;
            f.iVal = (long)System.Math.Sqrt(f.iVal * PRECISION);
            return f;
        }

        static Float Abs(Float f)
        {
            if (f.iVal > 0)
            {
                return f;
            }
            f.iVal = -f.iVal;
            return f;
        }


        public static bool operator <=(Float v1, Float v2)
        {
            return v1.iVal <= v2.iVal;
        }
        public static bool operator <(Float v1, Float v2)
        {
            return v1.iVal < v2.iVal;
        }
        public static bool operator >=(Float v1, Float v2)
        {
            return v1.iVal >= v2.iVal;
        }
        public static bool operator >(Float v1, Float v2)
        {
            return v1.iVal > v2.iVal;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is Float == false)
            {
                return false;
            }
            return this == (Float)obj;
        }

        public override int GetHashCode()
        {
            return iVal.GetHashCode();
        }

        public static bool operator ==(Float v1, Float v2)
        {
            return v1.iVal == v2.iVal;
        }
        public static bool operator !=(Float v1, Float v2)
        {
            return v1.iVal != v2.iVal;
        }

        public static Float operator -(Float v1, Float v2)
        {
            v1.iVal -= v2.iVal;
            return v1;
        }
        public static Float operator *(Float v1, int v2)
        {
            v1.iVal *= v2;
            if ((v1.iVal > 0 && (v1.iVal) > int.MaxValue) || (v1.iVal < 0 && (-v1.iVal) > int.MaxValue))
            {
                throw new System.Exception("Float overflow " + v1.iVal + " * " + v2);
            }
            return v1;
        }

        public static Float operator *(Float v1, Float v2)
        {
            v1.iVal *= v2.iVal;
            v1.iVal /= PRECISION;
            if ((v1.iVal > 0 && (v1.iVal) > int.MaxValue) || (v1.iVal < 0 && (-v1.iVal) > int.MaxValue))
            {
                throw new System.Exception("Float overflow " + v1.iVal + " * " + v2.iVal);
            }
            return v1;
        }

        public static Float operator /(Float v1, Float v2)
        {
            if (long.MaxValue / PRECISION < System.Math.Abs(v1.iVal))
            {
                throw new System.Exception("Float overflow: " + v1.iVal);
            }
            v1.iVal *= PRECISION;
            v1.iVal /= v2.iVal;
            return v1;
        }
        public static Float operator /(Float v1, int v2)
        {
            v1.iVal /= v2;
            return v1;
        }

        public override string ToString()
        {
            return Val.ToString();
        }
    }

}
