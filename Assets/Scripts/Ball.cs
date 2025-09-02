using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    private Rigidbody2D _rb2D;

    private bool _isDragging;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }   
    

    void Launch(Vector2 force)
    {
        _rb2D.velocity = force * _speed;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartDrag();
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            EndDrag();
        }

        if (!_isDragging) return;
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(transform.position, mousePos, Color.red);
    }

    private void StartDrag()
    {
        _isDragging = true;
    }
    
    private void EndDrag()
    {
        _isDragging = false;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = -(mousePos - transform.position);
        float distance = direction.magnitude;
        Launch(direction.normalized * distance);
    }
}
