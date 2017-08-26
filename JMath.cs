#define UNITY
using UnityEngine;

namespace JMath
{
	public static class EXCEPTION{
		public static readonly System.Exception NO_IMPLEMENT = new System.Exception("no implementm");
	}

	public static class Mathf{
		public static Float Min(Float a,Float b ){
            if (a.iVal<b.iVal)return a;
			return b;
		}

		public static Float Min(float a,Float b ){
            return Min(new Float(a),b);
		}
		public static Float MaxAbs(Float a,Float b){
            if (System.Math.Abs(a.iVal)>System.Math.Abs(b.iVal)){
				return a;
			}
			return b;
		}
		public static int CeilToInt(Float f){
            if (f.iVal % Float.PRECISION == 0)
            {
                return (int)(f.iVal / Float.PRECISION);
            }
			return (int)(f.iVal/Float.PRECISION)+1;
		}
		public static int Max(int a,int b){
			if(a>b)return a;
			return b;
		}
		public static Float Abs(Float a){
            if (a.iVal<0){
				a.iVal = -a.iVal;
			}
			return a;
		}
		public static int Abs(int a){
			return System.Math.Abs(a);
		}
		public static Float Clamp(Float Val,Float v1,Float v2){
			if(v1>v2){
				var tmv = v1;
				v1 = v2;
				v2 = tmv;
			}
            if (v1.iVal<=Val.iVal && Val.iVal<=v2.iVal){
				return Val;
			}
			if (Val.iVal < v1.iVal) {
				return v1;
			}
			return v2;
		}

		public static Float MoveTowards(Float Val, Float to, Float speed)
		{
			var newVal = Val + speed;
			newVal = Clamp(newVal, Val, to);
			return newVal;
		}
	}

    public struct Vector2
    {
	    public static readonly Vector2 down = new Vector2(0, -1);
        public static readonly Vector2 up = new Vector2(0, 1);
        public static readonly Vector2 left = new Vector2(-1, 0);
        public static readonly Vector2 right = new Vector2(1, 0);
        public static readonly Vector2 zero = new Vector2(0, 0);
        public static readonly Vector2 one = new Vector2(1, 1);

        public Float x, y;

	    public Float distance
	    {
		    get { return (x * x + y * y).Sqrt(); }
	    }
	    
        public Vector2 normalized
        {
            get
            {
                var len = (x * x + y * y).Sqrt();
                Vector2 v = this;
	            if (len == 0) return zero;
                v.x /= len;
                v.y /= len;
                return v;
            }
        }
#if UNITY
        public UnityEngine.Vector2 Val
        {
            get
            {
                return new UnityEngine.Vector2(x.Val, y.Val);
            }
        }
#endif
		public static Float Distance(Vector2 a,Vector2 b){
			var x = a.x-b.x;
			var y = a.y - b.y;
			return (x * x + y * y).Sqrt ();
		}
#if UNITY
		public Vector2(UnityEngine.Vector2 uv):this(uv.x,uv.y){}
#endif
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
            var v = v1;
            v.x = v1.x * i;
			v.y = v1.y * i;
            return v;
        }
	    
	    public static Float operator *(Vector2 f, Vector2 v)
	    {
		    return f.Dot(v);
	    }
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            var v = v1;
            v.x = v1.x + v2.x;
            v.y = v1.y + v2.y;
            return v;
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            var v = v1;
            v.x = v1.x - v2.x;
            v.y = v1.y - v2.y;
            return v;
        }
	    public static Vector2 operator -(Vector2 v2)
	    {
		    var v1 = zero;
		    var v = v1;
		    v.x = v1.x - v2.x;
		    v.y = v1.y - v2.y;
		    return v;
	    }
		public static Vector2 operator /(Vector2 v1, Float v2)
		{
            var v = v1;
			v.x /= v2;
			v.y /= v2;
			return v;
		}

	    public void Negate()
	    {
		    this.x = -x;
		    this.y = -y;
	    }

	    public Float Dot(Vector2 v)
	    {
		    return Vector2.Dot(this, v);
	    }
	    
	    public static Float Dot(Vector2 v1,Vector2 v2)
	    {
		    return v1.x * v2.x + v1.y * v2.y;
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
	    public const float TOLERANCE = 10f/PRECISION;
	    public long iVal;
	    
        public static Float zero = new Float(0);
        public float Val
        {
            get
            {
                return (float)((double)iVal/PRECISION);
            }
        }

        public Float(float a)
        {
            iVal = (long)(a * PRECISION);
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

	    public static implicit operator Float(float v)
	    {
		    return new Float(v);
	    }
	    
        public static Float operator +(Float v1, Float v2)
        {
            v1.iVal += v2.iVal;
            return v1;
        }

		public static Float operator -(Float v1, Float v2)
        {
			v1.iVal -=v2.iVal;
			return v1;
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
		public static bool operator <=(Float v1, float v2)
		{
            return v1 <= new Float (v2);
		}

        public static bool operator <=(Float v1, Float v2)
        {
            return v1.iVal <= v2.iVal;
        }
        public static bool operator <(Float v1, Float v2)
        {
            return v1.iVal < v2.iVal;
        }
		public static bool operator >=(Float v1, float v2)
		{
            return v1 >= new Float (v2);
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
		public static Float operator -(Float v1)
		{
            v1.iVal = -v1.iVal;
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
	        var d1 = v1.iVal - int.MaxValue;
	        var d2 = v2.iVal - int.MaxValue;
	        if (d1 > 0 || d2 > 0)
	        {
		        throw new System.Exception("Float overflow " + v1.iVal + ">" + int.MaxValue + "   " + v2.iVal + " > " +
		                                   int.MaxValue);
	        }
	        v1.iVal *= v2.iVal;
            v1.iVal /= PRECISION;
       
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

		public static bool operator >(Float v1,float f){
            return v1 >new Float(f);	
		}
		public static bool operator <(Float v1,float f){
            return v1 <new Float(f);	
		}

        public override string ToString()
        {
            return Val.ToString();
        }
    }

}
