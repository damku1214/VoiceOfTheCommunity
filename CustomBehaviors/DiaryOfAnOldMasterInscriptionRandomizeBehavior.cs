using System;
using Characters.Gear.Items;

namespace VoiceOfTheCommunity.CustomBehaviors;

[Serializable]
public sealed class DiaryOfAnOldMasterRandomInscriptionBehavior : KeywordRandomizer
{
    private new void Awake()
    {
        base.Awake();
        InscriptionsRandomizer();
    }

    private void InscriptionsRandomizer()
    {
        switch (Southpaw.Random.NextInt(0, 3))
        {
            case 0:
                _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.HiddenBlade;
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.HiddenBlade;
                break;
            case 1:
                _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Heritage;
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Heritage;
                break;
            case 2:
                _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Strike;
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Strike;
                break;
            case 3:
                _item.keyword1 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Relic;
                _item.keyword2 = Characters.Gear.Synergy.Inscriptions.Inscription.Key.Relic;
                break;
        }
    }
}
