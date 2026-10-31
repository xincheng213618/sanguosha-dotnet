#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageWindowsEntries
{
    internal void AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame window) => _resolve().AdvanceDamageTriggerCandidate(window);
    internal void BeginDamageTriggerWindow(GameEngine.IDamageAttempt attack, long damageFrameId, SkillProgramTriggerWindow window, IReadOnlyList<DamageTriggerCandidate> candidates) => _resolve().BeginDamageTriggerWindow(attack, damageFrameId, window, candidates);
    internal IReadOnlyList<DamageTriggerCandidate> CollectDamageTriggerCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window) => _resolve().CollectDamageTriggerCandidates(attack, window);
    internal void CompleteDamageTriggerWindow(DamageTriggerWindowFrame window) => _resolve().CompleteDamageTriggerWindow(window);
    internal void RunOneDamageTriggerStep() => _resolve().RunOneDamageTriggerStep();
    internal bool TryBeginDamageTriggerWindow(GameEngine.IDamageAttempt attack, long damageFrameId, SkillProgramTriggerWindow window) => _resolve().TryBeginDamageTriggerWindow(attack, damageFrameId, window);
}
