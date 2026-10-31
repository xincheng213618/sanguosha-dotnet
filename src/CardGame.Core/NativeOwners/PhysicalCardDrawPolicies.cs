#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PhysicalCardDrawPolicies(Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, Func<RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime, Func<RuntimeEntries.CharacterGuoZhaoAnniversaryEntries> getCharacterGuoZhaoAnniversaryRuntime, Func<RuntimeEntries.CoreGameEngineShanheCampaignsEntries> getCoreGameEngineShanheCampaignsRuntime, Func<ModeRuleSet> getModeRuleSet, Func<MatchModeRuntime> getMatchModeRuntime) : IPhysicalCardDrawPolicies
{
    public bool DrawFromBottom(CharacterState player) => getCoreGameEngineProgramCardPoliciesRuntime().HasCardPolicy(player, SkillProgramCardPolicyKind.DrawFromBottom);
    public Card MinimumDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, Card ordinary) => getCharacterLuKangWumiaoRuntime().WuLuKangMinimumDrawMaterial(player, reason, stock, ordinary);
    public bool TryChooseDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, out Card? material) => getCharacterGuoZhaoAnniversaryRuntime().TryChooseAnniversaryGuoZhaoDrawMaterial(player, reason, stock, out material);
    public void ExcludeDeckEquipment(CardLocation location) => getCoreGameEngineShanheCampaignsRuntime().ExcludeShanheDeckEquipment(location);
    public bool AllowsDiscardRecycling => getModeRuleSet().AllowsDiscardRecycling;

    public void HandleEmptyDrawPile() => getMatchModeRuntime().TryHandleEmptyDrawPile();
    public void NotifyDeckRecycled() => getMatchModeRuntime().NotifyDeckRecycled();
}
