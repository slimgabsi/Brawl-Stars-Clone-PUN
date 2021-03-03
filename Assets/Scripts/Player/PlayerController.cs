using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviourPunCallbacks
{
   
    [SerializeField] private Transform playerMvtSprite;
    [SerializeField] Transform playerMesh;
    [SerializeField] Camera PlayerCamera;
    [SerializeField] LineRenderer PlayerAttackLine;
    [SerializeField] Transform gunTransform;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform Wolrd;



    private Vector3 attackLookAtPoint =new Vector3();


    private Joystick leftJoystick;
    private Joystick rightJoystick;
    private Animator animator;
    private float _moveSpeed;
    private bool isReadyToAttack;
    //speed can be set by manager later 
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    private void Awake()
    {
        leftJoystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        rightJoystick = GameObject.FindGameObjectWithTag("RightJoystick").GetComponent<Joystick>();
        animator = GetComponentInChildren<Animator>(); 
    }

    private void Start()
    {
        MoveSpeed = 6; 
    }

    private void FixedUpdate()
    {
       
        
        if (!photonView.IsMine)
        {
            PlayerCamera.gameObject.SetActive(false);
            this.enabled = false;
       }
        else
        {
            Move(); Attack();
        }
    }
    private void Move()
    {

        if (leftJoystick.Horizontal == 0 && leftJoystick.Vertical == 0)
        {
            playerMvtSprite.gameObject.SetActive(false);
            animator.SetBool("Run", false);
        }
        else
        {
            animator.SetBool("Run", true);
            playerMesh.LookAt(new Vector3(playerMvtSprite.position.x, playerMesh.position.y, playerMvtSprite.position.z));
            playerMesh.eulerAngles = new Vector3(0, playerMesh.eulerAngles.y, 0);
            playerMvtSprite.gameObject.SetActive(true);
            playerMvtSprite.localPosition = new Vector3(leftJoystick.Horizontal, 0.05f, leftJoystick.Vertical);
            // transform.forward = new Vector3();
            transform.Translate(new Vector3(leftJoystick.Horizontal, transform.forward.y, leftJoystick.Vertical) * Time.deltaTime * _moveSpeed);
        }
    }
    private void Attack()
    {
        if (Input.GetMouseButtonUp(0)&& isReadyToAttack)
        {
            animator.SetTrigger("Attack");
            GameObject bulletClone = Instantiate(bulletPrefab, Wolrd);
            bulletClone.transform.position = gunTransform.position;
            bulletClone.GetComponent<Rigidbody>().AddForce(gunTransform.forward * 500);

        }

        if (Mathf.Abs(rightJoystick.Horizontal) > 0.5f || Mathf.Abs(rightJoystick.Vertical) > 0.5f)
        {
            PlayerAttackLine.gameObject.SetActive(true);
            attackLookAtPoint = new Vector3(rightJoystick.Horizontal + transform.position.x, 1, rightJoystick.Vertical + transform.position.z);
            PlayerAttackLine.SetPosition(0, transform.position + 0.1f * Vector3.up);
            PlayerAttackLine.SetPosition(1, transform.position + new Vector3(rightJoystick.Horizontal * 5, 0, rightJoystick.Vertical * 5) + 0.1f * Vector3.up);
            isReadyToAttack = true;
            playerMesh.LookAt(PlayerAttackLine.GetPosition(1));
        }

        else if (Mathf.Abs(rightJoystick.Horizontal) == 0 && Mathf.Abs(rightJoystick.Vertical) == 0)
        {
            PlayerAttackLine.gameObject.SetActive(false);
            isReadyToAttack = false;      
        }
        
    }


}
