#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSimultaneousPindianClaimsEntries
{
    internal int[] AvailableSimultaneousClaimCards(PindianPolicyClaim claim) => _resolve().AvailableSimultaneousClaimCards(claim);
    internal PindianPolicyClaims FreezeSimultaneousClaims(ProgramSkillFrame root, PindianResult result) => _resolve().FreezeSimultaneousClaims(root, result);
    internal bool OfferSimultaneousClaim(ProgramSkillFrame f) => _resolve().OfferSimultaneousClaim(f);
    internal void ResolveSimultaneousClaim(ProgramSkillFrame f, PromptChoice choice) => _resolve().ResolveSimultaneousClaim(f, choice);
}
