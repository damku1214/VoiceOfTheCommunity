namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class HelixBroochAbilityComponent : AbilityComponentHack<HelixBroochAbility>, IStackable
{
    public int currentHelixCount { get; set; }

    public float stack
    {
        get
        {
            return currentHelixCount;
        }
        set
        {
            currentHelixCount = (int)value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}
