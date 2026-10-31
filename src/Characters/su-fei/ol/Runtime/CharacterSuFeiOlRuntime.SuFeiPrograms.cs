#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSuFeiOlRuntime
{
    internal const string SuFeiDrawReason = "skill-program.lianpian.draw", SuFeiGiftReason = "skill-program.lianpian.gift";
    internal static CardConversionSource SuFeiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_su_fei_ol_SuFeiPrograms.SuFeiSource(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 450)]
    internal bool CanOfferSuFei(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (!ProgramInstructionResolver.Default.Features(t).HasOperation(SkillProgramEffectOp.DrawThenOfferActualConsecutiveTargetGift))
            return true;
        return w.CardUse is { } identity && _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is { SuFeiQualification: { } q, Action: { } a } && a.ActionId == identity.CardActionId && ValidSuFeiQualification(q, c.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginSuFei(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SuFei is not null || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } w || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { SuFeiQualification: { } q, Action: { } a } use || a.ActionId != identity.CardActionId || !ValidSuFeiQualification(q, f.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Lianpian lost its unpaid exact consecutive native use.");
        var r = new SuFeiReceipt(SuFeiSource(f), f.GameplayHash, f.InstructionIndex, w.ParentFrameId, q, SuFeiStage.DrawChildren, DrawBefore: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawAfter: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuFei = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var cards = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(SuFeiDrawReason));
        r = r with
        {
            DrawAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            DrawnId = cards.Count == 1 ? cards[0] : null,
            ActualDraw = cards.Count
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { SuFei = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LianpianDrawPaidEvent(f.Id, r.Source, r.GameplayHash, q.Current.NativeUseId, q.Current.Action.ActionId, r.DrawBefore, r.DrawAfter, r.ActualDraw));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool CanGiveSuFei(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && f.SuFei is { ActualDraw: 1, DrawnId: { } id } r && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat) && r.Qualification.CommonTargets.Any(s => s != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive);
    internal IReadOnlyList<PromptChoice> SuFeiChoices(ProgramSkillFrame f)
    {
        if (f.SuFei is not { Stage: SuFeiStage.ChoosingGift } r || !CanGiveSuFei(f))
            return[];
        var choices = r.Qualification.CommonTargets.Where(s => s != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).Select(s => new PromptChoice(new($"lianpian.{(f.Id)}.give-{(s)}"), $"将刚摸到的牌交给{(_coreMatchParticipantsPlayersAccess._players[s].Name)}", [r.DrawnId!.Value], [s], new Dictionary<string, string> { { "program-action", "lianpian" }, { "branch", "give" } })).ToList();
        choices.Add(new(new($"lianpian.{(f.Id)}.keep"), "保留刚摸到的牌", [], [], new Dictionary<string, string> { { "program-action", "lianpian" }, { "branch", "keep" } }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishSuFeiChoice(ProgramSkillFrame f)
    {
        var choices = SuFeiChoices(f);
        if (choices.Count == 0)
        {
            FinishSuFei(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "可将本次实际摸到的牌交给共同目标。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.DrawThenOfferActualConsecutiveTargetGift)]
    internal PromptChoice SuFeiAiChoice(ProgramSkillFrame f) => SuFeiChoices(f).Last();
    [ProgramAnswer("lianpian")]
    internal void ResolveSuFeiChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        RequireSuFei(f);
        var r = f.SuFei!;
        if (r.Stage != SuFeiStage.ChoosingGift || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], SuFeiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Lianpian cannot replace its drawn entity or gift authority.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters.GetValueOrDefault("branch") == "keep")
        {
            FinishSuFei(f);
            return;
        }

        var target = choice.Targets.Single();
        if (!CanGiveSuFei(f) || !r.Qualification.CommonTargets.Contains(target) || !choice.Cards.SequenceEqual([r.DrawnId!.Value]))
            throw new InvalidOperationException("Lianpian lost its exact drawn entity or common target.");
        r = r with
        {
            Stage = SuFeiStage.GiftChildren,
            GiftTarget = target,
            GiftBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            GiftAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuFei = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([r.DrawnId.Value], CardLocation.Hand(target), new(SuFeiGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, target, [r.DrawnId.Value], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, moves) =>
        {
            var paid = r with
            {
                GiftAfter = moves.Single().Sequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { SuFei = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LianpianGiftPaidEvent(f.Id, paid.Source, paid.GameplayHash, paid.Qualification.Current.NativeUseId, paid.Qualification.Current.Action.ActionId, target, paid.GiftBefore, paid.GiftAfter));
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainSuFei(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void FinishSuFei(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SuFei = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 23900)]
    internal bool ResumeSuFei(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SuFei: { } r } f || f.Id != id || r.Stage == SuFeiStage.ChoosingGift)
            return false;
        RequireSuFei(f);
        if (DrainSuFei(f))
            return true;
        if (r.Stage == SuFeiStage.GiftChildren || !CanGiveSuFei(f))
        {
            FinishSuFei(f);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SuFei = r with { Stage = SuFeiStage.ChoosingGift }, PendingMovementContinuation = null });
        PublishSuFeiChoice(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 23900)]
    internal bool ReturnSuFeiMovement(ProgramSkillFrame f) => f.SuFei is not null && ResumeSuFei(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 220)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 220)]
    internal bool CanContinueSuFei(ProgramSkillFrame f)
    {
        if (f.SuFei is null)
            return false;
        RequireSuFei(f);
        return true;
    }
    internal void RequireSuFei(ProgramSkillFrame f)
    {
        if (f.SuFei is not { } r || r.Source != SuFeiSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.WindowId != f.WindowContext?.ParentFrameId || !ValidSuFeiQualification(r.Qualification, f.OwnerSeat) || r.DrawBefore > r.DrawAfter || r.DrawAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualDraw is not (0 or 1) || r.DrawnId.HasValue != (r.ActualDraw == 1) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LianpianDrawPaidEvent>().Count(e => e == new LianpianDrawPaidEvent(f.Id, r.Source, r.GameplayHash, r.Qualification.Current.NativeUseId, r.Qualification.Current.Action.ActionId, r.DrawBefore, r.DrawAfter, r.ActualDraw)) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.DrawBefore && m.Sequence <= r.DrawAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == SuFeiDrawReason && (r.DrawnId is null || m.CardId == r.DrawnId)) != r.ActualDraw)
            throw new InvalidOperationException("Lianpian lacks its exact native use, source, window or once-paid draw invoice.");
        if (r.Stage == SuFeiStage.ChoosingGift)
        {
            if (f.PendingMovementContinuation is not null || r.GiftTarget is not null)
                throw new InvalidOperationException("Lianpian choice cannot borrow a movement payment.");
        }
        else if (f.PendingMovementContinuation is not { SubjectSeat: var actor, BeforeCount: 0, CoverageResultBind: null } || actor != f.OwnerSeat)
            throw new InvalidOperationException("Lianpian paid child lost its owning movement return.");
        if (r.Stage == SuFeiStage.GiftChildren && (r.GiftTarget is not { } target || !r.Qualification.CommonTargets.Contains(target) || r.DrawnId is not { } id || r.GiftAfter <= r.GiftBefore || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LianpianGiftPaidEvent>().Count(e => e == new LianpianGiftPaidEvent(f.Id, r.Source, r.GameplayHash, r.Qualification.Current.NativeUseId, r.Qualification.Current.Action.ActionId, target, r.GiftBefore, r.GiftAfter)) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.GiftBefore && m.Sequence <= r.GiftAfter && m.CardId == id && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(target) && m.Reason.Value == SuFeiGiftReason) != 1))
            throw new InvalidOperationException("Lianpian cannot repeat or substitute its actual paid gift.");
    }

    SkillProgramStepOutcome ISuFeiProgramHost.BeginSuFei(ProgramSkillFrame f, SkillProgramEffect e) => BeginSuFei(f, e);
}
