#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianClaimsEntries
{
    internal bool BeginPindianClaims(global::CardGame.Core.PindianFrame frame) => _resolve().BeginPindianClaims(frame);
    internal void ResolvePindianClaimChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolvePindianClaimChoice(selected);
}
