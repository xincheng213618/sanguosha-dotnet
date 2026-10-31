#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianRandomSelectionsEntries
{
    internal bool AssertPindianRandomSelection(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PendingDecision decision) => _resolve().AssertPindianRandomSelection(frame, decision);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PindianRandomCandidate> CollectPindianRandomPolicies(int source, int opponent) => _resolve().CollectPindianRandomPolicies(source, opponent);
    internal global::CardGame.Core.PindianRandomSelection CreatePindianRandomSelection(global::CardGame.Core.PindianFrame frame, bool deferredTop = false, int? deferredCard = default) => _resolve().CreatePindianRandomSelection(frame, deferredTop, deferredCard);
    internal bool HasPindianRandomPolicy(int seat) => _resolve().HasPindianRandomPolicy(seat);
    internal bool IsPindianRandomPrompt(global::CardGame.Core.PindianFrame frame) => _resolve().IsPindianRandomPrompt(frame);
    internal bool PindianPolicySourceLive(int seat, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.SkillProgramCardPolicyKind kind) => _resolve().PindianPolicySourceLive(seat, source, kind);
    internal bool TryContinuePindianRandomSelection(global::CardGame.Core.PindianFrame frame) => _resolve().TryContinuePindianRandomSelection(frame);
    internal bool TryResolvePindianRandomChoice(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PromptChoice selected) => _resolve().TryResolvePindianRandomChoice(frame, selected);
}
