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

    [SerializeField]
    private AudioClip sandSound;

    [SerializeField]
    private AudioClip holeSound;

    [SerializeField]
    private AudioClip dragSound;

    [SerializeField]
    private AudioClip endSound;

    private Vector3 _respawnPoint;
    private bool _isDragging;
    
    private bool _canBeLaunched;
    public bool CanBeLaunched { get { return _canBeLaunched; } set { _canBeLaunched = value; } }

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
        if (hit.collider != null && hit.collider.gameObject == gameObject && CanBeLaunched)
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
        //ajoute effet sonnore quand tape la balle
        AudioManager.instance.PlayClipAt(dragSound, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("LevelEnd"))
        {
            UIManager.Instance?.LevelFinished();
            AudioManager.instance.PlayClipAt(endSound, transform.position);
        }

        if (other.gameObject.CompareTag("Collectible"))
        {
            //D�truit l'objet
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
            //respawn de la balle � ca derni�re position quand tombe dans le troue 
            transform.position = _respawnPoint;

            //stop �a velocity quand tombe dans le troue
            _rb2D.linearVelocity = new Vector2(0,0);

            //effets sonor quand tombe dans le trou
            AudioManager.instance.PlayClipAt(holeSound, transform.position);
        }

        if (collision.gameObject.CompareTag("Sand"))
        {
            //Slow balle dans le sable
            _rb2D.linearDamping = _sandLinearDamping;

            //effets sonor quand arrive dans le sable
            AudioManager.instance.PlayClipAt(sandSound, transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sand"))
        {
            _rb2D.linearDamping = _baseLinearDamping;
        }
    }
}