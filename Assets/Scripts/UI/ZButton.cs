using UnityEngine;
using UnityEngine.UI;

namespace AF
{
    [RequireComponent(typeof(Button))]
    public class ZButton : MonoBehaviour
    {
        PlayerInput playerInput;

        [SerializeField] Image image;
        Button btn => GetComponent<Button>();

        void OnEnable()
        {
            playerInput = FindAnyObjectByType<PlayerInput>(FindObjectsInactive.Include);

            UpdateIcon(playerInput.startingCharacterManager);

            btn.onClick.AddListener(playerInput.OnAttack);
            playerInput.onSetCurrentCharacter.AddListener(UpdateIcon);
        }

        void OnDisable()
        {
            btn.onClick.RemoveListener(playerInput.OnAttack);
            playerInput.onSetCurrentCharacter.RemoveListener(UpdateIcon);
        }

        void UpdateIcon(CharacterManager newCharacter)
        {
            image.sprite = newCharacter.combatManager.AttackAbility?.icon;
        }
    }
}
