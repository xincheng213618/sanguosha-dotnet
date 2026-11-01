namespace CardGame.Core;
public sealed record WeilinHandColorEvent(long ActionId, CardConversionSource Source, int TargetSeat, int TurnNumber, bool IsRed) : IGameEvent;
internal sealed class RewriteWeilinTargetHandDescriptor : GuanYuVariantDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RewriteWeilinTargetHand;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance);
}
