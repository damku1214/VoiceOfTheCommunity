namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class VaseOfTheFallenAbilityComponent : AbilityComponentHack<VaseOfTheFallenAbility>, IStackable
{
    public int currentKillCount { get; set; }

    public float stack
    {
        get
        {
            return currentKillCount;
        }
        set
        {
            currentKillCount = (int) value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}
