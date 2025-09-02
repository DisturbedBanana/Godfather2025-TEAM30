using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Launch();
    }   
    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Wall"))
    //     {
    //         // Average the normals from all contact points
    //         Vector3 avgNormal = Vector3.zero;
    //         foreach (ContactPoint contact in collision.contacts)
    //         {
    //             avgNormal += contact.normal;
    //         }
    //         avgNormal /= collision.contacts.Length;
    //
    //         Vector3 reflect = Vector3.Reflect(rb.velocity, avgNormal);
    //
    //         float newSpeed = Mathf.Max(reflect.magnitude * 0.9f, speed * 0.5f);
    //         rb.velocity = reflect.normalized * newSpeed;
    //     }
    // }

    void Launch()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector3 direction = new Vector3(x, y, 0).normalized;
        rb.velocity = direction * speed;
    }


    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Launch();
        }
    }
}
