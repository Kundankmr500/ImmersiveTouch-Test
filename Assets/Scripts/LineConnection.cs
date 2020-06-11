using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineConnection : MonoBehaviour
{
    public GameObject Atom2;
    public float StartWidth;
    public float EndWidth;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = StartWidth;
        lineRenderer.endWidth = EndWidth;

        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, Atom2.transform.position);
    }
}
