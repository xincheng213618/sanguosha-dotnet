#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeResponseMaterialQueriesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeResponseMaterialQueriesCharacterCaoPiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MouCaoPiResponseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type);
}

internal interface ICoreNativeResponseMaterialQueriesCharacterXiahouHuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool AnniversaryXiahouHuiResponseForbidden(int actorSeat, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICoreNativeResponseMaterialQueriesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}

internal interface ICoreNativeResponseMaterialQueriesCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeResponseMaterialQueriesCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasGuanHaiCapability { get; }

    bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeResponseMaterialQueriesCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> ShouliFieldResponseCards(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind requiredKind);
}

internal interface ICoreNativeResponseMaterialQueriesCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
}

internal interface ICoreNativeResponseMaterialQueriesCharacterZhugeLiangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuZglCopyBlocked(int actor, int id);
}

internal interface ICoreNativeResponseMaterialQueriesCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeiluanCapability { get; }
}

internal interface ICoreNativeResponseMaterialQueriesCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreNativeResponseMaterialQueriesCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasResponseEntityExchangeObservers();
}

internal interface ICoreNativeResponseMaterialQueriesCoreRecordedSuitSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRecordedSuitResponseAllowed(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeResponseMaterialQueriesCoreActionDiscardColorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksActionDiscardColor { get; }
}

internal interface ICoreNativeResponseMaterialQueriesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreNativeResponseMaterialQueriesCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseZhangbaSerpentSpear(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeResponseMaterialQueriesCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanNativeResponseForbidden(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind required, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeResponseMaterialQueriesCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId);
    bool IsProgramMaterialUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> cardIds, bool response = false);
    bool IsProgramNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource? source = default, bool dyingUse = false);
}

internal interface ICoreNativeResponseMaterialQueriesCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreNativeResponseMaterialQueriesCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileResponseCards(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeResponseMaterialQueriesCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedPlayPhaseUseBan(int seat);
}

internal interface ICoreNativeResponseMaterialQueriesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreNativeResponseMaterialQueriesCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBiguoCategoryForbidden(int seat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreNativeResponseMaterialQueriesCoreLastZoneConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetLastZoneJudgmentConversionCards(global::CardGame.Core.CharacterState owner, bool forResponse);
}
