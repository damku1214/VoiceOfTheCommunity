namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class CursedShieldAbilityComponent : AbilityComponentHack<CursedShieldAbility>, IStackable
{
    public float savedShieldAmount { get; set; }

    public float stack
    {
        get
        {
            return savedShieldAmount;
        }
        set
        {
            savedShieldAmount = value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}
