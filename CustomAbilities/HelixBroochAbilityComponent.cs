namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class HelixBroochAbilityComponent : AbilityComponentHack<HelixBroochAbility>
{
    public int currentHelixCount { get; set; }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}
