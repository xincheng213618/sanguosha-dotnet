#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianMaximumSlashClaimsEntries
{
    internal void AssertPindianPolicyClaims(PindianFrame frame) => _resolve().AssertPindianPolicyClaims(frame);
    internal bool BeginMaximumSlashPindianClaims(PindianFrame frame) => _resolve().BeginMaximumSlashPindianClaims(frame);
    internal PindianPolicyClaims FreezePindianPolicyClaims(PindianFrame frame) => _resolve().FreezePindianPolicyClaims(frame);
    internal bool HasMaximumSlashPindianClaim(PindianFrame frame) => _resolve().HasMaximumSlashPindianClaim(frame);
    internal void ResolveMaximumSlashPindianClaim(PindianFrame frame, PromptChoice selected) => _resolve().ResolveMaximumSlashPindianClaim(frame, selected);
}
