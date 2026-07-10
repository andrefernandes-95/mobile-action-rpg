namespace AF
{
    using UnityEngine;

    public class Ability : ScriptableObject
    {
        [Header("Info")]
        public new string name;

        [TextArea] public string description;
        public Sprite icon;

        [Header("Cooldown")]
        public float cooldown;


        public virtual void OnStart(CharacterManager characterManager)
        {
        }

        public virtual void OnEnd(CharacterManager characterManager)
        {
        }

    }

}