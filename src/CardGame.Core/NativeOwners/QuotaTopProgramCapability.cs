#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QuotaTopProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineQuotaTopProgramsEntries> getCoreGameEngineQuotaTopProgramsRuntime) : IQuotaTopProgramHost
{
    public SkillProgramStepOutcome PeekTurnQuotaTop(ProgramSkillFrame frame, int count) => getCoreGameEngineQuotaTopProgramsRuntime().BeginQuotaTop(frame, count);
    public void NullifyFirstTurnTargetByHand(ProgramSkillFrame frame) => getCoreGameEngineQuotaTopProgramsRuntime().NullifyProgramFirstTurnTarget(frame);
}
