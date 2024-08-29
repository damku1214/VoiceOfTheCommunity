using System;
using Characters.Gear.Items;

namespace VoiceOfTheCommunity.CustomBehaviors;

[Serializable]
public sealed class TheSecretOfTheKingRandomInscriptionBehavior : KeywordRandomizer
{
    private new void Awake()
    {
        base.Awake();
        _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Omen;
    }

    private void InscriptionsRandomizer()
    {
        switch (Southpaw.Random.NextInt(0, 4))
        {
            case 0:
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Poisoning; break;
            case 1:
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Arson; break;
            case 2:
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.AbsoluteZero; break;
            case 3:
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Dizziness; break;
            case 4:
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.ExcessiveBleeding; break;
        }
    }
}
