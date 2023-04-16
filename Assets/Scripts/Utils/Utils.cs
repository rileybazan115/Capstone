using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
	public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontsize = 40,
		Color color = default(Color), TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 1)
	{
		if (color == null) color = Color.white;
		return CreateWorldText(parent, text, localPosition, fontsize, (Color)color, textAnchor, textAlignment, sortingOrder);
	}

	public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition,
		int fontsize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
	{
		GameObject gameObject = new GameObject("World_text", typeof(TextMesh));
		Transform transform = gameObject.transform;
		transform.SetParent(parent, false);
		transform.localPosition = localPosition;
		TextMesh textMesh = gameObject.GetComponent<TextMesh>();
		textMesh.anchor = textAnchor;
		textMesh.alignment = textAlignment;
		textMesh.text = text;
		textMesh.fontSize = fontsize;
		textMesh.color = color;
		textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
		return textMesh;
	}

	public static Vector3 GetMouseWorldPosition3D()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
		{
			//Debug.DrawLine(Input.mousePosition, hit.point, Color.white, 100f);
			hit.point += Vector3.up;
			return hit.point;
		}
		return Vector3.zero;
	}

	public static Vector3 GetMouseWorldPosition()
	{
		Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
		vec.z = -20f;
		return vec;
	}

	public static Vector3 GetMouseWorldPositionWithZ()
	{
		return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
	}

	public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
	{
		return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
	}

	public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
	{
		Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
		Debug.DrawLine(worldPosition, screenPosition, Color.red, 100f);
		return worldPosition;
	}
}
