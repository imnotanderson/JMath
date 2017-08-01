using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JMath
{
    public class Shape
    {
        private const int LOOP_MAX = 1000;
        private Exception LOOP_MAX_EXCEPTION = new Exception("LOOP_MAX_EXCEPTION");
        
        public List<Vector2> pList = new List<Vector2>();

        public Vector2 GetGjkDepth(Shape s)
        {
            return GetGjkDepth(this, s);
        }

        #region epa

        bool EPA(Shape shapeA, Shape shapeB, List<Vector2> simplex, out Float penetration_depth, out Vector2 penetration_vector)
        {
            Float distance = 0;
            int index = 0;
            Vector2 normal = Vector2.zero;
            
            var loopMax = LOOP_MAX;
            while(true)
            {
                {
                    loopMax--;
                    if (loopMax < 0)
                    {
                        throw LOOP_MAX_EXCEPTION;
                        break;
                    }
                }

                getNearestEdge(simplex, ref distance, ref normal, ref index);
    
                Vector2 sup = support(shapeA, shapeB, normal); //get support point in direction of edge's normal
    
                var d = sup*normal;
    
                if( d - distance< Float.TOLERANCE)
                {
                    penetration_vector = normal;
                    penetration_depth = distance;
                    return true;
                }
                else
                {
                    simplex.Insert(index, sup);
                }
            }
  
        }
        
        void getNearestEdge(List<Vector2> simplex,ref Float distance, ref Vector2 normal,ref int index)
        {
            distance = 1000f;
  
            for(int i = 0; i < simplex.Count; i++)
            {
                int j;
                if(i+1 == simplex.Count)
                    j = 0;
                else
                    j = i+1;
   
                Vector2 v1 = simplex[i];
                Vector2 v2 = simplex[j];
   
                Vector2 edge = v2-v1;
   
                Vector2 originTov1 = v1;
   
                Vector2 n = originTov1*(edge*edge) - edge*(edge*originTov1); //triple product to get vector from edge towards the originTov1
                n = n.normalized;//n/sqrt(pow(n.x,2)+pow(n.y,2)+pow(n.z,2)); //normalize
                var dist = n*v1; //distance from origin to edge
    
                if(dist < distance)
                {
                    distance = dist;
                    index = j;
                    normal = n;
                }
            }
        }

        #endregion
        
        #region gjk
        private List<Vector2> Simplex = new List<Vector2>();
        //Simplex is a list of points declared globally 
        Vector2 GetGjkDepth(Shape shape1, Shape shape2)
        {
            Vector2 d = new Vector2(1, -1);
            Simplex.Add(support(shape1, shape2, d));
            
            // negate d for the next point
            d.Negate();
            var isCollide = false;
            // start looping

            int loopMax = LOOP_MAX;
            while (true)
            {
                {
                    loopMax--;
                    if (loopMax < 0)
                    {
                        throw LOOP_MAX_EXCEPTION;
                        break;
                    }
                }
                // add a new point to the simplex because we haven't terminated yet
                Simplex.Add(support(shape1, shape2, d));

                
                // make sure that the last point we added actually passed the origin
                if (Simplex[Simplex.Count-1].Dot(d) <= 0)
                {
                    // if the point added last was not past the origin in the direction of d
                    // then the Minkowski Sum cannot possibly contain the origin since
                    // the last point added is on the edge of the Minkowski Difference
                    isCollide = false;
                    break;
                }
                else
                {
                    if (containsOrigin(ref d) )//also change direction
                    {
                        // if it does then we know there is a collision
                        isCollide = true;
                        break;
                    }
                }
            }
            
            if (isCollide)
            {
                Float penetration_depth;
                Vector2 penetration_vector;
                EPA(shape1, shape2, Simplex, out penetration_depth, out penetration_vector);
                return penetration_vector * penetration_depth;
            }
            else
            {
                return Vector2.zero;
            }
        }
        
        
        private bool containsOrigin(ref Vector2  d)
        {
            // get the last point added to the simplex
            Vector2 a = Simplex.Last();
            // compute AO (same thing as -A)
           Vector2 ao = new Vector2(-a.x, -a.y);

            //triangle  ABC
            if (Simplex.Count() == 3)
            {
               
                // get b and c
                Vector2 b = Simplex[1];
                Vector2 c = Simplex[0];
                
                Vector2 ab = b - a;
                Vector2 ac = c - a;
           
                //direction perpendicular to AB
                d=new Vector2(-ab.y,ab.x);
                //away from C
                if(d.Dot(c)>0)// if same direction, make d opposite
                {
                    d.Negate();
                }
                                
                //If the new vector (d) perpenicular on AB is in the same direction with the origin (A0)
                //it means that C is the furthest from origin and remove to create a new simplex
                if(d.Dot(ao)>0)//same direction
                {
                    Simplex.Remove(c);
                    return false;
                }

                //direction to be perpendicular to AC
                d = new Vector2(-ac.y, ac.x);

                //away form B
                if(d.Dot(b) >0)
                {
                    d.Negate();
                }
                
                //If the new vector (d) perpenicular on AC edge, is in the same direction with the origin (A0)
                //it means that B is the furthest from origin and remove to create a new simplex
                             
                if(d.Dot(ao) > 0)
                {
                    Simplex.Remove(b);
                    return false;
                }

               //origin must be inside the triangle, so this is the simplex
                return true;
            }

            //line
            else
            {
                // then its the line segment case
                var b = Simplex[0];
                // compute AB
                var ab = b - a;

                //direction perpendicular to ab, to orgin: ABXAOXAB
                d = new Vector2(-ab.y, ab.x);
                if(d.Dot(ao)<0)
                {
                    d.Negate();
                }
                   
                              
            }
            return false;
        }

        private Vector2 tripleProduct(Vector2 a,Vector2 b,Vector2 c)
        {
            //(A x B) x C = B(C.dot(A)) – A(C.dot(B)) 
            var d1=c.Dot(a);
            var d2=c.Dot(b);

            var v1 = new Vector2(d1 * b.x, d1 * b.y);
            var v2 = new Vector2(d2 * a.x, d2 * a.x);
            return v1 - v2;
        }
        
        private Vector2 support(Shape shape1, Shape shape2, Vector2 d)
        {
           // get points on the edge of the shapes in opposite directions
            Vector2 p1 = shape1.getFarthestPointInDirection(d);
            Vector2 p2 = shape2.getFarthestPointInDirection(new Vector2(-d.x,-d.y));


            // Minkowski Difference
            Vector2 p3 = new Vector2((p1 - p2).x, (p1 - p2).y);

            // p3 is now a point in Minkowski space on the edge of the Minkowski Difference
            return p3;
        }
        
        Vector2 getFarthestPointInDirection(Vector2 d)
        {
            int index = 0;
            var maxDot = this.pList[index].Dot(d);

            for (int i = 1; i < pList.Count; i++)
            {
                var dot = Vector2.Dot(pList[i], d);

                if (dot > maxDot)
                {
                    maxDot = dot;
                    index = i;
                }
            }

            return pList[index];
        }

        #endregion
        
    }
}