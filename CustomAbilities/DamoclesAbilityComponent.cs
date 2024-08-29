namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class DamoclesAbilityComponent : AbilityComponentHack<DamoclesAbility>, IStackable
{
    public int currentCount { get; set; }

    public float stack
    {
        get
        {
            return currentCount;
        }
        set
        {
            currentCount = (int)value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}
