#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDrawDebtsEntries
{
    internal void AssertTurnDrawDebtReceipts(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> instructions) => _resolve().AssertTurnDrawDebtReceipts(frame, instructions);
    internal bool CanRunTurnDrawDebtPayment(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunTurnDrawDebtPayment(candidate, trigger, context);
    internal global::CardGame.Core.ProgramExtraDrawIssuedEvent? CurrentTurnDrawDebt(int owner, string skill, string instance, string hash, string stateId) => _resolve().CurrentTurnDrawDebt(owner, skill, instance, hash, stateId);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawExtraAndArmTurnDebt(global::CardGame.Core.ProgramSkillFrame frame, string stateId) => _resolve().DrawExtraAndArmTurnDebt(frame, stateId);
    internal bool IsExtraDrawDebtMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paid, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsExtraDrawDebtMovement(frame, paid, movement);
    internal void ObserveTurnDrawDebtUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveTurnDrawDebtUse(payload);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectTurnDrawDebtPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectTurnDrawDebtPayment(frame, effect);
    internal global::CardGame.Core.SkillProgramEffect TurnDrawDebtOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().TurnDrawDebtOwnedSelectionEffect(frame, effect);
    internal bool TracksTurnDrawDebt { get => _resolve().TracksTurnDrawDebt; }
}
