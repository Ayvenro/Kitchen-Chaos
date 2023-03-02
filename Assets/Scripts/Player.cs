using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }
    [SerializeField] private InputAction move;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float rotateSpeed = 9f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject headObject;
    [SerializeField] private LayerMask countersLayerMask;
    private float playerRadius = 0.7f;
    private bool isWalking;
    private Vector3 lastInteractDirection;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more then one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnUseAction += GameInput_OnUseAction;
    }

    private void GameInput_OnUseAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }


    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, headObject.transform.position, playerRadius, moveDirection, moveDistance);
        if (!canMove)
        {
            canMove = DiagonalMovementNearWall(ref moveDirection, moveDistance);

        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }
        isWalking = moveDirection != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private bool DiagonalMovementNearWall(ref Vector3 moveDirection, float moveDistance)
    {
        bool canMove;
        Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
        canMove = !Physics.CapsuleCast(transform.position, headObject.transform.position, playerRadius, moveDirectionX, moveDistance);
        if (canMove)
        {
            moveDirection = moveDirectionX;
        }
        else
        {
            Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
            canMove = !Physics.CapsuleCast(transform.position, headObject.transform.position, playerRadius, moveDirectionZ, moveDistance);
            if (canMove)
            {
                moveDirection = moveDirectionZ;
            }
        }

        return canMove;
    }
    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public bool IsWalking()
    {
        return isWalking;
    }

}
