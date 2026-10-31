#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterDaQiaoMobileCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICharacterDaQiaoMobileCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICharacterDaQiaoMobileContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterDaQiaoMobileCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default);
}

internal interface ICharacterDaQiaoMobileCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    global::CardGame.Core.RuleQueryEvaluation EvaluateAttackRange(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICharacterDaQiaoMobileCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICharacterDaQiaoMobileCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSayingRoundRange(int actor, int target);
}

internal interface ICharacterDaQiaoMobileCharacterTaishiCiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenTaishiCiPoweiRange(int actor, int target);
}

internal interface ICharacterDaQiaoMobileCoreProgramAttackEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramSlashRedirectBaseTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int redirectorSeat, int targetSeat);
}

internal interface ICharacterDaQiaoMobileCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterDaQiaoMobileCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ProgramCardAttack { get; }
}

internal interface ICharacterDaQiaoMobileCoreProgramPersistentCombatPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGameFactionAttackRangeTarget(int actorSeat, int targetSeat);
}

internal interface ICharacterDaQiaoMobileCoreProgramOwnedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect GetOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterDaQiaoMobileResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}
