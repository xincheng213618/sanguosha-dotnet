#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OriginalTargetAdditionProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries> getCoreGameEngineOriginalTargetAdditionsRuntime) : IOriginalTargetAdditionProgramHost
{
    public void GrantOriginalTargetAddition(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineOriginalTargetAdditionsRuntime().GrantOriginalTargetAddition(frame, targetSeat);
    public SkillProgramStepOutcome OfferOriginalTargetAddition(ProgramSkillFrame frame) => getCoreGameEngineOriginalTargetAdditionsRuntime().OfferOriginalTargetAddition(frame);
}
