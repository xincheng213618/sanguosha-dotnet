#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSimultaneousPindianClaimsEntries
{
    internal int[] AvailableSimultaneousClaimCards(global::CardGame.Core.PindianPolicyClaim claim) => _resolve().AvailableSimultaneousClaimCards(claim);
    internal global::CardGame.Core.PindianPolicyClaims FreezeSimultaneousClaims(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.PindianResult result) => _resolve().FreezeSimultaneousClaims(root, result);
    internal bool OfferSimultaneousClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OfferSimultaneousClaim(f);
    internal void ResolveSimultaneousClaim(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSimultaneousClaim(f, choice);
}
