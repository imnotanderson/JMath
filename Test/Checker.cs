//#define UNITY
#if UNITY
namespace JMath
{
	using UnityEngine;

	[ExecuteInEditMode]
	public class Checker : MonoBehaviour
	{
		public ShapeMono x1, x2;

		// Update is called once per frame
		void Update()
		{
			if (x1 == null || x2 == null) return;
			depth = x1.s1.GetGjkDepth(x2.s1);
			name = "check " + depth;
		}

		JMath.Vector2 depth = Vector2.zero;

		void OnDrawGizmos()
		{
			if (x1 == null || x2 == null) return;
			Gizmos.color = Color.red;
			DrawShape(x1.s1);
		}

		void DrawShape(Shape s1)
		{
			for (int i = 0; i < s1.pList.Count; i++)
			{
				var j = i + 1 == s1.pList.Count ? 0 : i + 1;
				var p0 = s1.pList[i];
				var p1 = s1.pList[j];
				p0 -= depth;
				p1 -= depth;
				Gizmos.DrawLine(new Vector3(p0.x.Val, p0.y.Val, 0), new Vector3(p1.x.Val, p1.y.Val));
			}
		}
	}


}
#endif