#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageWindowsEntries
{
    internal void AdvanceDamageTriggerCandidate(global::CardGame.Core.DamageTriggerWindowFrame window) => _resolve().AdvanceDamageTriggerCandidate(window);
    internal void BeginDamageTriggerWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DamageTriggerCandidate> candidates) => _resolve().BeginDamageTriggerWindow(attack, damageFrameId, window, candidates);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DamageTriggerCandidate> CollectDamageTriggerCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().CollectDamageTriggerCandidates(attack, window);
    internal void CompleteDamageTriggerWindow(global::CardGame.Core.DamageTriggerWindowFrame window) => _resolve().CompleteDamageTriggerWindow(window);
    internal void RunOneDamageTriggerStep() => _resolve().RunOneDamageTriggerStep();
    internal bool TryBeginDamageTriggerWindow(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().TryBeginDamageTriggerWindow(attack, damageFrameId, window);
}
