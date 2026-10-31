#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredBenefitsProgramsEntries
{
    internal void ApplyQueuedNextTurnRuleModifiers(int targetSeat) => _resolve().ApplyQueuedNextTurnRuleModifiers(targetSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramWeaponDamageChoice(global::CardGame.Core.ProgramSkillFrame frame, int amount) => _resolve().BeginProgramWeaponDamageChoice(frame, amount);
    internal int CountPlayPhaseDamageTakenByAny() => _resolve().CountPlayPhaseDamageTakenByAny();
    internal void PublishFactionRecoveryPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishFactionRecoveryPrompt(frame);
    internal void PublishWeaponDamagePrompt(global::CardGame.Core.ProgramSkillFrame frame, int amount) => _resolve().PublishWeaponDamagePrompt(frame, amount);
    internal void QueueProgramNextTurnRuleModifier(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::CardGame.Core.SkillRuleQuery query, int amount) => _resolve().QueueProgramNextTurnRuleModifier(frame, targetSeat, query, amount);
    internal global::CardGame.Core.SkillProgramStepOutcome RequestProgramFactionRecovery(global::CardGame.Core.ProgramSkillFrame frame, string factionId) => _resolve().RequestProgramFactionRecovery(frame, factionId);
    internal void ResolveFactionRecoveryChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveFactionRecoveryChoice(choice);
    internal void ResolveProgramJudgmentColorBenefit(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string sourceBind, int recovery) => _resolve().ResolveProgramJudgmentColorBenefit(frame, targetSeat, sourceBind, recovery);
    internal void ResolveWeaponDamageChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveWeaponDamageChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectAiFactionRecovery(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiFactionRecovery(decision, frame);
    internal bool TryBeginFactionRecoveryDebts(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying, bool survived) => _resolve().TryBeginFactionRecoveryDebts(dying, survived);
    internal bool TryContinueFactionRecoveryDebts(long frameId) => _resolve().TryContinueFactionRecoveryDebts(frameId);
    internal void ValidateFactionRecoveryDraft(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateFactionRecoveryDraft(frame);
    internal void ValidateWeaponDamageDraft(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateWeaponDamageDraft(frame);
}
