using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody2D _rb2D;
    private LineRenderer _lineRenderer;

    [SerializeField]
    private float _baseLinearDamping;

    [SerializeField] 
    private float _sandLinearDamping;


    private Vector3 _respawnPoint;

    private bool _isDragging;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    void Launch(Vector2 force)
    {
        _rb2D.linearVelocity = force * _speed;

        //change spawn sur la position du dernier tir
        _respawnPoint = transform.position;


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
        ScoreManager.Instance?.AddLevelScore();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("LevelEnd"))
        {
            UIManager.Instance?.LevelFinished();
        }

        if (other.gameObject.CompareTag("Collectible"))
        {
            //Détruit l'objet
            Destroy(other.gameObject);

            //ajouter a l'inventaire quand collision avec Player
            Inventory.instance.AddCoins(1);

            Debug.Log(PlayerPrefs.GetInt("ArgentTest").ToString());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hole"))
        {
            transform.position = _respawnPoint;

            _rb2D.linearVelocity = new Vector2(0,0);
        }

        if (collision.gameObject.CompareTag("Sand"))
        {
            _rb2D.linearDamping = _sandLinearDamping;

            Debug.LogError("Ball Slow");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sand"))
        {
            _rb2D.linearDamping = _baseLinearDamping;

            Debug.LogError("Ball no slow");
        }
    }
}