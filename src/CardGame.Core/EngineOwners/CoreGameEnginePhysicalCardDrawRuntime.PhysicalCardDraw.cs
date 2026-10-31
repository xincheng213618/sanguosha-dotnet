#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhysicalCardDrawRuntime
{
    internal PhysicalCardDrawRuntime PhysicalCardDraws => _physicalCardDrawOperations.Get();

    bool global::CardGame.Core.IPhysicalCardDrawPolicies.DrawFromBottom(CharacterState player) => _coreProgramCardPoliciesOperations.HasCardPolicy(player, SkillProgramCardPolicyKind.DrawFromBottom);
    Card global::CardGame.Core.IPhysicalCardDrawPolicies.MinimumDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, Card ordinary) => _characterLuKangWumiaoOperations.WuLuKangMinimumDrawMaterial(player, reason, stock, ordinary);
    bool global::CardGame.Core.IPhysicalCardDrawPolicies.TryChooseDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, out Card? material) => _characterGuoZhaoAnniversaryOperations.TryChooseAnniversaryGuoZhaoDrawMaterial(player, reason, stock, out material);
    void global::CardGame.Core.IPhysicalCardDrawPolicies.ExcludeDeckEquipment(CardLocation location) => _coreShanheCampaignsOperations.ExcludeShanheDeckEquipment(location);
    bool global::CardGame.Core.IPhysicalCardDrawPolicies.AllowsDiscardRecycling => _modeRulesOperations._modeRules.AllowsDiscardRecycling;

    void global::CardGame.Core.IPhysicalCardDrawPolicies.HandleEmptyDrawPile() => _modeRuntimeOperations._modeRuntime.TryHandleEmptyDrawPile();
    void global::CardGame.Core.IPhysicalCardDrawPolicies.NotifyDeckRecycled() => _modeRuntimeOperations._modeRuntime.NotifyDeckRecycled();
    void global::CardGame.Core.IPhysicalCardDrawLogs.Add(string type, string message, int? actor) => _coreMatchLogOperations.AddLog(type, message, actor);
}
