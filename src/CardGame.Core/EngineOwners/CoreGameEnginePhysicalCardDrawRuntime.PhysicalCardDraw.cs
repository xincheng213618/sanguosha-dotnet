#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhysicalCardDrawRuntime
{
    internal PhysicalCardDrawRuntime PhysicalCardDraws => _physicalCardDrawOperations.Get();

    bool IPhysicalCardDrawPolicies.DrawFromBottom(CharacterState player) => _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.DrawFromBottom);
    Card IPhysicalCardDrawPolicies.MinimumDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, Card ordinary) => _characterLuKangWumiaoOperations.WuLuKangMinimumDrawMaterial(player, reason, stock, ordinary);
    bool IPhysicalCardDrawPolicies.TryChooseDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, out Card? material) => _characterGuoZhaoAnniversaryOperations.Value.TryChooseAnniversaryGuoZhaoDrawMaterial(player, reason, stock, out material);
    void IPhysicalCardDrawPolicies.ExcludeDeckEquipment(CardLocation location) => _coreShanheCampaignsOperations.ExcludeShanheDeckEquipment(location);
    bool IPhysicalCardDrawPolicies.AllowsDiscardRecycling => _modeRulesOperations._modeRules.AllowsDiscardRecycling;

    void IPhysicalCardDrawPolicies.HandleEmptyDrawPile() => _modeRuntimeOperations._modeRuntime.TryHandleEmptyDrawPile();
    void IPhysicalCardDrawPolicies.NotifyDeckRecycled() => _modeRuntimeOperations._modeRuntime.NotifyDeckRecycled();
    void IPhysicalCardDrawLogs.Add(string type, string message, int? actor) => _coreMatchLogOperations.AddLog(type, message, actor);
}
