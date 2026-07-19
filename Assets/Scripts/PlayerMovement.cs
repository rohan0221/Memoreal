using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(x, 0, z) * speed * Time.deltaTime;
        controller.Move(move);
    }
}