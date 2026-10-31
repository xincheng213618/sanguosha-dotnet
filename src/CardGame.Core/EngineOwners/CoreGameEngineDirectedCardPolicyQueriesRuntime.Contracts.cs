#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDirectedCardPolicyQueriesCharacterGaoShunBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBoundaryGaoShunArmorBypass(int actor, int target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreDirectedCardPolicyQueriesCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? SpecificSlashRank(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, long? useId = default);
}

internal interface ICoreDirectedCardPolicyQueriesCharacterHuaXiongMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouHuaXiongWei(int owner);
}

internal interface ICoreDirectedCardPolicyQueriesCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSpendSlashUse(global::CardGame.Core.CharacterState player, global::CardGame.Core.CharacterState target, bool ignoresCount, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICoreDirectedCardPolicyQueriesCoreJudgedRankSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJudgedRankSlashQuota(int actor, global::CardGame.Core.CardKind kind, int? rank);
}

internal interface ICoreDirectedCardPolicyQueriesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreDirectedCardPolicyQueriesCoreOriginalTargetAdditionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedOriginalTargetArmorBypass(long id, int targetSeat);
}

internal interface ICoreDirectedCardPolicyQueriesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreDirectedCardPolicyQueriesCharacterFanJiangZhangDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasYuanchouArmorBypass(long useId, int targetSeat);
}

internal interface ICoreDirectedCardPolicyQueriesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDirectedCardPolicyQueriesCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasArmorBypass(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDirectedCardPolicyQueriesCoreProgramAllHandBonusOperationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BypassesSlashLimitBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreDirectedCardPolicyQueriesCoreProgramDistanceRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDistanceRules DistanceRules { get; }
}

internal interface ICoreDirectedCardPolicyQueriesCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDirectedTurnCardPolicy(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.DirectedTurnCardPolicyEffect effect);
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreDirectedCardPolicyQueriesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDirectedCardPolicyQueriesCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}

internal interface ICoreDirectedCardPolicyQueriesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDirectedCardPolicyQueriesCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsArmorIneffectiveForTurn(global::CardGame.Core.CharacterState target);
}
