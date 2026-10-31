#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TurnDrawDebtProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDrawDebtsEntries> getCoreGameEngineTurnDrawDebtsRuntime) : ITurnDrawDebtProgramHost
{
    public SkillProgramStepOutcome DrawExtraAndArmTurnDamageUseDebt(ProgramSkillFrame f, string stateId) => getCoreGameEngineTurnDrawDebtsRuntime().DrawExtraAndArmTurnDebt(f, stateId);
    public SkillProgramStepOutcome SelectTurnDamageUseDebtPayment(ProgramSkillFrame f, SkillProgramEffect effect) => getCoreGameEngineTurnDrawDebtsRuntime().SelectTurnDrawDebtPayment(f, effect);
}
