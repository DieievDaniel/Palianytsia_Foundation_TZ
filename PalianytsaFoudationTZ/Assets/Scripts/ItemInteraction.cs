using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteraction : MonoBehaviour
{
    private GameInput gameInput;
    private GameObject pickedItem;
    private bool isCarryingItem = false;

    public Transform carryTransform; // Позиция, в которой будет держаться поднятый предмет
    public float throwForce = 10f; // Сила броска предмета
    public float maxInteractionDistance = 1f; // Максимальное расстояние для взаимодействия с предметом

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
                    // Поднимаем предмет
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
            // Бросаем предмет
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
}
