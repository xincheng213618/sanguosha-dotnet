#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNamedCardAcquisitionsRuntime
{
    internal static string NamedAcquisitionReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_NamedCardAcquisitions.NamedAcquisitionReason(f);
    internal IEnumerable<(Card Card, CardLocation From)> PublicNamedCardCandidates(CardKind kind)
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var zone in new[]
            {
                CardLocation.Equipment(p.Seat),
                CardLocation.Judgment(p.Seat)
            }

            )
                foreach (var c in _cardZonesOperations._cardZones.CardsAt(zone).Where(c => c.Kind == kind && !c.IsGeneralWeapon))
                    yield return (c, zone);
        foreach (var c in _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => c.Kind == kind && !c.IsGeneralWeapon))
            yield return (c, CardLocation.DiscardPile);
    }

    internal Card? DeckNamedCard(CardKind kind) => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).FirstOrDefault(c => c.Kind == kind && !c.IsGeneralWeapon);
    internal IReadOnlyList<PromptChoice> NamedCardAcquisitionChoices(ProgramSkillFrame f)
    {
        if (f.NamedCardAcquisition is not { Stage: NamedCardAcquisitionStage.Selecting } d)
            return[];
        PromptChoice Choice(string suffix, string label, IReadOnlyList<int> cards, CardLocation from) => new(new($"named-acquisition.{(f.Id)}.{(suffix)}"), label, cards, [], new Dictionary<string, string> { ["program-action"] = "named-card-acquisition", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["source-zone"] = from.Zone.ToString(), ["source-owner"] = (from.OwnerSeat ?? -1).ToString(CultureInfo.InvariantCulture) });
        var list = PublicNamedCardCandidates(d.PrintedKind).Select(c => Choice($"public-{(c.Card.Id)}", $"获得{(c.Card.DisplayName)}（{(c.From.Zone)}）", [c.Card.Id], c.From)).ToList();
        if (DeckNamedCard(d.PrintedKind)is not null)
            list.Add(Choice("deck", "从牌堆获得指定牌", [], CardLocation.DrawPile));
        return Array.AsReadOnly(list.ToArray());
    }

    internal SkillProgramStepOutcome BeginNamedCardAcquisition(ProgramSkillFrame supplied, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.NamedCardAcquisition is not null || e.OutputKind is not { } kind || e.SkillIds.Contains(f.SkillId))
            throw new InvalidOperationException("A named-card acquisition cannot replace itself or overwrite an issued draft.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var grants = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(g => e.SkillIds.Contains(g.SkillId)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedCardAcquisition = new(f.InstructionIndex, kind, NamedCardAcquisitionStage.Selecting, true) });
        foreach (var g in grants)
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.RemoveGrant(g.GrantId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSelectedSkillsLostEvent(f.Id, f.OwnerSeat, f.SkillId, grants.Length));
        if (NamedCardAcquisitionChoices(f).Count == 0)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramNamedCardAcquiredEvent(f.Id, f.OwnerSeat, kind, null, false));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { NamedCardAcquisition = null });
            return SkillProgramStepOutcome.Continue;
        }

        PublishNamedCardAcquisition(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishNamedCardAcquisition(ProgramSkillFrame f)
    {
        var choices = NamedCardAcquisitionChoices(f);
        if (choices.Count == 0)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramNamedCardAcquiredEvent(f.Id, f.OwnerSeat, f.NamedCardAcquisition!.PrintedKind, null, false));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { NamedCardAcquisition = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择指定牌的公开来源，或从牌堆获得。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = false,
            SkillPrompt = new(f.SkillId, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name, "获得指定牌", _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("named-card-acquisition")]
    internal void ResolveNamedCardAcquisitionChoice(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { NamedCardAcquisition: { Stage: NamedCardAcquisitionStage.Selecting } d } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !NamedCardAcquisitionChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards)) || !Enum.TryParse<CardZoneKind>(selected.Parameters.GetValueOrDefault("source-zone"), out var zone))
            throw new InvalidOperationException("Named-card acquisition requires its exact currently published source choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { NamedCardAcquisition = null });
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), "指定牌收益尚未发行，原来源已失效。");
            return;
        }

        Card card;
        CardLocation from;
        if (zone == CardZoneKind.DrawPile)
        {
            card = DeckNamedCard(d.PrintedKind) ?? throw new InvalidOperationException("The declared deck kind disappeared.");
            from = CardLocation.DrawPile;
        }
        else
        {
            var candidate = PublicNamedCardCandidates(d.PrintedKind).Single(c => selected.Cards.SequenceEqual([c.Card.Id]));
            card = candidate.Card;
            from = candidate.From;
        }

        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedCardAcquisition = d with { Stage = NamedCardAcquisitionStage.MovementChildren, CardId = card.Id, From = from, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new(NamedAcquisitionReason(f)));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedCardAcquisition = f.NamedCardAcquisition!with { SequenceAfter = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? before } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramNamedCardAcquiredEvent(f.Id, f.OwnerSeat, d.PrintedKind, from.Zone, true));
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
    }

    internal bool ValidNamedCardAcquisition(ProgramSkillFrame f)
    {
        if (f.NamedCardAcquisition is not { } d || !_coreNamedAcquisitionObserversOperations.Value.NamedAcquisitionParentMatches(f) || f.InstructionIndex != d.InstructionIndex || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect is not { Op: SkillProgramEffectOp.LoseSkillsAndObtainNamedCard } effect || effect.OutputKind != d.PrintedKind || !d.SkillsRemoved || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSelectedSkillsLostEvent>().Count(e => e.ProgramFrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.InitiatingSkillId == f.SkillId) != 1)
            return false;
        if (d.Stage == NamedCardAcquisitionStage.Selecting)
            return d.CardId is null && d.From is null;
        return d.Stage == NamedCardAcquisitionStage.MovementChildren && d.CardId is { } card && d.From is { } from && d.SequenceAfter > d.SequenceBefore && from.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment or CardZoneKind.DrawPile or CardZoneKind.DiscardPile && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > d.SequenceBefore && m.Sequence <= d.SequenceAfter && m.CardId == card && m.CardKind == d.PrintedKind && m.From == from && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == NamedAcquisitionReason(f)) == 1;
    }

    internal void AssertNamedCardAcquisition(ProgramSkillFrame f)
    {
        if (f.NamedCardAcquisition is not null && !ValidNamedCardAcquisition(f))
            throw new InvalidOperationException("The named-card acquisition lost its exact issuance or original movement.");
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 360)]
    internal bool ResumeNamedCardAcquisition(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { NamedCardAcquisition: { } d } f || f.Id != id)
            return false;
        AssertNamedCardAcquisition(f);
        if (d.Stage == NamedCardAcquisitionStage.Selecting)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { NamedCardAcquisition = null });
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), "指定牌收益尚未发行，原来源已失效。");
            }
            else if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishNamedCardAcquisition(f);
            return true;
        }

        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnNamedCardAcquisitionMovement(f);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { NamedCardAcquisition = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), "指定牌的已发行移动与子结算已结清。");
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 360)]
    internal bool ReturnNamedCardAcquisitionMovement(ProgramSkillFrame f)
    {
        if (f.NamedCardAcquisition is not { Stage: NamedCardAcquisitionStage.MovementChildren })
            return false;
        AssertNamedCardAcquisition(f);
        if (_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        ResumeNamedCardAcquisition(f.Id);
        return true;
    }
}
