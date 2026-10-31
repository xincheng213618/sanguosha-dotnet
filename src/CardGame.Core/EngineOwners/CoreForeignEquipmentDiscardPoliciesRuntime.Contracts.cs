#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreForeignEquipmentDiscardPoliciesCharacterZhangFenAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDagongCheCapability { get; }

    bool IsUnupgradedDagongCheDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreForeignEquipmentDiscardPoliciesCharacterDongBaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXiaHuiDiscardForbidden(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreForeignEquipmentDiscardPoliciesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreForeignEquipmentDiscardPoliciesCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCategoryRestricted(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreForeignEquipmentDiscardPoliciesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat);
}

internal interface ICoreForeignEquipmentDiscardPoliciesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreForeignEquipmentDiscardPoliciesCharacterPangTongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenPangTongHandDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreForeignEquipmentDiscardPoliciesCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBianFuRenCapability { get; }
}
