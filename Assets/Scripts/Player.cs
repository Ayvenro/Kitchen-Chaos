using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private InputAction move;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float rotateSpeed = 9f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject headObject;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint; 
    private float playerRadius = 0.7f;
    private bool isWalking;
    private Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

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
        gameInput.OnUseAlternativeAction += GameInput_OnUseAlternativeAction;
    }

    private void GameInput_OnUseAlternativeAction(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            if (selectedCounter != null)
            {
                selectedCounter.InteractAlternative(this);
            }
        }

    }

    private void GameInput_OnUseAction(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            if (selectedCounter != null)
            {
                selectedCounter.Interact(this);
            }
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
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
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
            canMove = AdjustMoveDirectionNearWall(ref moveDirection, moveDistance);

        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }
        isWalking = moveDirection != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private bool AdjustMoveDirectionNearWall(ref Vector3 moveDirection, float moveDistance)
    {
        bool canMove;
        Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
        canMove = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, headObject.transform.position, playerRadius, moveDirectionX, moveDistance);
        if (canMove)
        {
            moveDirection = moveDirectionX;
        }
        else
        {
            Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
            canMove = moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, headObject.transform.position, playerRadius, moveDirectionZ, moveDistance);
            if (canMove)
            {
                moveDirection = moveDirectionZ;
            }
        }

        return canMove;
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public Transform GetTheKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
