using UnityEngine;

public class DecalDrawer : MonoBehaviour
{
    private RenderTexture renderTexture;
    [SerializeField] private Texture2D brushTexture;

    private void Start()
    {
        renderTexture = new RenderTexture((int)(1000 * transform.localScale.x), (int)(1000 * transform.localScale.y), 16, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = renderTexture;
        }
    }

    public void DrawDecal(RaycastHit hit)
    {
        Vector2 uv = hit.textureCoord;
        RenderTexture.active = renderTexture;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0);
        Graphics.DrawTexture(new Rect(uv.x * renderTexture.width - brushTexture.width / 2, (1 - uv.y) * renderTexture.height - brushTexture.height / 2, brushTexture.width, brushTexture.height), brushTexture);

        GL.PopMatrix();
        RenderTexture.active = null;
    }
}