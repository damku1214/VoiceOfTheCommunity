using CustomItems.CustomAbilities;

namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class RustyChaliceAbilityComponent : AbilityComponentHack<RustyChaliceAbility>, IStackable
{
    public int currentSwapSkillHitCount { get; set; }

    public float stack
    {
        get
        {
            return currentSwapSkillHitCount;
        }
        set
        {
            currentSwapSkillHitCount = (int) value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}
