using UnityEngine;

namespace AF
{
    public abstract class Effect : ScriptableObject
    {
        public abstract void OnStart(CharacterManager characterManager);
        public abstract void OnApplied(CharacterManager characterManager);
        public abstract void OnEnd(CharacterManager characterManager);
        public abstract string GetDescription();
    }
}
