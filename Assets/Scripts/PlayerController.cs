using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    // ���� ����
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // ������ üũ����
    private Vector3 lastPos;

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;

    // �ΰ���
    [SerializeField]
    private float lookSensitivity;

    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0.0f;

    // �ʿ� ������Ʈ
    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;

    private GunController theGunController;
    private Crosshair theCrosshair;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        //theCamera = FindObjectOfType<Camera>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }



    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        MoveCheck();
        CameraRotationLimit();
        CharacterRotation();
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    // �ɱ� �õ�
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // �ɱ�
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else 
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
    }

    // �ε巯�� �ɱ� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.05f);
            theCamera.transform.localPosition = new Vector3(-1.0f, _posY, -1.5f);                             // 3��Ī ī�޶� ��ġ ����� �Բ� ������ ��.
            if (count > 100) break;
            yield return null;
        }

        theCamera.transform.localPosition = new Vector3(-1.0f, applyCrouchPosY, -1.5f);                       // 3��Ī ī�޶� ��ġ ����� �Բ� ������ ��.
    }

    // �ٴ� ����
    private void IsGround()
    {
        // ���� ������Ʈ�� ��ġ���� �Ʒ�����(world position)���� ĸ���ö��̴��� ����(���� ���� �� ���� = 0.1f)��ŭ Raycast �߻�
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.RunningAnimation(!isGround);
    }

    // ����
    private void Jump()
    {
        // ������ �������� ����
        if (isCrouch) Crouch();
        
        myRigid.velocity = transform.up * jumpForce;
    }

    // �޸��� �õ�
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    // �޸���
    private void Running()
    {
        // �޸��� ����� �������� ����
        if (isCrouch) Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isWalk) 
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characerRotationY = new Vector3(0.0f, _yRotation, 0.0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characerRotationY));
    }

    private void CameraRotationLimit()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");                 // ���콺 ���� ���� ������
        float _cameraRotationX = _xRotation * lookSensitivity;          // ���콺 ���ϰ��� �ΰ��� ����
        currentCameraRotationX -= _cameraRotationX;                     // �ΰ��� ����� ���� currentCameraRotationX ������ �߰����ش�. ���콺������ ���� ���ش�.(���ϸ� �Ųٷ� ������ - ������ ����, �ø��� �Ʒ���)
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);    // currentCameraRotationX �� ���� ���ð��� (+-)cameraRotationLimit ���̰����� ���Ѵ�.
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0.0f, 0.0f);                     // ī�޶� ������ �����Ų��.
    }
}
