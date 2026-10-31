#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignTurnContestAidProgramCapability(Func<ForeignTurnContestRuntime> getForeignTurnContestRuntime, Func<RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries> getCoreGameEngineSameTypeActualUseAidRuntime) : IForeignTurnContestAidProgramHost
{
    public SkillProgramStepOutcome ResolveForeignTurnPindian(ProgramSkillFrame frame, string resultBind) => getForeignTurnContestRuntime().Resolve(frame, resultBind);
    public SkillProgramStepOutcome OfferSameTypeDifferentNameOrExtraTarget(ProgramSkillFrame frame) => getCoreGameEngineSameTypeActualUseAidRuntime().BeginSameTypeAid(frame);
}
