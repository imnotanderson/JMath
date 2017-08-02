//#define UNITY
#if UNITY
namespace JMath
{
	using UnityEngine;
	using System.Collections.Generic;

	[ExecuteInEditMode]
	public class ShapeMono : MonoBehaviour
	{
		public JMath.Shape s1;
		public List<Transform> tList = new List<Transform>();

		void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			if (s1 == null || s1.Count < 1) return;
			for (int i = 0; i < s1.Count; i++)
			{
				var j = i + 1 == s1.Count ? 0 : i + 1;
				var p0 = s1[i];
				var p1 = s1[j];
				Gizmos.DrawLine(new Vector3(p0.x.Val, p0.y.Val, 0), new Vector3(p1.x.Val, p1.y.Val));
			}
		}

		void Update()
		{
			Refresh();
		}

		[ContextMenu("refresh")]
		public void Refresh()
		{
			List<JMath.Vector2> ps = new List<Vector2>();
			ps = new List<JMath.Vector2>();
			foreach (var t in tList)
			{
				var p = t.localPosition;
				ps.Add(new JMath.Vector2(p.x, p.y));
			}
			s1 = new Shape(new Vector2(transform.position.x, transform.position.y), ps.ToArray());
		}

		[ContextMenu("MakeTrans")]
		public void MakeTrans()
		{
			for (int i = 0; i < tList.Count; i++)
			{
				if (tList[i] == null)
				{
					var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
					tList[i] = go.transform;
					go.transform.parent = transform;
					go.name = i.ToString();
				}
			}
		}

	}
}
#endif