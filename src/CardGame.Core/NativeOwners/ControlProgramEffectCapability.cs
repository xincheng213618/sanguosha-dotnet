#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ControlProgramEffectCapability(Func<RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries> getCoreGameEngineProgramCategoryTargetLedgerRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardUseRolesEntries> getCoreGameEngineProgramCardUseRolesRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramDamageReductionRuntime>> getCoreGameEngineProgramDamageReductionRuntime) : IControlProgramEffectHost
{
    public void ConsumeCategoryTargetLedger(ProgramSkillFrame frame, string usageId) => getCoreGameEngineProgramCategoryTargetLedgerRuntime().ConsumeProgramCategoryTargetLedger(frame, usageId);
    public void ReplaceCurrentCardUseActor(ProgramSkillFrame frame, int actorSeat) => getCoreGameEngineProgramCardUseRolesRuntime().ReplaceCurrentProgramCardUseActor(frame, actorSeat);
    public SkillProgramStepOutcome AddCurrentCardUseTarget(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineProgramCardUseRolesRuntime().AddCurrentProgramCardUseTarget(frame, targetSeat);
    public void ReduceCurrentDamage(ProgramSkillFrame frame, int amount) => getCoreGameEngineProgramDamageReductionRuntime().Value.ReduceProgramCurrentDamage(frame, amount);
}
