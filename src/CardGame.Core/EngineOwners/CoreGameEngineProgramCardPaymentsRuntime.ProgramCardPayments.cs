#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramCardPaymentsRuntime
{
    internal int ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _programPaidCardOperations.Get().ResolveProgramParticipant(frame, reference);
    internal SkillProgramStepOutcome SelectAndMoveProgramOwnedCard(ProgramSkillFrame frame, ProgramParticipantReference chooser, ProgramParticipantReference cardOwner, IReadOnlyList<CardZoneKind> zones, SkillProgramCardDestination destination, ProgramParticipantReference? destinationRef, string? resultBind, CardMoveReason reason, IReadOnlyList<SkillProgramCardCategory>? cardCategories = null, bool skipIfNoCards = false, bool allowSameOwnerHandReturn = false, string? coverageResultBind = null, bool awaitMovementTriggers = false, bool revealBeforeMove = false, IReadOnlyList<CardKind>? cardKinds = null, bool prohibitReplacingEquipment = false, IReadOnlyList<Suit>? suits = null) => _programPaidCardOperations.Get().SelectAndMoveProgramOwnedCard(frame, chooser, cardOwner, zones, destination, destinationRef, resultBind, reason, cardCategories, skipIfNoCards, allowSameOwnerHandReturn, coverageResultBind, awaitMovementTriggers, revealBeforeMove, cardKinds, prohibitReplacingEquipment, suits);
    internal bool HasOwnedProgramCardCategory(int ownerSeat, IReadOnlyList<CardZoneKind> zones, IReadOnlyList<SkillProgramCardCategory> categories, IReadOnlyList<CardKind> kinds) => _programPaidCardOperations.Get().HasOwnedProgramCardCategory(ownerSeat, zones, categories, kinds);
    internal IReadOnlyList<PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, IReadOnlyList<CardZoneKind> zones, OwnedCardMoveIntent intent, IReadOnlyList<SkillProgramCardCategory>? cardCategories = null, IReadOnlyList<CardKind>? cardKinds = null, IReadOnlyList<Suit>? suits = null, Func<CardZoneKind, Card, bool>? canSelect = null, SkillProgramCardDestination destination = SkillProgramCardDestination.DiscardPile, int? destinationSeat = null, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = null) => _programPaidCardOperations.Get().BuildOwnedCardPaymentChoices(frameId, chooserSeat, cardOwnerSeat, zones, intent, cardCategories, cardKinds, suits, canSelect, destination, destinationSeat, prohibitReplacingEquipment, correspondingZoneSeat);
    internal void ResolveSelectAndMoveOwnedCardChoice(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected) => _programPaidCardOperations.Get().ResolveSelectAndMoveOwnedCardChoice(frame, effect, selected);
    internal bool CanMoveProgramCardToCorrespondingZone(Card card, CardZoneKind sourceZone, int destinationSeat) => _programPaidCardOperations.Get().CanMoveProgramCardToCorrespondingZone(card, sourceZone, destinationSeat);
    internal CardLocation ProgramCorrespondingZoneLocation(Card card, CardZoneKind sourceZone, int destinationSeat) => _programPaidCardOperations.Get().ProgramCorrespondingZoneLocation(card, sourceZone, destinationSeat);
    internal int CountLivingInAttackRange(int subjectSeat)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[subjectSeat].IsAlive)
            return 0;
        var range = _coreNativeRuleQueriesOperations.GetAttackRange(subjectSeat);
        return _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive && player.Seat != subjectSeat && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(subjectSeat, player.Seat));
    }

    internal bool WouldEquipmentRemovalReduceCoverage(int subjectSeat, int equipmentCardId)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[subjectSeat].IsAlive || !_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[subjectSeat]).Any(card => card.Id == equipmentCardId))
            return false;
        var subject = _coreMatchParticipantsPlayersAccess._players[subjectSeat];
        var projectedRange = global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(subject, equipmentCardId));
        var projectedCount = _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive && player.Seat != subjectSeat && (_characterSunYuanOlOperations.HasSayingRoundRange(subjectSeat, player.Seat) || _coreProgramPersistentCombatPoliciesOperations.IsGameFactionAttackRangeTarget(subjectSeat, player.Seat) || global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(subject, player, equipmentCardId)) <= projectedRange));
        return projectedCount < CountLivingInAttackRange(subjectSeat);
    }

    internal void SetProgramAttackRangeCoverage(long frameId, string name, int subjectSeat, int beforeCount, int afterCount) => _programPaidCardOperations.Get().SetProgramAttackRangeCoverage(frameId, name, subjectSeat, beforeCount, afterCount);
    internal static bool IsProgramAttackRangeCoverageDecreased(ProgramSkillFrame frame, string bind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramCardPayments.IsProgramAttackRangeCoverageDecreased(frame, bind);
    internal static bool MatchesProgramCardCategory(CardKind kind, IReadOnlyList<SkillProgramCardCategory> categories) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramCardPayments.MatchesProgramCardCategory(kind, categories);
    internal static bool MatchesSkillProgramCardCategory(CardKind kind, SkillProgramCardCategory category) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramCardPayments.MatchesSkillProgramCardCategory(kind, category);
    internal static bool MatchesProgramCardFilter(CardKind kind, IReadOnlyList<SkillProgramCardCategory> categories, IReadOnlyList<CardKind> kinds) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramCardPayments.MatchesProgramCardFilter(kind, categories, kinds);
    internal bool CanEnterEquipmentSlot(int recipientSeat, Card card) => _programPaidCardOperations.Get().CanEnterEquipmentSlot(recipientSeat, card);
    internal static SkillProgramCardCategory GetProgramCardCategory(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramCardPayments.GetProgramCardCategory(kind);
}
