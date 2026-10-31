#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignTurnContestAidProgramCapability(global::System.Func<global::CardGame.Core.ForeignTurnContestRuntime> getForeignTurnContestRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries> getCoreGameEngineSameTypeActualUseAidRuntime) : IForeignTurnContestAidProgramHost
{
    public SkillProgramStepOutcome ResolveForeignTurnPindian(ProgramSkillFrame frame, string resultBind) => getForeignTurnContestRuntime().Resolve(frame, resultBind);
    public SkillProgramStepOutcome OfferSameTypeDifferentNameOrExtraTarget(ProgramSkillFrame frame) => getCoreGameEngineSameTypeActualUseAidRuntime().BeginSameTypeAid(frame);
}
