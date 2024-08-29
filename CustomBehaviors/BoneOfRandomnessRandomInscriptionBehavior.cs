using System;
using Characters.Gear.Items;

namespace VoiceOfTheCommunity.CustomBehaviors;

[Serializable]
public sealed class BoneOfRandomnessRandomInscriptionBehavior : KeywordRandomizer
{
    private new void Awake()
    {
        base.Awake();
        _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Bone;
    }
}
