#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineColdDamageEntries
{
    internal void AssertColdDamageReplacementFrames() => _resolve().AssertColdDamageReplacementFrames();
    internal global::CardGame.Core.GameEngine.IDamageAttempt ColdDamageAttack(global::CardGame.Core.ColdDamageReplacementFrame frame) => _resolve().ColdDamageAttack(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ColdDamageChoices(global::CardGame.Core.ColdDamageReplacementFrame frame) => _resolve().ColdDamageChoices(frame);
    internal void ContinueColdDamageReplacement(long frameId) => _resolve().ContinueColdDamageReplacement(frameId);
    internal bool HasColdDamagePrompt() => _resolve().HasColdDamagePrompt();
    internal bool HasColdDamageReplacement(long attackId) => _resolve().HasColdDamageReplacement(attackId);
    internal void PublishColdDamageChoice(global::CardGame.Core.ColdDamageReplacementFrame frame) => _resolve().PublishColdDamageChoice(frame);
    internal void ResolveColdDamageChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveColdDamageChoice(choice);
    internal bool TryBeginColdDamageReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature) => _resolve().TryBeginColdDamageReplacement(attack, amount, nature);
    internal bool TryChooseColdDamageAi(global::CardGame.Core.PendingDecision decision, scoped out global::CardGame.Core.PromptChoice selected) => _resolve().TryChooseColdDamageAi(decision, out selected);
}
