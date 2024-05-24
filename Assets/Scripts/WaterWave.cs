using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshCollider))]
public class WaterWave : MonoBehaviour
{
	[System.Serializable]
	struct Wave
	{
		[Range(0, 10)] public float amplitude;
		[Range(0, 10)] public float length;
		[Range(0, 10)] public float roll;
		[Range(0, 10)] public float rate;
	}

	[SerializeField] Wave wave;

	[Header("Mesh Generator")]
	[SerializeField][Range(1, 80)] float xSize = 40;
	[SerializeField][Range(1, 80)] float zSize = 40;
	[SerializeField][Range(2, 80)] int xVertexNum = 40;
	[SerializeField][Range(2, 80)] int zVertexNum = 40;

	MeshFilter meshFilter;
	MeshCollider meshCollider;

	Mesh mesh;
	Vector3[] vertices;
	Vector3[,] buffer;

	void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshCollider = GetComponent<MeshCollider>();

		MeshGenerator.Plane(meshFilter, xSize, zSize, xVertexNum, zVertexNum);

		mesh = meshFilter.mesh;
		vertices = mesh.vertices;

		buffer = new Vector3[xVertexNum, zVertexNum];
	}

	Vector3 GerstnerWave(Vector3 position, float time, float length, float amplitude, float roll)
	{
		Vector3 v = new Vector3();

		v.x = Mathf.Cos((position.x * length + time) * roll);
		v.y = Mathf.Sin((position.x * length + v.x + time) * amplitude);

        return v;
	}

	void Update()
	{
		// update vertex values with wave
		for (int z = 0; z < zVertexNum; z++)
		{
			float zPosition = ((float)z / (zVertexNum - 1) - 0.5f) * xSize;
			for (int x = 0; x < xVertexNum; x++)
			{
				Vector3 p = Vector3.zero;
				p.x = ((x / (float)(xVertexNum - 1)) - 0.5f) * xSize;
				p.z = ((z / (float)(zVertexNum - 1)) - 0.5f) * zSize;
				//p.y = Mathf.Sin(p.x * wave.length + Time.time * wave.rate) * wave.amplitude;

				Vector3 offset = GerstnerWave(p, Time.time * wave.rate, wave.length, wave.amplitude, wave.roll);

				buffer[x, z] = p + offset;
			}
		}

		// set vertices from buffer
		for (int x = 0; x < xVertexNum; x++)
		{
			for (int z = 0; z < zVertexNum; z++)
			{
				vertices[x + z * xVertexNum] = buffer[x, z];
			}
		}

		// recalculate mesh with new vertice values
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		meshCollider.sharedMesh = mesh;
	}
}
