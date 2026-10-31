#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TriggeredCardProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineTriggeredCardProgramsRuntime>> getCoreGameEngineTriggeredCardProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries> getCoreGameEngineProgramPhaseColorRestrictionsRuntime) : ITriggeredCardProgramHost
{
    public SkillProgramStepOutcome UseTriggeredVirtualSlash(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineTriggeredCardProgramsRuntime().Value.UseTriggeredProgramVirtualSlash(frame, targetSeat);
    public SkillProgramStepOutcome UseDiscardedCardAsDelayedTrick(ProgramSkillFrame frame, int targetSeat, CardKind kind) => getCoreGameEngineTriggeredCardProgramsRuntime().Value.UseProgramDiscardedCardAsDelayedTrick(frame, targetSeat, kind);
    public SkillProgramStepOutcome PayEquipmentColorDiscard(ProgramSkillFrame frame, string resultBind) => getCoreGameEngineTriggeredCardProgramsRuntime().Value.PayProgramEquipmentColorDiscard(frame, resultBind);
    public void GrantPlayPhaseColorRestriction(ProgramSkillFrame frame, string sourceBind, int targetSeat) => getCoreGameEngineProgramPhaseColorRestrictionsRuntime().GrantProgramPlayPhaseColorRestriction(frame, sourceBind, targetSeat);
}
