#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianClaimsEntries
{
    internal bool BeginPindianClaims(PindianFrame frame) => _resolve().BeginPindianClaims(frame);
    internal void ResolvePindianClaimChoice(PromptChoice selected) => _resolve().ResolvePindianClaimChoice(selected);
}
