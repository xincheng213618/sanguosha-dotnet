#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal bool WenyiActualTurnOpen => _coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.TurnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);

    internal bool IsWenyiHpTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveWenyiHealthOneAid }];
    internal bool HasWenyiHealthSubject(int targetSeat) => HasHuanShuJunCapability && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && WenyiRemaining(p.Seat) > 0 && _coreNationalSkillsOperations.GetSkillBindingShard(p).ProgramInstances.Any(i => i.Program.Triggers.Any(IsWenyiHpTrigger)));
    internal IReadOnlyList<ProgramTriggerCandidate> CollectWenyiHpCandidates(HpChangeContext change)
    {
        if (!HasHuanShuJunCapability || !WenyiActualTurnOpen || change.HpBefore == 1 || change.HpAfter != 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(change.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[change.TargetSeat].IsAlive)
            return[];
        var result = new List<ProgramTriggerCandidate>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && WenyiRemaining(p.Seat) > 0).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, p.Seat)))
            foreach (var i in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances)
                foreach (var t in i.Program.Triggers.Where(IsWenyiHpTrigger))
                    if (_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, i.SkillId, i.SkillInstanceId))
                        result.Add(new(owner.Seat, i.SkillId, t.Id, i.SkillInstanceId, i.Program.GameplayHash, t.Priority));
        return Array.AsReadOnly(result.GroupBy(c => c.OwnerSeat).Select(g => g.OrderByDescending(c => c.Priority).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).First()).ToArray());
    }

    internal ProgramSkillWindowContext CreateWenyiHpContext(HpChangeContext change, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.AfterHealthChanged, change.Id, c.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])with { HpChangeAmount = change.Amount, HpBeforeChange = change.HpBefore, HpAfterChange = change.HpAfter }, HpChange: change);
    internal bool WenyiHpRoot(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHealthChanged, HpChange: { HpAfter: 1 } change } c || change.HpBefore == 1 || c.ParentFrameId != change.Id || c.TargetSeat != change.TargetSeat || c.OwnerSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not HpChangedTriggerWindowFrame w || w.Change != change || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count)
            return false;
        var candidate = w.Candidates[w.CandidateIndex];
        return candidate.OwnerSeat == f.OwnerSeat && candidate.SkillId == f.SkillId && candidate.BindingId == f.TriggerId && candidate.SkillInstanceId == f.SkillInstanceId && candidate.GameplayHash == f.GameplayHash && w.Contexts[w.CandidateIndex].HpChange == change;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28101)]
    internal bool CanOfferWenyi(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!IsWenyiHpTrigger(trigger))
            return true;
        return WenyiActualTurnOpen && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && WenyiRemaining(c.OwnerSeat) > 0 && context.HpChange is { HpAfter: 1 } h && h.HpBefore != 1 && context.TargetSeat == h.TargetSeat && _coreMatchParticipantsPlayersAccess._players[h.TargetSeat].IsAlive;
    }

    internal bool WenyiTargetCanReceivePeach(ProgramSkillFrame f) => f.HuanShuJun is { TargetSeat: var seat } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[seat]) && !_coreCardUseModulesOperations.IsCardUseForbidden(f.OwnerSeat, CardKind.Peach, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, seat, CardKind.Peach) && (seat != f.OwnerSeat || !_coreHandComparisonOperations.HasSelfCardTargetProhibition(f.OwnerSeat));
    internal bool IsWenyiPeachRequest(ProgramSkillFrame f) => f.HuanShuJun is { Op: SkillProgramEffectOp.ResolveWenyiHealthOneAid, Stage: HuanShuJunStage.WenyiPeachRequest, Paid: false } && WenyiHpRoot(f) && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && WenyiRemaining(f.OwnerSeat) > 0 && WenyiTargetCanReceivePeach(f);
    internal bool TryGetWenyiPeachRequest(ProgramSkillFrame f, out int actor, out int target)
    {
        actor = f.OwnerSeat;
        target = f.HuanShuJun?.TargetSeat ?? -1;
        return f.HuanShuJun is { Op: SkillProgramEffectOp.ResolveWenyiHealthOneAid } r && WenyiHpRoot(f) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && (IsWenyiPeachRequest(f) || r.Paid && r.Kind == CardKind.Peach && r.Stage is HuanShuJunStage.NativeIssuing or HuanShuJunStage.NativeUse or HuanShuJunStage.NativeChildren);
    }

    internal void PrepareWenyiSuppliedPeach(ProgramSkillFrame supplied, int provider, IReadOnlyList<Card> materials, CardConversionSource? source, IReadOnlyList<CardConversionSource>? additionalSources = null, WeijiePaidOrigin? paidWeijie = null)
    {
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == supplied.Id);
        if ((!IsWenyiPeachRequest(f) && (paidWeijie is null || !IsWenyiPeachRequestAfterPaidSupply(f, paidWeijie))) || provider != f.OwnerSeat || materials.Select(c => c.Id).Distinct().Count() != materials.Count || materials.Any(c => c.Id <= 0 || !_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(c.Id)).Contains(c)))
            throw new InvalidOperationException("Wenyi lost its genuine pending Peach need or actual supplied entities.");
        var sources = (source is null ? Array.Empty<CardConversionSource>() : new[]
        {
            source
        }

        ).Concat(additionalSources ?? []).ToArray();
        if (materials.Count == 0 && sources.Length == 0)
            throw new InvalidOperationException("Wenyi cannot invent an unprovided virtual Peach.");
        var r = f.HuanShuJun!;
        if (paidWeijie is not null && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieAttemptResolvedEvent>().Count(e => e.FrameId == paidWeijie.AttemptId && e.Matched && e.NativeLegal) != 1)
            throw new InvalidOperationException("Wenyi cannot accept an unsuccessful paid Peach supply.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id && (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame child || !HuanpeiDraftRidesOn(child, f)))
            throw new InvalidOperationException("Wenyi cannot reserve another program's secondary cost.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { HuanShuJun = r with { Paid = true, Kind = CardKind.Peach, Materials = materials.Select(c => new HuanShuJunMaterial(c.Id, c.Kind, _cardZonesOperations._cardZones.GetLocation(c.Id))).ToArray(), Sources = sources, Stage = HuanShuJunStage.NativeIssuing, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenyiPaidEvent(f.Id, f.OwnerSeat, r.TargetSeat, r.WindowId, "peach"));
    }

    internal bool IsWenyiPeachRequestAfterPaidSupply(ProgramSkillFrame f, WeijiePaidOrigin paid) => f.HuanShuJun is { Op: SkillProgramEffectOp.ResolveWenyiHealthOneAid, Paid: false, Stage: HuanShuJunStage.WenyiPeachRequest } r && WenyiHpRoot(f) && r.Source == HuanShuJunSource(f) && r.GameplayHash == f.GameplayHash && paid.Need.OwnerFrameId == f.Id && paid.Need.RequestFrameId == f.Id && paid.Need.InstructionIndex == f.InstructionIndex && paid.Need.RequestOp == r.Op && paid.Need.ActorSeat == f.OwnerSeat && paid.Need.ProviderSeat == f.OwnerSeat && paid.OutputKind == CardKind.Peach && paid.TargetSeat == r.TargetSeat && paid.Need.Targets.SequenceEqual([r.TargetSeat]) && _characterGuoTuOlOperations.ValidWeijiePayment(paid) && WenyiRemaining(f.OwnerSeat) > 0;
    internal bool CanWenyiUseEquipment(int recipient, Card c) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) && _coreMatchParticipantsPlayersAccess._players[recipient].IsAlive && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && !c.IsGeneralWeapon && _coreMatchParticipantsPlayersAccess._players[recipient].EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(recipient, c.Kind, CardActionType.Use) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(_coreMatchParticipantsPlayersAccess._players[recipient], c) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(recipient, [c.Id]) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(recipient) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[recipient], c.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[recipient], c)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(recipient, recipient, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[recipient], c));
    internal void GiveWenyiEquipment(ProgramSkillFrame f, int cardId)
    {
        var r = f.HuanShuJun!;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == cardId);
        if (r.Paid || r.Stage != HuanShuJunStage.WenyiChoice || WenyiRemaining(f.OwnerSeat) == 0 || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId) || r.TargetSeat == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) || card.IsGeneralWeapon)
            throw new InvalidOperationException("Wenyi cannot give a changed or unowned equipment entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r = r with { Paid = true, Kind = card.Kind, Materials = [new(card.Id, card.Kind, from)], Stage = HuanShuJunStage.EquipmentGiftChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenyiPaidEvent(f.Id, f.OwnerSeat, r.TargetSeat, r.WindowId, "equipment"));
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(r.TargetSeat), new("skill-program.huan-shu-jun.wenyi.give"), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, r.TargetSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenyiEquipmentGivenEvent(f.Id, card.Id, r.TargetSeat, m.Sequence))));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HuanShuJun = f.HuanShuJun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueHuanShuJun(f.Id);
    }

    internal void IssueWenyiEquipmentUse(ProgramSkillFrame f)
    {
        var r = f.HuanShuJun!;
        var material = r.Materials.Single();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _cardZonesOperations._cardZones.GetLocation(material.CardId) != CardLocation.Hand(r.TargetSeat) || !CanWenyiUseEquipment(r.TargetSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId)))
        {
            RestoreWenyiRecipient(f, true);
            return;
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r with { Stage = HuanShuJunStage.NativeIssuing, Sources = [], Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        var id = _coreNativeCardUseOperations.BeginCardUse(card, r.TargetSeat, []);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(r.TargetSeat), CardLocation.Processing, CardMoveReasons.EquipmentUse);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], card, id))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], card, id);
    }

    internal void BindWenyiNativeUse(CardUseFrame supplied)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == supplied.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { HuanShuJun: { Op: SkillProgramEffectOp.ResolveWenyiHealthOneAid, Paid: true, Stage: HuanShuJunStage.NativeIssuing } r } f)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(supplied.Id)!;
        var actor = r.Kind == CardKind.Peach ? f.OwnerSeat : r.TargetSeat;
        if (use.WenyiUse is not null || r.Native is not null || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != actor || a.ProviderSeat != actor || a.RequesterSeat is not null || use.CardKind != r.Kind || !a.PhysicalCards.Select(c => c.CardId).SequenceEqual(r.Materials.Select(m => m.CardId)) || !a.ConversionChain.SequenceEqual(r.Sources) || (r.Kind == CardKind.Peach ? !use.TargetSeats.SequenceEqual([r.TargetSeat]) : use.CardId != r.Materials.Single().CardId))
            throw new InvalidOperationException("Wenyi's true native Use changed its provider, actor, card or recipient.");
        var origin = new WenyiNativeOrigin(f.Id, r.Source, r.GameplayHash, actor, actor, r.TargetSeat, use.CardKind, use.Id, a.ActionId, a);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { HuanShuJun = r with { Stage = HuanShuJunStage.NativeUse, Native = origin } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { WenyiUse = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenyiNativeIssuedEvent(f.Id, use.Id, a.ActionId, actor, actor, r.TargetSeat, use.CardKind));
    }

    internal bool IsIssuedWenyiUse(CardUseFrame use)
    {
        if (use.WenyiUse is not { } o || use.Action is not { Type: CardActionType.Use } a || o.UseId != use.Id || a.ActionId != o.ActionId || use.CardKind != o.Kind || !a.PhysicalCards.SequenceEqual(o.OriginalAction.PhysicalCards) || !a.ConversionChain.SequenceEqual(o.OriginalAction.ConversionChain) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.ProgramFrameId)is not ProgramSkillFrame { HuanShuJun: { Stage: HuanShuJunStage.NativeUse, NativeReturned: false, Native: { } n } } f || n.UseId != o.UseId || n.ActionId != o.ActionId || !SameHuanShuJunAction(n.OriginalAction, o.OriginalAction) || !WenyiHpRoot(f))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WenyiNativeIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == use.Id && e.ActionId == a.ActionId && e.ActorSeat == o.ActorSeat && e.ProviderSeat == o.ProviderSeat && e.TargetSeat == o.TargetSeat && e.Kind == o.Kind) == 1;
    }

    [NativeUseValidation(28101)]
    internal void ValidateWenyiNative(CardUseFrame use)
    {
        if (use.WenyiUse is not null && !IsIssuedWenyiUse(use))
            throw new InvalidOperationException("Wenyi lost its exact paid native child.");
    }

    [NativeUseReturn(28101)]
    internal void ReturnWenyiNative(CardUseFrame use)
    {
        if (use.WenyiUse is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        var r = f.HuanShuJun!;
        if (r.NativeReturned || r.Native?.UseId != use.Id || r.Native.ActionId != o.ActionId || use.Step != ResolutionFrameStep.Completed || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Wenyi's native use returned before completion or more than once.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r with { NativeReturned = true, Stage = HuanShuJunStage.NativeChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenyiNativeReturnedEvent(f.Id, use.Id, o.ActionId));
        ContinueHuanShuJun(f.Id);
    }

    internal void RestoreWenyiRecipient(ProgramSkillFrame f, bool face)
    {
        var r = f.HuanShuJun!;
        if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishHuanShuJun(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuanShuJun = r with { Stage = face ? HuanShuJunStage.FaceChildren : HuanShuJunStage.ChainChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenyiRestorationIssuedEvent(f.Id, r.TargetSeat, face));
        if (face)
            _coreProgramLifecycleOperations.SetProgramTargetFaceState(f.Id, f.OwnerSeat, r.TargetSeat, false);
        else
            _coreProgramLifecycleOperations.SetProgramChainedState(f, false, r.TargetSeat);
        ContinueHuanShuJun(f.Id);
    }

    internal void FinishUnissuedWenyiPeach(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HuanShuJun is not { Op: SkillProgramEffectOp.ResolveWenyiHealthOneAid, Paid: true, Stage: HuanShuJunStage.NativeIssuing, Native: null })
            throw new InvalidOperationException("Only Wenyi's exact paid but unissued Peach can take the native failure return.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        RestoreWenyiRecipient(f, true);
    }

    internal static bool SameHuanShuJunAction(CardActionContext a, CardActionContext b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_huan_shu_jun_ol_Wenyi.SameHuanShuJunAction(a, b);
    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 50, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveWenyiHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsWenyiHpTrigger(trigger) ? CanOfferWenyi(candidate, trigger, context) : null;
}
