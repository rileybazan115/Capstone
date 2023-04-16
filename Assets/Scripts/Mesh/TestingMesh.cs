using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMesh : MonoBehaviour
{
    private void Start()
    {
		//all corrdinates are clockwise
		//cube
		//unity cube has 24 vertices
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[24];
		Vector2[] uv = new Vector2[24];
		int[] triangles = new int[36];

		//vertices
		{
			//front
			vertices[0] = new Vector3(0, 0, 0);
			vertices[1] = new Vector3(0, 10, 0);
			vertices[2] = new Vector3(10, 10, 0);
			vertices[3] = new Vector3(10, 0, 0);

			//back
			vertices[4] = new Vector3(0, 0, 10);
			vertices[5] = new Vector3(0, 10, 10);
			vertices[6] = new Vector3(10, 10, 10);
			vertices[7] = new Vector3(10, 0, 0);

			//left
			vertices[8] = new Vector3(0, 0, 0);
			vertices[9] = new Vector3(0, 0, 10);
			vertices[10] = new Vector3(0, 10, 10);
			vertices[11] = new Vector3(0, 10, 0);

			//right
			vertices[12] = new Vector3(10, 0, 0);
			vertices[13] = new Vector3(10, 10, 0);
			vertices[14] = new Vector3(10, 10, 10);
			vertices[15] = new Vector3(10, 0, 10);

			//top
			vertices[16] = new Vector3(0, 10, 0);
			vertices[17] = new Vector3(0, 10, 10);
			vertices[18] = new Vector3(10, 10, 10);
			vertices[19] = new Vector3(10, 10, 0);

			//bottom
			vertices[20] = new Vector3(0, 0, 0);
			vertices[21] = new Vector3(10, 0, 0);
			vertices[22] = new Vector3(10, 0, 10);
			vertices[23] = new Vector3(0, 0, 10);
		}


		//uvs
		{
			//front
			uv[0] = new Vector3(0, 0, 0);
			uv[1] = new Vector3(0, 1, 0);
			uv[2] = new Vector3(1, 1, 0);
			uv[3] = new Vector3(1, 0, 0);
			
			//back
			uv[4] = new Vector3(0, 0, 1);
			uv[5] = new Vector3(0, 1, 1);
			uv[6] = new Vector3(1, 1, 1);
			uv[7] = new Vector3(1, 0, 0);
			
			//left
			uv[8] = new Vector3(0, 0, 0);
			uv[9] = new Vector3(0, 0, 1);
			uv[10] = new Vector3(0, 1, 1);
			uv[11] = new Vector3(0, 1, 0);
			
			//right
			uv[12] = new Vector3(1, 0, 0);
			uv[13] = new Vector3(1, 1, 0);
			uv[14] = new Vector3(1, 1, 1);
			uv[15] = new Vector3(1, 0, 1);
			
			//top
			uv[16] = new Vector3(0, 1, 0);
			uv[17] = new Vector3(0, 1, 1);
			uv[18] = new Vector3(1, 1, 1);
			uv[19] = new Vector3(1, 1, 0);
			
			//bottom
			uv[20] = new Vector3(0, 0, 0);
			uv[21] = new Vector3(1, 0, 0);
			uv[22] = new Vector3(1, 0, 1);
			uv[23] = new Vector3(0, 0, 1);
		}
		

		//cube triangles
		{
			//front top correct
			triangles[0] = 0;
			triangles[1] = 1;
			triangles[2] = 2;

			//front bottom
			triangles[3] = 2;
			triangles[4] = 3;
			triangles[5] = 0;

			//back top correct
			triangles[6] = 4;
			triangles[7] = 6;
			triangles[8] = 5;

			//back bottom correct
			triangles[9] = 6;
			triangles[10] = 4;
			triangles[11] = 15; //? why 15 not 7

			//left bottom correct
			triangles[12] = 8;
			triangles[13] = 9;
			triangles[14] = 10;

			//left top correct
			triangles[15] = 8;
			triangles[16] = 10;
			triangles[17] = 11;

			//right top correct
			triangles[18] = 12;
			triangles[19] = 13;
			triangles[20] = 14;

			//right bottom correct
			triangles[21] = 12;
			triangles[22] = 14;
			triangles[23] = 15;

			//top left correct
			triangles[27] = 5;
			triangles[28] = 6;
			triangles[29] = 2;

			//top right correct
			triangles[24] = 1;
			triangles[25] = 5;
			triangles[26] = 2;

			//bottom right correct
			triangles[30] = 4;
			triangles[31] = 3;
			triangles[32] = 15;// why not 7

			//bottom left correct
			triangles[33] = 0;
			triangles[34] = 3;
			triangles[35] = 4;
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;




		/*//square
		Mesh mesh = new Mesh();

		Vector3[] vertices = new Vector3[4];
		Vector2[] uv = new Vector2[4];
		int[] triangles = new int[6];

		vertices[0] = new Vector3(0, 0);
		vertices[1] = new Vector3(0, 10);
		vertices[2] = new Vector3(10, 10);
		vertices[3] = new Vector3(10, 0);

		uv[0] = new Vector3(0, 0, 0);
		uv[1] = new Vector3(0, 1, 0);
		uv[2] = new Vector3(1, 1, 0);
		uv[3] = new Vector3(1, 0, 0);

		//set triangles to clockwise to have it front facing
		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;

		triangles[3] = 0;
		triangles[4] = 2;
		triangles[5] = 3;


		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;

		GetComponent<MeshFilter>().mesh = mesh;*/
	}

}
