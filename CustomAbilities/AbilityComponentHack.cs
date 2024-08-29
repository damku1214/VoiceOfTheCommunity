using System;
using Characters.Abilities;
using UnityEngine;

namespace VoiceOfTheCommunity.CustomAbilities;

public abstract class AbilityComponentHack<T> : AbilityComponent<T>, ISerializationCallbackReceiver
    where T : Characters.Abilities.Ability, ICloneable
{
    [SerializeReference]
    private T _abilityReference;

    public void OnAfterDeserialize()
    {
        if (_ability == null && _abilityReference != null)
        {
            _ability = (T)_abilityReference.Clone();
        }
    }

    public void OnBeforeSerialize()
    {
        _abilityReference = _ability;
    }
}
