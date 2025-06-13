using UnityEngine;

public class WireCubeRenderer : MonoBehaviour
{
    public Material lineMaterial;
    public string targetTag = "Ladrillo";
    public float lineOffset = 0.01f;  // Para simular grosor

    private void OnRenderObject()
    {
        if (!lineMaterial) return;

        lineMaterial.SetPass(0);

        GameObject[] ladrillos = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject ladrillo in ladrillos)
        {
            Renderer rend = ladrillo.GetComponent<Renderer>();
            if (rend == null || !rend.isVisible) continue;

            GL.Begin(GL.LINES);
            GL.Color(Color.black);
            DrawCubeWire(ladrillo.transform.position, ladrillo.transform.localScale);
            GL.End();
        }
    }

    void DrawCubeWire(Vector3 center, Vector3 size)
    {
        Vector3 half = size / 2f + Vector3.one * lineOffset;

        Vector3[] pts = new Vector3[8];
        pts[0] = center + new Vector3(-half.x, -half.y, -half.z);
        pts[1] = center + new Vector3(half.x, -half.y, -half.z);
        pts[2] = center + new Vector3(half.x, -half.y, half.z);
        pts[3] = center + new Vector3(-half.x, -half.y, half.z);
        pts[4] = center + new Vector3(-half.x, half.y, -half.z);
        pts[5] = center + new Vector3(half.x, half.y, -half.z);
        pts[6] = center + new Vector3(half.x, half.y, half.z);
        pts[7] = center + new Vector3(-half.x, half.y, half.z);

        int[,] edges = {
            {0,1},{1,2},{2,3},{3,0},
            {4,5},{5,6},{6,7},{7,4},
            {0,4},{1,5},{2,6},{3,7}
        };

        for (int i = 0; i < 12; i++)
        {
            GL.Vertex(pts[edges[i, 0]]);
            GL.Vertex(pts[edges[i, 1]]);
        }
    }
}
