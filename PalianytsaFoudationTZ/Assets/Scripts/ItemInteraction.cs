using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteraction : MonoBehaviour
{
    private GameInput gameInput;
    private GameObject pickedItem;
    private bool isCarryingItem = false;

    public Transform carryTransform; 
    public float throwForce = 10f; 
    public float maxInteractionDistance = 1f; 

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void OnEnable()
    {
        gameInput.Enable();
        gameInput.Player.PickUp.performed += ctx => PickUp();
        gameInput.Player.ThrowAway.performed += ctx => ThrowAway();
    }

    private void OnDisable()
    {
        gameInput.Disable();
    }

    private void PickUp()
    {
        if (!isCarryingItem)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxInteractionDistance))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    pickedItem = hit.collider.gameObject;
                    pickedItem.transform.SetParent(carryTransform);
                    pickedItem.transform.localPosition = Vector3.zero;
                    isCarryingItem = true;
                }
            }
        }
    }

    private void ThrowAway()
    {
        if (isCarryingItem && pickedItem != null)
        {
            pickedItem.transform.SetParent(null);
            Rigidbody itemRB = pickedItem.GetComponent<Rigidbody>();
            if (itemRB != null)
            {
                itemRB.isKinematic = false;
                itemRB.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            }
            pickedItem = null;
            isCarryingItem = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit collision with: " + collision.gameObject.name);
    }
}
