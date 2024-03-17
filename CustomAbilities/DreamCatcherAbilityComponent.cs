namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class DreamCatcherAbilityComponent : AbilityComponentHack<DreamCatcherAbility>, IStackable
{
    public int currentItemBreakCount { get; set; }

    public float stack
    {
        get
        {
            return currentItemBreakCount;
        }
        set
        {
            currentItemBreakCount = (int)value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}