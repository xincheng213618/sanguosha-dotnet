#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeEquipmentAndFactionQueriesCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanEnterEquipmentSlot(int recipientSeat, global::CardGame.Core.Card card);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSpendSlashUse(global::CardGame.Core.CharacterState player, global::CardGame.Core.CharacterState target, bool ignoresCount, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreJudgedRankSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPotentialJudgedRankSlashQuota(int actor, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipmentRuleCards(global::CardGame.Core.CharacterState player, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreProgramDistanceRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDistanceRules DistanceRules { get; }
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
    int GetProgramRequiredResponseCount(global::CardGame.Core.CharacterState owner, int sourceSeat, int responderSeat, global::CardGame.Core.CardKind incomingKind, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreProgramFactionPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.FactionResponsePolicySource? GetFactionResponsePolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCharacterHuangfuSongWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuHfOrDixianPotential(int actor);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCharacterLuBuHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int HappyLuBuRequiredResponseCount(int skillOwner, int source, int responder, global::CardGame.Core.CardKind incoming, global::CardGame.Core.CardKind required);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource? source = default, bool dyingUse = false);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsArmorIneffectiveForTurn(global::CardGame.Core.CharacterState target);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPotentialWuyouFactionSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreFieldEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind? ProgramVirtualEquipment(int seat, global::CardGame.Core.EquipmentSlot slot);
}

internal interface ICoreNativeEquipmentAndFactionQueriesCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}
