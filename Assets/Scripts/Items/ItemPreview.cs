using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public sealed class ItemPreview : MonoBehaviour
    {
        [SerializeField] Item item;
        [SerializeField] UIDocument uIDocument;
        [SerializeField] GameObject rootGameObject;
        [SerializeField] GameObject itemModel;
        [SerializeField] AudioClip addItemSound;

        public bool isWorldObject = false;

        EquipmentManager equipmentManager;
        InventoryManager inventoryManager;

        VisualElement root;
        Label itemName;
        Label itemDescription;
        Label itemDifference;
        VisualElement switchingAction;
        VisualElement switchingActionFillBar;

        Coroutine SwitchingCoroutine;

        const float SWITCHING_TIME = 3f;

        bool hasSetupRefs = false;

        void Awake()
        {
            if (uIDocument != null)
            {
                root = uIDocument.rootVisualElement;
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                equipmentManager = player.GetComponent<EquipmentManager>();
                inventoryManager = player.GetComponent<InventoryManager>();
            }

            SetupRefs();

            gameObject.SetActive(isWorldObject);
        }

        void SetupRefs()
        {
            if (root == null)
            {
                return;
            }

            if (hasSetupRefs)
            {
                return;
            }

            hasSetupRefs = true;

            itemName = root.Q<Label>("ItemName");
            itemDescription = root.Q<Label>("ItemDescription");
            itemDifference = root.Q<Label>("Difference");
            switchingAction = root.Q<VisualElement>("SwitchingAction");
            switchingActionFillBar = switchingAction.Q<VisualElement>("Fill");
        }

        void OnEnable()
        {
            BuildUI();
        }

        void BuildUI()
        {
            if (root == null || equipmentManager == null)
            {
                return;
            }

            itemName.text = item.displayName;
            itemDescription.text = item.GetDescription();

            CompareItem();
            switchingAction.style.display = DisplayStyle.None;
        }

        void CompareItem()
        {
            Item itemToCompare = null;
            itemDifference.text = "";

            if (item is Weapon)
            {
                itemToCompare = equipmentManager.weaponInstance?.weaponData;
            }

            if (item == null)
            {
                return;
            }

            int difference = item.Difference(itemToCompare);

            if (difference > 0)
            {
                itemDifference.text = $"(+{difference})";
                ColorUtility.TryParseHtmlString("#C2FF00", out Color greenColor);
                itemDifference.style.color = greenColor;
            }
            else if (difference < 0)
            {
                itemDifference.text = $"(-{difference})";
                ColorUtility.TryParseHtmlString("#FF1E00", out Color redColor);
                itemDifference.style.color = redColor;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                BeginSwitching();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CancelSwitching();
            }
        }

        void BeginSwitching()
        {
            CancelSwitching();

            SwitchingCoroutine = StartCoroutine(PerformSwitching());
        }

        void CancelSwitching()
        {
            SetupRefs();

            if (SwitchingCoroutine != null)
            {
                StopCoroutine(SwitchingCoroutine);
            }

            switchingAction.style.display = DisplayStyle.None;
        }

        IEnumerator PerformSwitching()
        {
            float time = 0f;

            switchingAction.style.display = DisplayStyle.Flex;

            while (time < SWITCHING_TIME)
            {
                time += Time.deltaTime;

                float percentage = time * 100f / SWITCHING_TIME;

                switchingActionFillBar.style.width = new Length(100 - percentage, LengthUnit.Percent);
                yield return null;
            }

            switchingAction.style.display = DisplayStyle.None;

            AddItem();
        }

        void AddItem()
        {
            if (item is Weapon weapon)
            {
                inventoryManager.SwitchWeapon(equipmentManager.weaponInstance?.weaponData, weapon);
                rootGameObject.SetActive(false);
            }

            SoundManager.Instance.PlaySound(addItemSound);
        }

        public void Spawn()
        {
            isWorldObject = true;
            gameObject.SetActive(true);
        }

        void Update()
        {
            if (itemModel == null)
            {
                return;
            }

            itemModel.transform.Rotate(Vector3.up, 180f * Time.deltaTime);
        }
    }
}
