using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float jumpSpeed = 7f;

    public HUDManager hud;

    Rigidbody rb;
    bool pressedJump = false;
    Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        WalkHandler();
        JumpHandler();
        hud.Refresh();
    }

    void WalkHandler()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis * walkSpeed * Time.deltaTime, 0f, vAxis * walkSpeed*Time.deltaTime);

        Vector3 currPosition = transform.position;

        Vector3 newPosition = currPosition + movement;

        rb.MovePosition(newPosition);

    }

    void JumpHandler()
    {
        float jAxis = Input.GetAxis("Jump");
        bool isGrounded = CheckGrounded();

        if(jAxis > 0f)
        {

            if(!pressedJump && isGrounded)
            {
                pressedJump = true;
                Vector3 jumpVector = new Vector3(0f, jumpSpeed, 0f);
                rb.velocity += jumpVector;
            }
            else
            {
                pressedJump = false;
            }

            
            
        }
    }

    // Check if the object is grounded
    bool CheckGrounded()
    {
        // Object size in x
        float sizeX = coll.bounds.size.x;
        float sizeZ = coll.bounds.size.z;
        float sizeY = coll.bounds.size.y;

        // Position of the 4 bottom corners of the game object
        // We add 0.01 in Y so that there is some distance between the point and the floor
        Vector3 corner1 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + 0.01f, sizeZ / 2);
        Vector3 corner2 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + 0.01f, sizeZ / 2);
        Vector3 corner3 = transform.position + new Vector3(sizeX / 2, -sizeY / 2 + 0.01f, -sizeZ / 2);
        Vector3 corner4 = transform.position + new Vector3(-sizeX / 2, -sizeY / 2 + 0.01f, -sizeZ / 2);

        // Send a short ray down the cube on all 4 corners to detect ground
        bool grounded1 = Physics.Raycast(corner1, new Vector3(0, -1, 0), 0.01f);
        bool grounded2 = Physics.Raycast(corner2, new Vector3(0, -1, 0), 0.01f);
        bool grounded3 = Physics.Raycast(corner3, new Vector3(0, -1, 0), 0.01f);
        bool grounded4 = Physics.Raycast(corner4, new Vector3(0, -1, 0), 0.01f);

        // If any corner is grounded, the object is grounded
        return (grounded1 || grounded2 || grounded3 || grounded4);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            GameManagerScript.instance.IncreaseScore(1);
            hud.Refresh();
        }
        else if(other.gameObject.tag == "Enemy")
        {
            GameManagerScript.instance.ResetGame();
        }
        else if (other.gameObject.tag == "Exit")
        {
            GameManagerScript.instance.IncreaseLevel();
        }


    }
}
