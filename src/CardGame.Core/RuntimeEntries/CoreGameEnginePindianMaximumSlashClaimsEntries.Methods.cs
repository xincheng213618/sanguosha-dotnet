#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianMaximumSlashClaimsEntries
{
    internal void AssertPindianPolicyClaims(global::CardGame.Core.PindianFrame frame) => _resolve().AssertPindianPolicyClaims(frame);
    internal bool BeginMaximumSlashPindianClaims(global::CardGame.Core.PindianFrame frame) => _resolve().BeginMaximumSlashPindianClaims(frame);
    internal global::CardGame.Core.PindianPolicyClaims FreezePindianPolicyClaims(global::CardGame.Core.PindianFrame frame) => _resolve().FreezePindianPolicyClaims(frame);
    internal bool HasMaximumSlashPindianClaim(global::CardGame.Core.PindianFrame frame) => _resolve().HasMaximumSlashPindianClaim(frame);
    internal void ResolveMaximumSlashPindianClaim(global::CardGame.Core.PindianFrame frame, global::CardGame.Core.PromptChoice selected) => _resolve().ResolveMaximumSlashPindianClaim(frame, selected);
}
