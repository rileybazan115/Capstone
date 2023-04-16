using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtils : MonoBehaviour
{
	private static Quaternion[] cachedQuaternionEulerArr;
	//Might need to make parameter for which axis
	private static void CacheQuaternionEuler()
	{
		if (cachedQuaternionEulerArr != null) return;
		cachedQuaternionEulerArr = new Quaternion[360];
		for (int i = 0; i < 360; i++)
		{
			cachedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
		}
	}

	private static Quaternion GetQuaternionEuler(float rotFloat)
	{
		int rot = Mathf.RoundToInt(rotFloat);
		rot = rot % 360;
		if (rot < 0) rot += 360;
		//if (rot >= 360) rot -= 360;
		if (cachedQuaternionEulerArr == null) CacheQuaternionEuler();
		return cachedQuaternionEulerArr[rot];
	}

	public static Mesh CreateEmptyMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[0];
		mesh.uv = new Vector2[0];
		mesh.triangles = new int[0];
		return mesh;
	}
	public static void CreateEmptyMeshArrays2d(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
	{
		vertices = new Vector3[4 * quadCount];
		uvs = new Vector2[4 * quadCount];
		triangles = new int[6 * quadCount];
	}

	//////////////
	public static void CreateEmptyMeshArrays3d(int cubeCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
	{
		vertices = new Vector3[24 * cubeCount];
		uvs = new Vector2[24 * cubeCount];
		triangles = new int[36 * cubeCount];
	}

	public static Mesh CreateMesh(Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
	{
		return AddToMesh(null, pos, rot, baseSize, uv00, uv11);
	}

	public static Mesh AddToMesh(Mesh mesh, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
	{
		if (mesh == null)
		{
			mesh = CreateEmptyMesh();
		}
		Vector3[] vertices = new Vector3[4 + mesh.vertices.Length];
		Vector2[] uvs = new Vector2[4 + mesh.uv.Length];
		int[] triangles = new int[6 + mesh.triangles.Length];

		mesh.vertices.CopyTo(vertices, 0);
		mesh.uv.CopyTo(uvs, 0);
		mesh.triangles.CopyTo(triangles, 0);

		int index = vertices.Length / 4 - 1;
		//Relocate vertices
		int vIndex = index * 4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex + 1;
		int vIndex2 = vIndex + 2;
		int vIndex3 = vIndex + 3;

		baseSize *= .5f;

		bool skewed = baseSize.x != baseSize.y;
		if (skewed)
		{
			vertices[vIndex0] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, baseSize.y);
			vertices[vIndex1] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex2] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y);
			vertices[vIndex3] = pos + GetQuaternionEuler(rot) * baseSize;
		}
		else
		{
			vertices[vIndex0] = pos + GetQuaternionEuler(rot - 270) * baseSize;
			vertices[vIndex1] = pos + GetQuaternionEuler(rot - 180) * baseSize;
			vertices[vIndex2] = pos + GetQuaternionEuler(rot - 90) * baseSize;
			vertices[vIndex3] = pos + GetQuaternionEuler(rot - 0) * baseSize;
		}

		//Relocate UVs
		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

		//Create triangles
		int tIndex = index * 6;

		triangles[tIndex + 0] = vIndex0;
		triangles[tIndex + 1] = vIndex3;
		triangles[tIndex + 2] = vIndex1;

		triangles[tIndex + 3] = vIndex1;
		triangles[tIndex + 4] = vIndex3;
		triangles[tIndex + 5] = vIndex2;

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;

		//mesh.bounds = bounds;

		return mesh;
	}
	public static void AddToMeshArrays2d(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
	{
		//Relocate vertices
		int vIndex = index * 4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex + 1;
		int vIndex2 = vIndex + 2;
		int vIndex3 = vIndex + 3;

		baseSize *= .5f;

		bool skewed = baseSize.x != baseSize.y;
		if (skewed)
		{
			vertices[vIndex0] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, baseSize.y);
			vertices[vIndex1] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex2] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y);
			vertices[vIndex3] = pos + GetQuaternionEuler(rot) * baseSize;
		}
		else
		{
			vertices[vIndex0] = pos + GetQuaternionEuler(rot - 270) * baseSize;
			vertices[vIndex1] = pos + GetQuaternionEuler(rot - 180) * baseSize;
			vertices[vIndex2] = pos + GetQuaternionEuler(rot - 90) * baseSize;
			vertices[vIndex3] = pos + GetQuaternionEuler(rot - 0) * baseSize;
		}

		//Relocate UVs
		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

		//Create triangles
		int tIndex = index * 6;

		triangles[tIndex + 0] = vIndex0;
		triangles[tIndex + 1] = vIndex3;
		triangles[tIndex + 2] = vIndex1;

		triangles[tIndex + 3] = vIndex1;
		triangles[tIndex + 4] = vIndex3;
		triangles[tIndex + 5] = vIndex2;
	}

	/////////////////
	public static void AddToMeshArrays3d(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
	{
		//Relocate vertices
		//0123, 4567, 891011
		//might only need the first 4
		int vIndex = index * 4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex + 1;
		int vIndex2 = vIndex + 2;
		int vIndex3 = vIndex + 3;

		//this affected front
		int vIndex4 = vIndex;
		int vIndex5 = vIndex + 1;
		int vIndex6 = vIndex + 2;
		int vIndex7 = vIndex + 3;

		/*int vIndex8 = vIndex;
		int vIndex9 = vIndex + 1;
		int vIndex10 = vIndex + 2;
		int vIndex11 = vIndex + 3;

		int vIndex12 = vIndex;
		int vIndex13 = vIndex + 1;
		int vIndex14 = vIndex + 2;
		int vIndex15 = vIndex + 3;

		int vIndex16 = vIndex;
		int vIndex17 = vIndex + 1;
		int vIndex18 = vIndex + 2;
		int vIndex19 = vIndex + 3;

		int vIndex20 = vIndex;
		int vIndex21 = vIndex + 1;
		int vIndex22 = vIndex + 2;
		int vIndex23 = vIndex + 3;*/

		baseSize *= .5f;

		bool skewed = baseSize.x != baseSize.y;
		if (skewed)
		{
			vertices[vIndex0] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, baseSize.y, 0);
			vertices[vIndex1] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y, 0);
			vertices[vIndex2] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y, 0);
			vertices[vIndex3] = pos + GetQuaternionEuler(rot) * baseSize;

			vertices[vIndex4] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, baseSize.y, 0);
			vertices[vIndex5] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y, 0);
			vertices[vIndex6] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y, baseSize.z);
			vertices[vIndex7] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, baseSize.y, baseSize.z);

			/*vertices[vIndex8] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, baseSize.y);
			vertices[vIndex9] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex10] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y);
			vertices[vIndex11] = pos + GetQuaternionEuler(rot) * baseSize;

			vertices[vIndex12] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, baseSize.y);
			vertices[vIndex13] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex14] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y);
			vertices[vIndex15] = pos + GetQuaternionEuler(rot) * baseSize;

			vertices[vIndex16] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, baseSize.y);
			vertices[vIndex17] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex18] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y);
			vertices[vIndex19] = pos + GetQuaternionEuler(rot) * baseSize;

			vertices[vIndex20] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, baseSize.y);
			vertices[vIndex21] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex22] = pos + GetQuaternionEuler(rot) * new Vector3(baseSize.x, -baseSize.y);
			vertices[vIndex23] = pos + GetQuaternionEuler(rot) * baseSize;*/
		}
		else
		{
			//front //assigns each corner of the quad, should be right
			//i think some are rotating around wrong axis in quaternioneuler
			//maybe not, were still visible with i on wrong axis, just rotated
			vertices[vIndex0] = pos + GetQuaternionEuler(rot - 270) * baseSize; //270
			vertices[vIndex1] = pos + GetQuaternionEuler(rot - 180) * baseSize; //180
			vertices[vIndex2] = pos + GetQuaternionEuler(rot - 90) * baseSize;  //90
			vertices[vIndex3] = pos + GetQuaternionEuler(rot - 0) * baseSize;   //0

			//back //positive got rid of the one square
			vertices[vIndex0] = pos + GetQuaternionEuler(rot - 270) * baseSize;
			vertices[vIndex1] = pos + GetQuaternionEuler(rot - 180) * baseSize;
			vertices[vIndex2] = pos + GetQuaternionEuler(rot - 90) * baseSize;
			vertices[vIndex3] = pos + GetQuaternionEuler(rot - 0) * baseSize;

			/*vertices[vIndex8] = pos + GetQuaternionEuler(rot - 270) * baseSize;
			vertices[vIndex9] = pos + GetQuaternionEuler(rot - 180) * baseSize;
			vertices[vIndex10] = pos + GetQuaternionEuler(rot - 90) * baseSize;
			vertices[vIndex11] = pos + GetQuaternionEuler(rot - 0) * baseSize;

			vertices[vIndex12] = pos + GetQuaternionEuler(rot - 270) * baseSize;
			vertices[vIndex13] = pos + GetQuaternionEuler(rot - 180) * baseSize;
			vertices[vIndex14] = pos + GetQuaternionEuler(rot - 90) * baseSize;
			vertices[vIndex15] = pos + GetQuaternionEuler(rot - 0) * baseSize;

			vertices[vIndex16] = pos + GetQuaternionEuler(rot - 270) * baseSize;
			vertices[vIndex17] = pos + GetQuaternionEuler(rot - 180) * baseSize;
			vertices[vIndex18] = pos + GetQuaternionEuler(rot - 90) * baseSize;
			vertices[vIndex19] = pos + GetQuaternionEuler(rot - 0) * baseSize;

			vertices[vIndex20] = pos + GetQuaternionEuler(rot - 270) * baseSize;
			vertices[vIndex21] = pos + GetQuaternionEuler(rot - 180) * baseSize;
			vertices[vIndex22] = pos + GetQuaternionEuler(rot - 90) * baseSize;
			vertices[vIndex23] = pos + GetQuaternionEuler(rot - 0) * baseSize;*/
		}

		//Relocate UVs
		//doesnt make it black, but lets it change color??
		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

		/*uvs[vIndex8] = new Vector3(uv00.x, uv11.y);
		uvs[vIndex9] = new Vector3(uv00.x, uv00.y);
		uvs[vIndex10] = new Vector3(uv11.x, uv00.y);
		uvs[vIndex11] = new Vector3(uv11.x, uv11.y);

		uvs[vIndex12] = new Vector3(uv00.x, uv11.y);
		uvs[vIndex13] = new Vector3(uv00.x, uv00.y);
		uvs[vIndex14] = new Vector3(uv11.x, uv00.y);
		uvs[vIndex15] = new Vector3(uv11.x, uv11.y);

		uvs[vIndex16] = new Vector3(uv00.x, uv11.y);
		uvs[vIndex17] = new Vector3(uv00.x, uv00.y);
		uvs[vIndex18] = new Vector3(uv11.x, uv00.y);
		uvs[vIndex19] = new Vector3(uv11.x, uv11.y);

		uvs[vIndex20] = new Vector3(uv00.x, uv11.y);
		uvs[vIndex21] = new Vector3(uv00.x, uv00.y);
		uvs[vIndex22] = new Vector3(uv11.x, uv00.y);
		uvs[vIndex23] = new Vector3(uv11.x, uv11.y);*/

		int tIndex = index * 6;

		//front
		triangles[tIndex + 0] = vIndex2;
		triangles[tIndex + 1] = vIndex1;
		triangles[tIndex + 2] = vIndex0;

		triangles[tIndex + 3] = vIndex0;
		triangles[tIndex + 4] = vIndex3;
		triangles[tIndex + 5] = vIndex2;

		//spawns back square, but in same spot as front but only on one square?
		triangles[tIndex + 6] = vIndex0;
		triangles[tIndex + 7] = vIndex1;
		triangles[tIndex + 8] = vIndex2;

		triangles[tIndex + 9] = vIndex2;
		triangles[tIndex + 10] = vIndex3;
		triangles[tIndex + 11] = vIndex0;

		//? no effect, might be squished sideways same for rest //might be wrong wise //testing a bunch had no effect
		/*triangles[tIndex + 12] = vIndex8;//10
		triangles[tIndex + 13] = vIndex9;//9
		triangles[tIndex + 14] = vIndex10;//8

		triangles[tIndex + 15] = vIndex9;//11
		triangles[tIndex + 16] = vIndex10;//10
		triangles[tIndex + 17] = vIndex11;//9

		//?
		triangles[tIndex + 18] = vIndex2;//14
		triangles[tIndex + 19] = vIndex1;//13
		triangles[tIndex + 20] = vIndex0;//12

		triangles[tIndex + 21] = vIndex0;//15
		triangles[tIndex + 22] = vIndex3;//14
		triangles[tIndex + 23] = vIndex2;//12

		//?
		triangles[tIndex + 24] = vIndex2;//2
		triangles[tIndex + 25] = vIndex1;//6
		triangles[tIndex + 26] = vIndex0;//5

		triangles[tIndex + 27] = vIndex0;//2
		triangles[tIndex + 28] = vIndex3;//5
		triangles[tIndex + 29] = vIndex2;//1

		//?
		triangles[tIndex + 30] = vIndex2;//7
		triangles[tIndex + 31] = vIndex1;//3
		triangles[tIndex + 32] = vIndex0;//4

		triangles[tIndex + 33] = vIndex0;//4
		triangles[tIndex + 34] = vIndex3;//3
		triangles[tIndex + 35] = vIndex2;//0*/
	}
}
