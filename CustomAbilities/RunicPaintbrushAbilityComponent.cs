namespace VoiceOfTheCommunity.CustomAbilities;

public sealed class RunicPaintbrushAbilityComponent : AbilityComponentHack<RunicPaintbrushAbility>
{
    public int currentAtkBuffCount { get; set; }
    public int currentSpdBuffCount { get; set; }
    public int currentCdBuffCount { get; set; }

    public override void Initialize()
    {
        base.Initialize();
        baseAbility.component = this;
    }
}
