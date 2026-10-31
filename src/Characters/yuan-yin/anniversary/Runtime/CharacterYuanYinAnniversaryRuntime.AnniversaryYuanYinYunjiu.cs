#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanYinAnniversaryRuntime
{
    internal static bool IsAnniversaryYuanYinYunjiu(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveAnniversaryYuanYinYunjiu }];
    internal static CardMoveReason YuanYinGiftReason => new("skill-program.anniversary:yunjiu.give");

    internal IEnumerable<int> YuanYinRecipients(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner).Select(p => p.Seat);
    internal bool YuanYinDeathHasCards(int victim) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[victim]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[victim])).Any(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(victim, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Transfer));
    internal bool TryBeginAnniversaryYuanYinDeathBeforeCleanup(long deathId)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryYuanYinYunjiu) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DeathFrame death || death.Id != deathId || _coreMatchParticipantsPlayersAccess._players[death.VictimSeat].IsAlive || death.CleanedUpCardIds.Count != 0 || !YuanYinDeathHasCards(death.VictimSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryYuanYinDeathWindowIssuedEvent>().Any(e => e.DeathId == deathId))
            return false;
        var issued = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && YuanYinRecipients(p.Seat).Any()).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.CharacterDied).Where(c => IsAnniversaryYuanYinYunjiu(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).Select(c => (Candidate: c, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(p)))).OrderBy(x => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, x.Candidate.OwnerSeat)).ThenByDescending(x => x.Candidate.Priority).ThenBy(x => x.Candidate.SkillId, StringComparer.Ordinal).ThenBy(x => x.Candidate.SkillInstanceId, StringComparer.Ordinal).ToArray();
        if (issued.Length == 0)
            return false;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinDeathWindowIssuedEvent(id, death.Id, death.VictimSeat, death.KillerSeat, issued.Length));
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramKillTriggerWindowFrame(id, death.Id, death.VictimSeat, death.KillerSeat, issued.Select(x => x.Candidate).ToArray(), issued.Select(x => new ProgramSkillWindowContext(SkillProgramTriggerWindow.CharacterDied, id, x.Candidate.OwnerSeat, SourceSeat: death.KillerSeat, TargetSeat: death.VictimSeat, OccurrenceIndex: x.Candidate.OccurrenceIndex, Facts: x.Facts)).ToArray()) { BeforeDeathCleanup = true });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramKillTriggerWindowFrame>();
        return true;
    }

    internal bool YuanYinIssuedDeathWindow(ProgramKillTriggerWindowFrame window, DeathFrame death) => window.BeforeDeathCleanup && window.DeathFrameId == death.Id && window.VictimSeat == death.VictimSeat && window.KillerSeat == death.KillerSeat && !_coreMatchParticipantsPlayersAccess._players[death.VictimSeat].IsAlive && death.CleanedUpCardIds.Count == 0 && window.Candidates.Count > 0 && window.Contexts.Count == window.Candidates.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryYuanYinDeathWindowIssuedEvent>().Count(e => e == new AnniversaryYuanYinDeathWindowIssuedEvent(window.Id, death.Id, death.VictimSeat, death.KillerSeat, window.Candidates.Count)) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryYuanYinDeathWindowCompletedEvent>().Any(e => e.WindowId == window.Id || e.DeathId == death.Id) && window.Candidates.Select((c, i) => (c, Context: window.Contexts[i])).All(x => IsAnniversaryYuanYinYunjiu(_coreProgramLifecycleOperations.GetProgramTrigger2(x.c)) && x.Context.Window == SkillProgramTriggerWindow.CharacterDied && x.Context.ParentFrameId == window.Id && x.Context.OwnerSeat == x.c.OwnerSeat && x.Context.TargetSeat == death.VictimSeat && x.Context.SourceSeat == death.KillerSeat);
    internal void CompleteAnniversaryYuanYinDeathBeforeCleanupWindow(ProgramKillTriggerWindowFrame window, DeathFrame death)
    {
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault() != window || i < 1 || _resolutionStackOperations._resolutionStack[i - 1].Id != death.Id || window.CandidateIndex != window.Candidates.Count || !YuanYinIssuedDeathWindow(window, death))
            throw new InvalidOperationException("Yunjiu cannot repeat cleanup or return a forged normal post-cleanup death window.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinDeathWindowCompletedEvent(window.Id, death.Id));
    }

    internal bool CanOfferYuanYinYunjiu(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HasHpValue && w is { Window: SkillProgramTriggerWindow.CharacterDied, TargetSeat: { } victim } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive && YuanYinRecipients(c.OwnerSeat).Any() && YuanYinDeathHasCards(victim) && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is ProgramKillTriggerWindowFrame { BeforeDeathCleanup: true } parent && parent.VictimSeat == victim && parent.KillerSeat == w.SourceSeat && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == c && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == parent.DeathFrameId)is DeathFrame death && YuanYinIssuedDeathWindow(parent, death);
    internal SkillProgramStepOutcome BeginYuanYinYunjiu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryYuanYin is not null || f.InstructionIndex != 1 || f.SkillId != AnniversaryYuanYinComposition.Yunjiu || f.TriggerId != "dead-owned-card-gift" || f.WindowContext is not { } w || !CanOfferYuanYinYunjiu(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), w))
            throw new InvalidOperationException("Yunjiu requires the original dead owner's actual HE before native cleanup.");
        var parent = (ProgramKillTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(p => p.Id == w.ParentFrameId);
        var r = new AnniversaryYuanYinReceipt(YuanYinSource(f), f.GameplayHash, parent.Id, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, "yunjiu", "recipient")
        {
            DeathId = parent.DeathFrameId,
            Victim = parent.VictimSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYuanYin = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.Mode));
        PublishYuanYin(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> YuanYinChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryYuanYin!;
        if (r.Stage == "recipient")
            return Array.AsReadOnly(YuanYinRecipients(f.OwnerSeat).Select(seat => new PromptChoice(new($"yuan-yin.{(f.Id)}.recipient.{(seat)}"), "交给" + _coreMatchParticipantsPlayersAccess._players[seat].Name, [], [seat], new Dictionary<string, string> { ["program-action"] = "anniversary-yuan-yin", ["branch"] = "recipient" })).ToArray());
        if (r.Stage != "card" || !_coreMatchParticipantsPlayersAccess._players[r.Recipient].IsAlive)
            return[];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Victim, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).Select(c => c with { Id = new($"yuan-yin.{(f.Id)}.{(c.Id.Value)}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "anniversary-yuan-yin", ["branch"] = "card" } }).ToArray());
    }

    internal void PublishYuanYin(ProgramSkillFrame f)
    {
        var choices = YuanYinChoices(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || choices.Count == 0)
        {
            FinishYuanYin(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.AnniversaryYuanYin!.Stage == "recipient" ? "运柩：选择获得死者牌的其他角色。" : "运柩：交给其一张死者的牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-yuan-yin")]
    internal void AnswerYuanYin(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryYuanYin: { Mode: "yunjiu" } r } f || !ValidYuanYin(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !YuanYinChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Yunjiu rejects a changed native death, another recipient or a stale opaque card slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == "recipient")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYuanYin = r with { Recipient = selected.Targets.Single(), Stage = "card" } });
            PublishYuanYin(f);
            return;
        }

        var from = new CardLocation(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), r.Victim);
        var id = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)].Id;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYuanYin = r with { Stage = "gift-children", CardId = id, From = from, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(r.Recipient), YuanYinGiftReason, afterRequestSettled: settlement =>
        {
            _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(settlement, f.Id, [id], CardLocation.Hand(r.Recipient), before);
            var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = live.AnniversaryYuanYin!with
            {
                Gain = settlement,
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                Delivered = settlement.Movements.Any(m => m.CardId == id && m.From == from && m.To == CardLocation.Hand(r.Recipient))
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(live with { AnniversaryYuanYin = paid });
            _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, r.Victim, r.Recipient, [id], before, settlement);
        }, producerFrameId: f.Id);
        var settled = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(settled = settled with { AnniversaryYuanYin = settled.AnniversaryYuanYin!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinGiftPaidEvent(f.Id, r.DeathId, r.Victim, r.Recipient, settled.AnniversaryYuanYin!.Delivered, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueYuanYin(f.Id);
    }

    internal void ContinueYuanYinYunjiu(ProgramSkillFrame supplied)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryYuanYin: { Mode: "yunjiu" } r } f && f.Id == supplied.Id)
        {
            if (!ValidYuanYin(f))
                throw new InvalidOperationException("Yunjiu lost its original pre-cleanup death, real gift or once-issued benefit.");
            if (r.Stage is "recipient" or "card")
            {
                PublishYuanYin(f);
                return;
            }

            if (DrainYuanYin(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.AnniversaryYuanYin!;
            if (r.Stage == "recovery-children" || !r.Delivered || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishYuanYin(f);
                return;
            }

            if (r.Stage == "gift-children")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, AnniversaryYuanYin = r with { Stage = "maximum-children" } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinBenefitIssuedEvent(f.Id, "maximum-hp", f.OwnerSeat, 1));
                _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 1);
                continue;
            }

            if (r.Stage != "maximum-children")
                throw new InvalidOperationException("Yunjiu cannot issue recovery before its actual native maximum-HP child returns.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYuanYin = r with { Stage = "recovery-children" } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinBenefitIssuedEvent(f.Id, "recovery", f.OwnerSeat, 1));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
        }
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryYuanYinYunjiu)]
    internal PromptChoice ChooseYuanYin(PendingDecision p) => p.Choices.First();
}
