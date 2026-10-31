#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheCampaignsRuntime))]
internal sealed class CorePhysicalCardDrawCoreShanheCampaignsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheCampaignsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawCoreShanheCampaignsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheCampaignsRuntime> _targetFactory = targetFactory;
    public void ExcludeShanheDeckEquipment(global::CardGame.Core.CardLocation zone) => this._targetFactory().ExcludeShanheDeckEquipment(zone);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CorePhysicalCardDrawCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CorePhysicalCardDrawCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.PhysicalCardDrawRuntime))]
internal sealed class CorePhysicalCardDrawPhysicalCardDrawAdapter(global::System.Func<global::CardGame.Core.PhysicalCardDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawPhysicalCardDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.PhysicalCardDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PhysicalCardDrawRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CorePhysicalCardDrawCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card WuLuKangMinimumDrawMaterial(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardMoveReason reason, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> stock, global::CardGame.Core.Card ordinary) => this._targetFactory().WuLuKangMinimumDrawMaterial(player, reason, stock, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime))]
internal sealed class CorePhysicalCardDrawCharacterGuoZhaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePhysicalCardDrawCharacterGuoZhaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool TryChooseAnniversaryGuoZhaoDrawMaterial(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardMoveReason reason, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> drawPile, out global::CardGame.Core.Card? material) => this._targetFactory().TryChooseAnniversaryGuoZhaoDrawMaterial(player, reason, drawPile, out material);
}
