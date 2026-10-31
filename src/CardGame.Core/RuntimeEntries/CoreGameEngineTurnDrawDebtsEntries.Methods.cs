#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDrawDebtsEntries
{
    internal void AssertTurnDrawDebtReceipts(ProgramSkillFrame frame, IReadOnlyList<SkillProgramEffect> instructions) => _resolve().AssertTurnDrawDebtReceipts(frame, instructions);
    internal bool CanRunTurnDrawDebtPayment(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunTurnDrawDebtPayment(candidate, trigger, context);
    internal ProgramExtraDrawIssuedEvent? CurrentTurnDrawDebt(int owner, string skill, string instance, string hash, string stateId) => _resolve().CurrentTurnDrawDebt(owner, skill, instance, hash, stateId);
    internal SkillProgramStepOutcome DrawExtraAndArmTurnDebt(ProgramSkillFrame frame, string stateId) => _resolve().DrawExtraAndArmTurnDebt(frame, stateId);
    internal void ObserveTurnDrawDebtUse(IGameEvent payload) => _resolve().ObserveTurnDrawDebtUse(payload);
    internal SkillProgramStepOutcome SelectTurnDrawDebtPayment(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().SelectTurnDrawDebtPayment(frame, effect);
    internal SkillProgramEffect TurnDrawDebtOwnedSelectionEffect(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().TurnDrawDebtOwnedSelectionEffect(frame, effect);
    internal bool TracksTurnDrawDebt { get => _resolve().TracksTurnDrawDebt; }
}
