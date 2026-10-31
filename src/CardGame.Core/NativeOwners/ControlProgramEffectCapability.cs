#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ControlProgramEffectCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries> getCoreGameEngineProgramCategoryTargetLedgerRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseRolesEntries> getCoreGameEngineProgramCardUseRolesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDamageReductionEntries> getCoreGameEngineProgramDamageReductionRuntime) : IControlProgramEffectHost
{
    public void ConsumeCategoryTargetLedger(ProgramSkillFrame frame, string usageId) => getCoreGameEngineProgramCategoryTargetLedgerRuntime().ConsumeProgramCategoryTargetLedger(frame, usageId);
    public void ReplaceCurrentCardUseActor(ProgramSkillFrame frame, int actorSeat) => getCoreGameEngineProgramCardUseRolesRuntime().ReplaceCurrentProgramCardUseActor(frame, actorSeat);
    public SkillProgramStepOutcome AddCurrentCardUseTarget(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineProgramCardUseRolesRuntime().AddCurrentProgramCardUseTarget(frame, targetSeat);
    public void ReduceCurrentDamage(ProgramSkillFrame frame, int amount) => getCoreGameEngineProgramDamageReductionRuntime().ReduceProgramCurrentDamage(frame, amount);
}
