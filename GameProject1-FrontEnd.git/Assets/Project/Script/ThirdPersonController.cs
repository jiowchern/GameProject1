using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour
{

    public GameObject player;
    public GameObject mainCamera;
    public GameObject cameraCollisionBox;
    public CollisionCounter probe;
    public CollisionCounter cameraProbe;

    public float Maxdistance;
    public float moveSpeed;
    public float verticalRatio;


    private bool isHoriMove;
    private bool isVertiMove;
    private Vector3 horiVelocity;
    private Vector3 vertiVelocity;


    void CalculateHeight()
    {
        Vector3 originVector = mainCamera.transform.position;
        Vector3 localVector = mainCamera.transform.localPosition;

        // REMOVE Y to caculate XZ magnitude
        originVector.y = player.transform.position.y;

        float y = Maxdistance - (originVector - player.transform.position).magnitude;
        localVector.y = y;
        mainCamera.transform.localPosition = localVector * verticalRatio;
    }

    void Draw()
    {
        var rig = player.GetComponent<Rigidbody>();
        if (this.isHoriMove && this.isVertiMove)
        {

            rig.velocity = this.horiVelocity + this.vertiVelocity;
        }
        else if (this.isHoriMove)
        {
            rig.velocity = this.horiVelocity;
        }
        else if (this.isVertiMove)
        {
            rig.velocity = this.vertiVelocity;
        }

        if (this.isHoriMove || this.isVertiMove)
        {
            
            // Caculate the player's degree from velocity
            float rotate = Mathf.Atan2(rig.velocity.x, rig.velocity.z);
            player.transform.rotation = Quaternion.Euler(0, rotate / Mathf.PI * 180, 0);

        }
        else
        {
            rig.velocity = Vector3.zero;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
            return;
        // GET INPUT AXIS
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        // GET DIRECTION
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        //Calibrate Y AXIS 
        forward.y = 0;
        right.y = 0;
        // GET DIRECTION UNIT VECTOR
        forward.Normalize();
        right.Normalize();

        Vector3 distance = (mainCamera.transform.position - player.transform.position);
        distance.y = 0;

        //RESET ROTATION 
        this.isHoriMove = false;
        this.isVertiMove = false;
        this.horiVelocity = Vector3.zero;
        this.vertiVelocity = Vector3.zero;

        if (vertical > 0)
        {
            this.vertiVelocity = forward * moveSpeed;
            //Camera chases the player
            if (distance.magnitude > Maxdistance)
            {
                float originY = cameraCollisionBox.transform.position.y;
                Vector3 newPosition = player.transform.position + distance.normalized * Maxdistance;
                newPosition.y = originY;
                cameraCollisionBox.transform.position = newPosition;
            }

            this.CalculateHeight();
            isVertiMove = true;

        }
        else if (vertical < 0)
        {
            this.vertiVelocity = -forward * moveSpeed;
            if (cameraProbe.counter <= 0)
            {
                //Camera chases the player
                float originY = cameraCollisionBox.transform.position.y;
                Vector3 newPosition = player.transform.position + distance.normalized * Maxdistance;
                newPosition.y = originY;
                cameraCollisionBox.transform.position = newPosition;
            }
            else
            {
                //Camera move higher to see the player
                this.CalculateHeight();
            }
            isVertiMove = true;
        }

        if (horizontal > 0)
        {
            if (probe.counter > 0)
            {
                cameraCollisionBox.transform.position += -right * moveSpeed * Time.deltaTime;
            }
            this.horiVelocity = right * moveSpeed;
            isHoriMove = true;
        }
        else if (horizontal < 0)
        {
            if (probe.counter > 0)
            {
                cameraCollisionBox.transform.position += right * moveSpeed * Time.deltaTime;
            }
            this.horiVelocity = -right * moveSpeed;
            isHoriMove = true;
        }

        // perform the result on charactor
        Draw();
    }
}