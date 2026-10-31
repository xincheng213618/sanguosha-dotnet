#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TriggeredCardProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTriggeredCardProgramsEntries> getCoreGameEngineTriggeredCardProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries> getCoreGameEngineProgramPhaseColorRestrictionsRuntime) : ITriggeredCardProgramHost
{
    public SkillProgramStepOutcome UseTriggeredVirtualSlash(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineTriggeredCardProgramsRuntime().UseTriggeredProgramVirtualSlash(frame, targetSeat);
    public SkillProgramStepOutcome UseDiscardedCardAsDelayedTrick(ProgramSkillFrame frame, int targetSeat, CardKind kind) => getCoreGameEngineTriggeredCardProgramsRuntime().UseProgramDiscardedCardAsDelayedTrick(frame, targetSeat, kind);
    public SkillProgramStepOutcome PayEquipmentColorDiscard(ProgramSkillFrame frame, string resultBind) => getCoreGameEngineTriggeredCardProgramsRuntime().PayProgramEquipmentColorDiscard(frame, resultBind);
    public void GrantPlayPhaseColorRestriction(ProgramSkillFrame frame, string sourceBind, int targetSeat) => getCoreGameEngineProgramPhaseColorRestrictionsRuntime().GrantProgramPlayPhaseColorRestriction(frame, sourceBind, targetSeat);
}
