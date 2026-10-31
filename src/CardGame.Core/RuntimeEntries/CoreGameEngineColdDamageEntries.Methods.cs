#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineColdDamageEntries
{
    internal GameEngine.IDamageAttempt ColdDamageAttack(ColdDamageReplacementFrame frame) => _resolve().ColdDamageAttack(frame);
    internal IReadOnlyList<PromptChoice> ColdDamageChoices(ColdDamageReplacementFrame frame) => _resolve().ColdDamageChoices(frame);
    internal void ContinueColdDamageReplacement(long frameId) => _resolve().ContinueColdDamageReplacement(frameId);
    internal bool HasColdDamagePrompt() => _resolve().HasColdDamagePrompt();
    internal void PublishColdDamageChoice(ColdDamageReplacementFrame frame) => _resolve().PublishColdDamageChoice(frame);
    internal void ResolveColdDamageChoice(PromptChoice choice) => _resolve().ResolveColdDamageChoice(choice);
    internal bool TryBeginColdDamageReplacement(GameEngine.IDamageAttempt attack, int amount, DamageNature nature) => _resolve().TryBeginColdDamageReplacement(attack, amount, nature);
    internal bool TryChooseColdDamageAi(PendingDecision decision, scoped out PromptChoice selected) => _resolve().TryChooseColdDamageAi(decision, out selected);
}
