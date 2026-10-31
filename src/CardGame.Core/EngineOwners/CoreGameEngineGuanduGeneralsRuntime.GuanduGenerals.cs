#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduGeneralsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IGuanduGeneralHost.Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginGuanduGeneral(frame, operation);
    internal bool GuanduSkillEnabled(CharacterState owner, string skill) => owner.IsAlive && _contentRegistryOperations._contentRegistry.Skills.ContainsKey(skill) && _coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(p => p.Id == skill);
    internal bool GuanduGeneralGrantQualified(CharacterState owner, SkillGrant grant)
    {
        if (grant.SkillId is not ("guandu:zezhu" or "guandu:chenggong"))
            return true;
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || !owner.IsAlive || !owner.SkillGrants.Grants.Any(g => g.SkillId == "guandu:fushi" && g.IsEnabled && _corePrivateGeneralLibrariesOperations.LibraryBaseGrantLive(owner, g) && _coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, g)))
            return false;
        var qun = _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(p) == "qun");
        var wei = _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(p) == "wei");
        return grant.SkillId == "guandu:zezhu" ? qun > wei : wei > qun;
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 20)]
    internal IEnumerable<CardSnapshot> GuanduShicaiPrivatelyViewedCards(int viewerSeat) => _coreGuanduActivitiesOperations.IsGuanduActivity && viewerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && GuanduSkillEnabled(_coreMatchParticipantsPlayersAccess._players[viewerSeat], "guandu:shicai") && _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault()is { } top ? new[]
    {
        _coreSnapshotValueProjectionOperations.ToSnapshot(top)
    }

    : [];
    internal bool GuanduShicaiReady(int ownerSeat, string instance)
    {
        var acquired = _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduShicaiObtainedEvent>().LastOrDefault(e => e.OwnerSeat == ownerSeat && e.InstanceId == instance);
        return acquired is null || _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == acquired.CardId && m.Sequence > acquired.MovementSequence && m.From == CardLocation.Hand(ownerSeat) && m.To != m.From);
    }

    internal IEnumerable<RuleQueryContribution> GuanduGeneralRuleContributions(CharacterState player, SkillRuleQuery query)
    {
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || query != SkillRuleQuery.DrawCount)
            yield break;
        var sources = _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && _coreGuanduActivitiesOperations.GuanduAllies(p.Seat, player.Seat) && p.Markers.GetValueOrDefault(PlayerMarkerKind.GuanduGrain) > 0 && GuanduSkillEnabled(p, "guandu:liangying"));
        if (sources != 0)
            yield return new FiniteRuleQueryContribution("guandu:liangying", SkillRuleOperation.Add, sources);
    }

    internal void CaptureGuanduDiscardBatch(CardMovementBatchContext batch)
    {
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || batch.DiscardRecoveryPhase is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase)
            return;
        foreach (var movement in batch.Movements)
            if (_coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement)is { OwnerSeat: { } owner } from && owner == phase.ActorSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<GuanduDiscardFactEvent>().Any(e => e.MovementSequence == movement.Sequence))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduDiscardFactEvent(batch.Id, phase.TurnNumber, owner, phase.Token, movement.CardId, movement.Sequence, from));
    }

    internal bool GuanduTurnDiscarded(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduDiscardFactEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseActor == seat);
    internal Card[] GuanduOwnedCards(int seat, bool judgment = false) => (judgment ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.Judgment(seat))) : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]))).ToArray();
    internal Card[] GuanduDiscardable(int chooser, int owner) => GuanduOwnedCards(owner).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal static IReadOnlyList<int> FreezeGuanduSeats(IEnumerable<int> seats) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_GuanduGenerals.FreezeGuanduSeats(seats);
    internal bool IsGuanduGeneralContextAllowed(string skillId, SkillProgramEffectOp operation) => _coreGuanduActivitiesOperations.IsGuanduActivity || _coreMatchDefinitionOptionsAccess._options.ShanheBattle is not null && skillId == "guandu:moushi" && operation == SkillProgramEffectOp.ResolveGuanduMoushi || _coreShanheSourceGrainOperations.IsShanheSourceGrainContext(skillId, operation);
    internal SkillProgramStepOutcome BeginGuanduGeneral(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsGuanduGeneralContextAllowed(f.SkillId, operation) || f.GuanduGeneral is not null || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != operation)
            throw new InvalidOperationException("An activity general requires its exact unpaid owning instruction.");
        var r = new GuanduGeneralReceipt(operation, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, f.WindowContext?.ParentFrameId, "begin", Seats: FreezeGuanduSeats([]));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanduGeneral = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduGeneralIssuedEvent(f.Id, f.SkillId, operation, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, r.WindowId));
        RunGuanduGeneral(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainGuanduGeneralChildren(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 44000)]
    internal bool RunGuanduGeneral(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GuanduGeneral: { } r } f || f.Id != id)
            return false;
        if (r.Stage == "draw-attempt" || r.Stage == "native-use")
            return true;
        if (DrainGuanduGeneralChildren(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.GuanduGeneral!;
        if (r.Stage == "finish")
        {
            FinishGuanduGeneral(f);
            return true;
        }

        switch (r.Operation)
        {
            case SkillProgramEffectOp.ResolveGuanduShicai:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduShicai(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduZezhu:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduZezhu(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduJieliang:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduJieliang(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduJieliangClaim:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduJieliangClaim(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduCangchu:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduCangchu(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduSushou:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduSushou(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduPolu:
                _coreGuanduEquipmentOperations.ContinueGuanduPolu(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduThunderChariot:
                _coreGuanduEquipmentOperations.ContinueGuanduThunderChariot(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduChoulue:
                _coreGuanduGeneralNativeCardsOperations.ContinueGuanduChoulue(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduHengjiang:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduHengjiang(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduHengjiangReward:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduHengjiangReward(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduXiying:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduXiying(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduBeizhan:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduBeizhan(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduYuanlve:
                _coreGuanduGeneralNativeCardsOperations.ContinueGuanduYuanlve(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduMoushi:
                _coreGuanduGeneralNativeCardsOperations.ContinueGuanduMoushi(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduZhongjie:
                _coreGuanduGeneralBenefitsOperations.ContinueGuanduZhongjie(f);
                break;
            case SkillProgramEffectOp.ResolveGuanduSuishiDying:
                DrawGuanduGeneral(f, f.OwnerSeat, 1, "finish");
                break;
            case SkillProgramEffectOp.ResolveGuanduSuishiDeath:
                GuanduStage(f, "finish");
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, 1);
                RunGuanduGeneral(id);
                break;
            case SkillProgramEffectOp.ResolveGuanduFuyuan:
                DrawGuanduGeneral(f, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat]).Count < _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count ? _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat : f.OwnerSeat, 1, "finish");
                break;
            default:
                throw new InvalidOperationException("Unknown activity general producer.");
        }

        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 44000)]
    internal bool ReturnGuanduGeneralMovement(ProgramSkillFrame f) => f.GuanduGeneral is not null && RunGuanduGeneral(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 44000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 44000)]
    internal bool ContinueIssuedGuanduGeneral(ProgramSkillFrame f) => f.GuanduGeneral is not null;
    internal ProgramSkillFrame GuanduStage(ProgramSkillFrame f, string stage) => GuanduUpdate(f, f.GuanduGeneral!with { Stage = stage });
    internal ProgramSkillFrame GuanduUpdate(ProgramSkillFrame f, GuanduGeneralReceipt r)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GuanduGeneral = r });
        return f;
    }

    internal void DrawGuanduGeneral(ProgramSkillFrame f, int recipient, int count, string next)
    {
        f = GuanduUpdate(f, f.GuanduGeneral!with { Stage = "draw-attempt", DrawOrdinal = f.GuanduGeneral.DrawOrdinal + 1, DrawRecipient = recipient, DrawRequested = count, NextStage = next, Draw = null });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, count, new("activity.guandu." + f.SkillId + ".draw"));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveGuanduZezhu, SkillProgramEffectOp.ResolveGuanduCangchu, SkillProgramEffectOp.ResolveGuanduSushou, SkillProgramEffectOp.ResolveGuanduPolu, SkillProgramEffectOp.ResolveGuanduHengjiangReward, SkillProgramEffectOp.ResolveGuanduBeizhan, SkillProgramEffectOp.ResolveGuanduYuanlve, SkillProgramEffectOp.ResolveGuanduMoushi, SkillProgramEffectOp.ResolveGuanduZhongjie, SkillProgramEffectOp.ResolveGuanduSuishiDying, SkillProgramEffectOp.ResolveGuanduFuyuan)]
    internal void ReturnGuanduGeneralDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        var r = f.GuanduGeneral ?? throw new InvalidOperationException("Activity draw lost its producer.");
        if (r.Stage != "draw-attempt" || outcome.RecipientSeat != r.DrawRecipient || outcome.RequestedCount != r.DrawRequested || _coreProgramEventHistoryOperations.ProgramEventHistory<GuanduGeneralDrawEvent>().Any(e => e.FrameId == f.Id && e.Ordinal == r.DrawOrdinal))
            throw new InvalidOperationException("Activity draw changed its once-paid recipient or count.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduGeneralDrawEvent(f.Id, r.DrawOrdinal, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId, outcome.Replacement is not null));
        GuanduUpdate(f, r with { Stage = r.NextStage!, NextStage = null, Draw = outcome });
        RunGuanduGeneral(f.Id);
    }

    internal void FinishGuanduGeneral(ProgramSkillFrame f, bool completed = true)
    {
        var operation = f.GuanduGeneral!.Operation;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduGeneralCompletedEvent(f.Id, f.SkillId, completed));
        if (operation == SkillProgramEffectOp.ResolveGuanduChoulue && !f.ChoiceBindings.Any(b => b.Name == "choulue-kind"))
            f = f with
            {
                ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding("choulue-kind", "none", f.OwnerSeat)).ToArray())
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GuanduGeneral = null });
        if (operation == SkillProgramEffectOp.ResolveGuanduChoulue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        else
            _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 5300)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureGuanduDiscardBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureGuanduDiscardBatch(completed);
        return completed;
    }
}
