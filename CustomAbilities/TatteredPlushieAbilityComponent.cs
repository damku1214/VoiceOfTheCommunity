namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class TatteredPlushieAbilityComponent : AbilityComponentHack<TatteredPlushieAbility>, IStackable
{
    public int currentActivateCount { get; set; }

    public float stack
    {
        get
        {
            return currentActivateCount;
        }
        set
        {
            currentActivateCount = (int)value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}