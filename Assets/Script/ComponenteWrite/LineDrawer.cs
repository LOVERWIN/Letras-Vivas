using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class LineDrawer : MonoBehaviour, IDragHandler
{
    [System.Serializable]
    public class DrawEvent : UnityEvent<RenderTexture> { }

    [SerializeField] Vector2Int imageDimention = new Vector2Int(120, 120);
    [SerializeField] Color paintColor = Color.black;
    [SerializeField] RenderTextureFormat format = RenderTextureFormat.R8;
    [SerializeField] Shader shader;
    [SerializeField] float thickness = 3f;
    [SerializeField] DrawEvent OnDraw = new DrawEvent();

    RawImage imageView = null;
    Mesh lineMesh;
    Material lineMaterial;
    Texture2D clearTexture;
    RenderTexture texture;
    Vector3 previousPosition;
    bool isFirstPoint = true;

    void OnEnable()
    {
        imageView = GetComponent<RawImage>();

        lineMesh = new Mesh();
        lineMesh.MarkDynamic();

        lineMaterial = new Material(shader);
        lineMaterial.SetColor("_Color", paintColor);

        texture = new RenderTexture(imageDimention.x, imageDimention.y, 0, format);
        texture.filterMode = FilterMode.Bilinear;
        imageView.texture = texture;

        clearTexture = Texture2D.whiteTexture;
    }

    void Start()
    {
        ClearTexture();
    }

    void OnDisable()
    {
        texture?.Release();
        Destroy(lineMesh);
        Destroy(lineMaterial);
    }

    public void ClearTexture()
    {
        Graphics.Blit(clearTexture, texture);
        isFirstPoint = true;
    }

    public void OnDrag(PointerEventData data)
    {
        data.Use();

        var area = data.pointerDrag.GetComponent<RectTransform>();
        var currentPos = area.InverseTransformPoint(data.position);
        var previousPos = isFirstPoint ? currentPos : area.InverseTransformPoint(data.position - data.delta);

        var scale = new Vector3(2 / area.rect.width, -2 / area.rect.height, 0);
        var p0 = Vector3.Scale(previousPos, scale);
        var p1 = Vector3.Scale(currentPos, scale);

        if (!isFirstPoint)
        {
            DrawLine(p0, p1);
        }
        
        isFirstPoint = false;
        previousPosition = p1;

        OnDraw.Invoke(texture);
    }

    void DrawLine(Vector3 p0, Vector3 p1)
    {
        var prevRT = RenderTexture.active;
        RenderTexture.active = texture;

        lineMesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Segmentos adaptativos al grosor
        int segments = Mathf.Clamp(Mathf.CeilToInt(thickness * 2), 12, 32);
        float pixelRadius = thickness / imageDimention.x;

        // Cálculo de curvatura y pasos adaptativos
        float distance = Vector3.Distance(p0, p1);
        float curvature = isFirstPoint ? 0 : 1 - Vector3.Dot(
            (p0 - previousPosition).normalized, 
            (p1 - p0).normalized);
    
        int steps = Mathf.CeilToInt(distance * imageDimention.x * 0.5f * (1 + curvature * 2));

        Vector3 midPoint = (p0 + p1) * 0.5f;
        Vector3 controlPoint = midPoint + (Vector3.Cross(p1 - p0, Vector3.forward).normalized * distance * 0.2f * curvature);

        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            Vector3 point = CubicBezier(p0, controlPoint, controlPoint, p1, t);
            AddCircle(point, pixelRadius, segments, vertices, triangles);
        }

        lineMesh.vertices = vertices.ToArray();
        lineMesh.triangles = triangles.ToArray();

        lineMaterial.SetPass(0);
        Graphics.DrawMeshNow(lineMesh, Matrix4x4.identity);

        RenderTexture.active = prevRT;
        previousPosition = p0;
    }

    Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }

    void AddCircle(Vector3 center, float radius, int segments, List<Vector3> vertices, List<int> triangles)
    {
        int startIndex = vertices.Count;
        vertices.Add(center);

        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2;
            Vector3 point = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            vertices.Add(point);
        }

        for (int i = 1; i <= segments; i++)
        {
            triangles.Add(startIndex);
            triangles.Add(startIndex + i);
            triangles.Add(startIndex + (i % segments) + 1);
        }
    }
}