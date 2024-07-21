using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CubeBulletMeshCreator : MonoBehaviour
{
    private const float cubeSize = 0.3f;
    private const float randomness = 0.1f;

    private Mesh mesh;

    private void Awake()
    {
        GenerateMesh();
        
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = (Material) Resources.Load("BulletMaterial", typeof(Material));
    }

    public void GenerateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[8];
        //Add vertexes
        vertices[0] = new Vector3(-1, -1, -1) * cubeSize;
        vertices[1] = new Vector3(1, -1, -1) * cubeSize;
        vertices[2] = new Vector3(1, 1, -1) * cubeSize;
        vertices[3] = new Vector3(-1, 1, -1) * cubeSize;
        vertices[4] = new Vector3(-1, -1, 1) * cubeSize;
        vertices[5] = new Vector3(1, -1, 1) * cubeSize;
        vertices[6] = new Vector3(1, 1, 1) * cubeSize;
        vertices[7] = new Vector3(-1, 1, 1) * cubeSize;

        //Randomize vertexes positions
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] += new Vector3(
                Random.Range(-randomness, randomness),
                Random.Range(-randomness, randomness),
                Random.Range(-randomness, randomness)
            );
        }

        //Create surfaces
        int[] triangles = {
            0, 2, 1,
			0, 3, 2,

            4, 5, 6,
			4, 6, 7,

            3, 7, 2,
			2, 7, 6,

            0, 1, 4,
			1, 5, 4,

            0, 7, 3,
			0, 4, 7,

            1, 2, 5,
			2, 6, 5
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}