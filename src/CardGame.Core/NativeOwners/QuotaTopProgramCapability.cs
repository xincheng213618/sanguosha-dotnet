#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QuotaTopProgramCapability(Func<RuntimeEntries.CoreGameEngineQuotaTopProgramsEntries> getCoreGameEngineQuotaTopProgramsRuntime) : IQuotaTopProgramHost
{
    public SkillProgramStepOutcome PeekTurnQuotaTop(ProgramSkillFrame frame, int count) => getCoreGameEngineQuotaTopProgramsRuntime().BeginQuotaTop(frame, count);
    public void NullifyFirstTurnTargetByHand(ProgramSkillFrame frame) => getCoreGameEngineQuotaTopProgramsRuntime().NullifyProgramFirstTurnTarget(frame);
}
