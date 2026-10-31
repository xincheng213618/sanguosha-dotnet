#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangJiOlRuntime
{
    internal int ZhangJiLueMingCount(int owner) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhangJiStartedEvent>().Count(e => e.Source.OwnerSeat == owner && !e.Equipment);
    internal bool CanBeginZhangJiJudgment(ProgramSkillFrame f, int targetSeat, string reason, string bind, SkillProgramCardSetVisibility visibility) => f.ZhangJi is { Stage: "judgment", Equipment: false, JudgmentReturned: false, ChosenRank: >= 1 and <= 13, JudgmentId: not null } && ValidZhangJi(f) && targetSeat == f.OwnerSeat && reason == "lueming" && bind == "zhang-ji-rank" && visibility == SkillProgramCardSetVisibility.Public;
    internal bool IsZhangJiJudgment(JudgmentFrame j, ProgramSkillFrame f) => f.ZhangJi is { Equipment: false, Stage: "judgment", JudgmentReturned: false, JudgmentId: { } id } r && ValidZhangJi(f) && j.Id == id && j.ParentFrameId == f.Id && j.TargetSeat == f.OwnerSeat && j.SourceSeat == f.OwnerSeat && j.Continuation == JudgmentContinuationKind.ProgramSkill && j.Reason == "lueming" && j.ProgramResultBind == "zhang-ji-rank" && j.ProgramResultVisibility == SkillProgramCardSetVisibility.Public;
    internal bool TryReturnZhangJiJudgment(JudgmentFrame j)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == j.ParentFrameId)is not { ZhangJi: not null } f)
            return false;
        if (!IsZhangJiJudgment(j, f))
            throw new InvalidOperationException("Lueming received a foreign judgment return.");
        var result = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JudgmentResolvedEvent>().Single(e => e.ResolutionId == j.Id);
        var finalCard = _coreNativeDamageHandlesOperations.GetJudgmentCard(j);
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "zhang-ji-rank", finalCard is null ? [] : [finalCard.Id], SkillProgramCardSetVisibility.Public, finalCard is null ? [] : [_cardZonesOperations._cardZones.GetLocation(finalCard.Id)], finalCard is null ? null : _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentSuit(j, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[j.TargetSeat], finalCard)));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = f.ZhangJi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangJi = r with { JudgmentReturned = true, JudgmentCard = result.CardId, FinalRank = result.Rank, Stage = "outcome" } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangJiJudgmentReturnedEvent(f.Id, j.Id, result.CardId, result.Rank));
        ResumeZhangJi(f.Id);
        return true;
    }

    internal void IssueZhangJiOutcome(ProgramSkillFrame f)
    {
        var r = f.ZhangJi!;
        if (!_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishZhangJi(f);
            return;
        }

        if (r.FinalRank == r.ChosenRank)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangJi = r with { Stage = "damage" } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangJiDamageIssuedEvent(f.Id, f.OwnerSeat, r.Target, 2));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Target, 2);
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishZhangJi(f);
            return;
        }

        var cards = new[]
        {
            CardLocation.Hand(r.Target),
            CardLocation.Equipment(r.Target),
            CardLocation.Judgment(r.Target)
        }.SelectMany(loc => _cardZonesOperations._cardZones.CardsAt(loc)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Obtain)).OrderBy(c => c.Id).ToArray();
        if (cards.Length == 0)
        {
            FinishZhangJi(f);
            return;
        }

        var card = cards[_randomOperations._random.Next(cards.Length)];
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangJi = r with { Stage = "movement", Before = before, After = before, GainCard = card.Id, GainFrom = from }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(ZhangJiGainReason), (_, records) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { ZhangJi = current.ZhangJi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangJiCardGainedEvent(f.Id, r.Target, f.OwnerSeat, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
    }
}
