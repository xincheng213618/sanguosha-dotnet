#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePhysicalCardDrawCoreShanheCampaignsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExcludeShanheDeckEquipment(global::CardGame.Core.CardLocation zone);
}

internal interface ICorePhysicalCardDrawCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICorePhysicalCardDrawCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICorePhysicalCardDrawPhysicalCardDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PhysicalCardDrawRuntime Get();
}

internal interface ICorePhysicalCardDrawModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICorePhysicalCardDrawModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICorePhysicalCardDrawCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card WuLuKangMinimumDrawMaterial(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardMoveReason reason, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> stock, global::CardGame.Core.Card ordinary);
}

internal interface ICorePhysicalCardDrawCharacterGuoZhaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryChooseAnniversaryGuoZhaoDrawMaterial(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardMoveReason reason, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> drawPile, out global::CardGame.Core.Card? material);
}
