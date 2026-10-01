using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocity = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"), 
            0f, 
            Input.GetAxisRaw("Vertical"));

        transform.position += playerDirection.normalized * velocity * Time.deltaTime;
        
    }
}
