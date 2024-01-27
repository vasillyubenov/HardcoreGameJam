using UnityEngine;

public class SpawnOnCollision : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public string collisionTag = "";
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == collisionTag)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
    }
}