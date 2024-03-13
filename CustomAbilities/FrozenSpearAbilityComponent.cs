using CustomItems.CustomAbilities;

namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class FrozenSpearAbilityComponent : AbilityComponentHack<FrozenSpearAbility>, IStackable
{
    public int currentFreezeCount { get; set; }

    public float stack
    {
        get
        {
            return currentFreezeCount;
        }
        set
        {
            currentFreezeCount = (int)value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}
