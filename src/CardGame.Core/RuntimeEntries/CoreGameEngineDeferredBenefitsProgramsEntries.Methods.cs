#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredBenefitsProgramsEntries
{
    internal void ApplyQueuedNextTurnRuleModifiers(int targetSeat) => _resolve().ApplyQueuedNextTurnRuleModifiers(targetSeat);
    internal SkillProgramStepOutcome BeginProgramWeaponDamageChoice(ProgramSkillFrame frame, int amount) => _resolve().BeginProgramWeaponDamageChoice(frame, amount);
    internal int CountPlayPhaseDamageTakenByAny() => _resolve().CountPlayPhaseDamageTakenByAny();
    internal void PublishFactionRecoveryPrompt(ProgramSkillFrame frame) => _resolve().PublishFactionRecoveryPrompt(frame);
    internal void PublishWeaponDamagePrompt(ProgramSkillFrame frame, int amount) => _resolve().PublishWeaponDamagePrompt(frame, amount);
    internal void QueueProgramNextTurnRuleModifier(ProgramSkillFrame frame, int targetSeat, SkillRuleQuery query, int amount) => _resolve().QueueProgramNextTurnRuleModifier(frame, targetSeat, query, amount);
    internal SkillProgramStepOutcome RequestProgramFactionRecovery(ProgramSkillFrame frame, string factionId) => _resolve().RequestProgramFactionRecovery(frame, factionId);
    internal void ResolveFactionRecoveryChoice(PromptChoice choice) => _resolve().ResolveFactionRecoveryChoice(choice);
    internal void ResolveProgramJudgmentColorBenefit(ProgramSkillFrame frame, int targetSeat, string sourceBind, int recovery) => _resolve().ResolveProgramJudgmentColorBenefit(frame, targetSeat, sourceBind, recovery);
    internal void ResolveWeaponDamageChoice(PromptChoice choice) => _resolve().ResolveWeaponDamageChoice(choice);
    internal PromptChoice SelectAiFactionRecovery(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiFactionRecovery(decision, frame);
    internal bool TryBeginFactionRecoveryDebts(GameEngine.DyingCompletionReceipt dying, bool survived) => _resolve().TryBeginFactionRecoveryDebts(dying, survived);
    internal bool TryContinueFactionRecoveryDebts(long frameId) => _resolve().TryContinueFactionRecoveryDebts(frameId);
}
