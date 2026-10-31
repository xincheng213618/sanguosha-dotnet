#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime))]
internal sealed class CoreForeignEquipmentDiscardPoliciesCharacterZhangFenAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignEquipmentDiscardPoliciesCharacterZhangFenAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasDagongCheCapability { get => this._targetFactory().HasDagongCheCapability; }

    public bool IsUnupgradedDagongCheDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsUnupgradedDagongCheDiscardForbidden(actor, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime))]
internal sealed class CoreForeignEquipmentDiscardPoliciesCharacterDongBaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignEquipmentDiscardPoliciesCharacterDongBaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> _targetFactory = targetFactory;
    public bool IsXiaHuiDiscardForbidden(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsXiaHuiDiscardForbidden(actorSeat, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreForeignEquipmentDiscardPoliciesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignEquipmentDiscardPoliciesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreForeignEquipmentDiscardPoliciesCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignEquipmentDiscardPoliciesCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public bool IsTurnHandCategoryRestricted(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCategoryRestricted(owner, card);
    public bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsSelfHandCategoryDiscardForbidden(actor, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreForeignEquipmentDiscardPoliciesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignEquipmentDiscardPoliciesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat) => this._targetFactory().GetEquipment(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime))]
internal sealed class CoreForeignEquipmentDiscardPoliciesCharacterPangTongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignEquipmentDiscardPoliciesCharacterPangTongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsShenPangTongHandDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsShenPangTongHandDiscardForbidden(actor, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime))]
internal sealed class CoreForeignEquipmentDiscardPoliciesCharacterBianFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreForeignEquipmentDiscardPoliciesCharacterBianFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> _targetFactory = targetFactory;
    public bool HasBianFuRenCapability { get => this._targetFactory().HasBianFuRenCapability; }
}
