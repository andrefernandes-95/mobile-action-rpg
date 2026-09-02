using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AF
{
    public sealed class PotionPreview : MonoBehaviour
    {
        [SerializeField] UIDocument uIDocument;
        [SerializeField] GameObject rootGameObject;
        [SerializeField] GameObject itemModel;
        [SerializeField] AudioClip addItemSound;

        EquipmentManager equipmentManager;

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
            }

            SetupRefs();
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

            itemName.text = "Health Potion";
            itemDescription.text = "";
            itemDescription.style.display = DisplayStyle.None;
            itemDifference.text = "";
            itemDifference.style.display = DisplayStyle.None;
            switchingAction.style.display = DisplayStyle.None;
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
            switchingAction.Q<Label>("SwitchingLabel").text = "Picking...";

            while (time < SWITCHING_TIME)
            {
                time += Time.deltaTime;

                float percentage = time * 100f / SWITCHING_TIME;

                switchingActionFillBar.style.width = new Length(100 - percentage, LengthUnit.Percent);
                yield return null;
            }

            switchingAction.style.display = DisplayStyle.None;

            AddPotion();
        }

        void AddPotion()
        {
            if (PlayerInventory.Instance.TryAddPotion())
            {
                rootGameObject.SetActive(false);
                SoundManager.Instance.PlaySound(addItemSound);
            }
        }
    }
}
