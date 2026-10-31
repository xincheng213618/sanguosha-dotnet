#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianRandomSelectionsEntries
{
    internal bool AssertPindianRandomSelection(PindianFrame frame, PendingDecision decision) => _resolve().AssertPindianRandomSelection(frame, decision);
    internal IReadOnlyList<PindianRandomCandidate> CollectPindianRandomPolicies(int source, int opponent) => _resolve().CollectPindianRandomPolicies(source, opponent);
    internal PindianRandomSelection CreatePindianRandomSelection(PindianFrame frame, bool deferredTop = false, int? deferredCard = default) => _resolve().CreatePindianRandomSelection(frame, deferredTop, deferredCard);
    internal bool HasPindianRandomPolicy(int seat) => _resolve().HasPindianRandomPolicy(seat);
    internal bool IsPindianRandomPrompt(PindianFrame frame) => _resolve().IsPindianRandomPrompt(frame);
    internal bool PindianPolicySourceLive(int seat, CardConversionSource source, SkillProgramCardPolicyKind kind) => _resolve().PindianPolicySourceLive(seat, source, kind);
    internal bool TryContinuePindianRandomSelection(PindianFrame frame) => _resolve().TryContinuePindianRandomSelection(frame);
    internal bool TryResolvePindianRandomChoice(PindianFrame frame, PromptChoice selected) => _resolve().TryResolvePindianRandomChoice(frame, selected);
}
