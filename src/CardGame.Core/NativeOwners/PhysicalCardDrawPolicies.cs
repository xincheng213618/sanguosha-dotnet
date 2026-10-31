#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PhysicalCardDrawPolicies(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoZhaoAnniversaryEntries> getCharacterGuoZhaoAnniversaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheCampaignsEntries> getCoreGameEngineShanheCampaignsRuntime, global::System.Func<global::CardGame.Core.ModeRuleSet> getModeRuleSet, global::System.Func<global::CardGame.Core.MatchModeRuntime> getMatchModeRuntime) : IPhysicalCardDrawPolicies
{
    public bool DrawFromBottom(CharacterState player) => getCoreGameEngineProgramCardPoliciesRuntime().HasCardPolicy(player, SkillProgramCardPolicyKind.DrawFromBottom);
    public Card MinimumDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, Card ordinary) => getCharacterLuKangWumiaoRuntime().WuLuKangMinimumDrawMaterial(player, reason, stock, ordinary);
    public bool TryChooseDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, out Card? material) => getCharacterGuoZhaoAnniversaryRuntime().TryChooseAnniversaryGuoZhaoDrawMaterial(player, reason, stock, out material);
    public void ExcludeDeckEquipment(CardLocation location) => getCoreGameEngineShanheCampaignsRuntime().ExcludeShanheDeckEquipment(location);
    public bool AllowsDiscardRecycling => getModeRuleSet().AllowsDiscardRecycling;

    public void HandleEmptyDrawPile() => getMatchModeRuntime().TryHandleEmptyDrawPile();
    public void NotifyDeckRecycled() => getMatchModeRuntime().NotifyDeckRecycled();
}
