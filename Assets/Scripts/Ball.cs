using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody2D _rb2D;
    private LineRenderer _lineRenderer;

    private bool _isDragging;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    void Launch(Vector2 force)
    {
        _rb2D.velocity = force * _speed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckForBallClick();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && _isDragging)
        {
            EndDrag();
        }

        if (!_isDragging) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, mousePos);
    }

    private void CheckForBallClick()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            StartDrag();
        }
    }

    private void StartDrag()
    {
        _isDragging = true;
        _lineRenderer.enabled = true;
    }

    private void EndDrag()
    {
        _isDragging = false;
        _lineRenderer.enabled = false;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = -(mousePos - transform.position);
        float distance = direction.magnitude;
        Launch(direction.normalized * distance);
    }
}