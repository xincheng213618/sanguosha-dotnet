#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrivateWhirlRuntime
{
    internal bool HasPrivateWhirlCapability => _coreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess._hasIssuedPrivateWhirl;

    internal static CardConversionSource WhirlSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_PrivateWhirl.WhirlSource(f);
    internal static CardLocation WhirlLocation(CardConversionSource source) => RuntimeSkills.RuntimeRules_Core_PrivateWhirl.WhirlLocation(source);
    internal PrivateWhirlDeposit[] LiveWhirl(int owner, string? skill = null, string? instance = null)
    {
        if (!HasPrivateWhirlCapability || !_cardZonesOperations._cardZones.HasPrivatePersistentCards(owner))
            return[];
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var removed = history.OfType<PrivateWhirlRemovedEvent>().Select(e => (e.Deposit.DepositId, e.Deposit.Source, e.Deposit.CardId)).ToHashSet();
        return history.OfType<PrivateWhirlDepositedEvent>().Select(e => e.Deposit).Where(d => d.Source.OwnerSeat == owner && (skill is null || d.Source.SkillId == skill) && (instance is null || d.Source.SkillInstanceId == instance) && !removed.Contains((d.DepositId, d.Source, d.CardId)) && _cardZonesOperations._cardZones.GetLocation(d.CardId) == d.Location).ToArray();
    }

    internal bool WhirlSourceCurrent(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 340)]
    internal bool CanOfferPrivateWhirl(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [var effect])
            return true;
        var deposits = LiveWhirl(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId);
        if (CoreGameEnginePersistentPrivateWhirlRuntime.IsPrivateWhirlDeposit(effect.Op))
            return LiveWhirl(candidate.OwnerSeat).Length < _corePersistentPrivateWhirlOperations.PrivateWhirlMaximum(candidate.SkillId) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0;
        if (effect.Op == SkillProgramEffectOp.ClearPrivateWhirlAtNextPlayEnding)
            return deposits.Any(d => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > d.CreatedPlayInstance);
        if (effect.Op != SkillProgramEffectOp.ConsumePrivateWhirlForActualUse)
            return true;
        return deposits.Length > 0 && context.CardUse is { } card && _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } action } && action.ActorSeat == candidate.OwnerSeat && action.ActionId == card.CardActionId && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrivateWhirlSegmentIssuedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId && e.Source.SkillInstanceId == candidate.SkillInstanceId && e.ActionId == action.ActionId);
    }

    internal SkillProgramStepOutcome BeginPrivateWhirl(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.PrivateWhirl is not null || f.WindowContext is not { } context || !WhirlSourceCurrent(f) || !CanOfferPrivateWhirl(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Whirl requires an unpaid current source and exact offered boundary.");
        _coreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess._hasIssuedPrivateWhirl = true;
        var r = new PrivateWhirlReceipt
        {
            InstructionIndex = f.InstructionIndex,
            Source = WhirlSource(f),
            GameplayHash = f.GameplayHash,
            WindowId = context.ParentFrameId,
            CreatedPlayInstance = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId
        };
        if (CoreGameEnginePersistentPrivateWhirlRuntime.IsPrivateWhirlDeposit(effect.Op))
            r = r with
            {
                Stage = PrivateWhirlStage.SelectDeposit
            };
        else if (effect.Op == SkillProgramEffectOp.ConsumePrivateWhirlForActualUse)
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(context.CardUse!.ParentCardUseFrameId)!;
            r = r with
            {
                Stage = PrivateWhirlStage.SelectRemove,
                OriginalAction = use.Action,
                UseId = use.Id,
                Deposits = LiveWhirl(f.OwnerSeat, f.SkillId, f.SkillInstanceId)
            };
        }
        else
            r = r with
            {
                Stage = PrivateWhirlStage.ExpiryChildren,
                Deposits = LiveWhirl(f.OwnerSeat, f.SkillId, f.SkillInstanceId).Where(d => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > d.CreatedPlayInstance).ToArray()
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = r });
        if (r.Stage == PrivateWhirlStage.ExpiryChildren)
        {
            PayWhirlRemoval(f, r.Deposits, "play-expired");
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishWhirlChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice WhirlChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"private-whirl.{(f.Id)}.{(f.PrivateWhirl!.Stage)}.{(branch)}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "private-whirl", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> WhirlChoices(ProgramSkillFrame f)
    {
        var r = f.PrivateWhirl!;
        var choices = new List<PromptChoice>();
        if (r.Stage == PrivateWhirlStage.SelectDeposit)
        {
            if (r.Selected.Count + LiveWhirl(f.OwnerSeat).Length < _corePersistentPrivateWhirlOperations.PrivateWhirlMaximum(f.SkillId))
                choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !r.Selected.Contains(c.Id)).Select(c => WhirlChoice(f, $"add-{(c.Id)}", $"暗置{(c.DisplayName)}（{(c.Suit)} {(c.RankText)}）", [c.Id])));
            choices.Add(WhirlChoice(f, "confirm", $"确认暗置{(r.Selected.Count)}张手牌", r.Selected));
        }
        else if (r.Stage == PrivateWhirlStage.SelectRemove)
            choices.AddRange(r.Deposits.Where(d => _cardZonesOperations._cardZones.GetLocation(d.CardId) == d.Location).Select(d => WhirlChoice(f, $"remove-{(d.CardId)}", $"移去旋中的{(_coreCardAttackStateOperations.GetAttackCard(d.CardId).DisplayName)}", [d.CardId])));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishWhirlChoice(ProgramSkillFrame f)
    {
        var choices = WhirlChoices(f);
        if (choices.Count == 0)
        {
            FinishWhirl(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "请选择暗置或移去的旋。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("private-whirl")]
    internal void ResolvePrivateWhirlChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Whirl lost its owning frame.");
        RequirePrivateWhirl(f);
        var r = f.PrivateWhirl!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], WhirlChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Whirl selection changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!WhirlSourceCurrent(f))
        {
            FinishWhirl(f);
            return;
        }

        if (r.Stage == PrivateWhirlStage.SelectRemove)
        {
            PayWhirlRemoval(f, [r.Deposits.Single(d => d.CardId == choice.Cards.Single())], "used");
            return;
        }

        if (choice.Parameters["branch"] != "confirm")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = r with { Selected = r.Selected.Append(choice.Cards.Single()).ToArray() } });
            PublishWhirlChoice(f);
            return;
        }

        if (r.Selected.Count == 0)
        {
            FinishWhirl(f);
            return;
        }

        var location = WhirlLocation(r.Source);
        _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = r with { Stage = PrivateWhirlStage.DepositChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var deposits = new List<PrivateWhirlDeposit>();
        foreach (var id in r.Selected)
        {
            var sequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _corePhysicalMovementFacadesOperations.MoveCard(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == id), CardLocation.Hand(f.OwnerSeat), location, new("skill-program.private-whirl.deposit"));
            var deposit = new PrivateWhirlDeposit(f.Id, r.Source, r.GameplayHash, id, location, r.CreatedPlayInstance, sequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
            deposits.Add(deposit);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateWhirlDepositedEvent(deposit));
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = f.PrivateWhirl!with { Deposits = deposits, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        IssueWhirlSegment(f, r.Selected.Count);
        ContinueWhirl(f);
    }

    internal void PayWhirlRemoval(ProgramSkillFrame f, IReadOnlyList<PrivateWhirlDeposit> deposits, string reason)
    {
        var r = f.PrivateWhirl!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = r with { Deposits = deposits, Stage = reason == "used" ? PrivateWhirlStage.RemovalChildren : PrivateWhirlStage.ExpiryChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        foreach (var d in deposits)
        {
            var sequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(d.CardId), d.Location, CardLocation.DiscardPile, new($"skill-program.private-whirl.{(reason)}"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateWhirlRemovedEvent(f.Id, d, r.OriginalAction?.ActionId, sequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, reason));
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = f.PrivateWhirl!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        IssueWhirlSegment(f, deposits.Count);
        ContinueWhirl(f);
    }

    internal void IssueWhirlSegment(ProgramSkillFrame f, int count)
    {
        var r = f.PrivateWhirl!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateWhirlSegmentIssuedEvent(f.Id, r.Source, r.Stage, r.OriginalAction?.ActionId, r.Before, r.After, count, r.RemainingCount, r.NotUniqueMaximum));
    }

    internal bool DrainWhirl(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void ContinueWhirl(ProgramSkillFrame f)
    {
        if (DrainWhirl(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = f.PrivateWhirl!;
        if (r.Stage == PrivateWhirlStage.RemovalChildren)
        {
            DrawWhirlSegment(f, 1, PrivateWhirlStage.FirstDrawChildren);
            return;
        }

        if (r.Stage == PrivateWhirlStage.FirstDrawChildren)
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
            var notUnique = _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count >= hand);
            var x = LiveWhirl(f.OwnerSeat, f.SkillId, f.SkillInstanceId).Length;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = r with { RemainingCount = x, NotUniqueMaximum = notUnique } });
            if (notUnique && x > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                DrawWhirlSegment(f, x, PrivateWhirlStage.ExtraDrawChildren);
                return;
            }
        }

        FinishWhirl(f);
    }

    internal void DrawWhirlSegment(ProgramSkillFrame f, int amount, PrivateWhirlStage stage)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = f.PrivateWhirl!with { Stage = stage, Before = before, After = before, DrawCount = 0 } });
        var drawn = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], amount, true, new("skill-program.private-whirl.draw")).Count : 0;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = f.PrivateWhirl!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawCount = drawn } });
        IssueWhirlSegment(f, drawn);
        ContinueWhirl(f);
    }

    internal void FinishWhirl(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateWhirl = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 22100)]
    internal bool ResumePrivateWhirl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PrivateWhirl: { } r } f || f.Id != id || r.Stage is PrivateWhirlStage.SelectDeposit or PrivateWhirlStage.SelectRemove)
            return false;
        RequirePrivateWhirl(f);
        ContinueWhirl(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 22100)]
    internal bool ReturnPrivateWhirlMovement(ProgramSkillFrame f) => f.PrivateWhirl is not null && ResumePrivateWhirl(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 60)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 60)]
    internal bool CanContinuePrivateWhirl(ProgramSkillFrame f)
    {
        if (f.PrivateWhirl is not { } r || r.Stage is PrivateWhirlStage.SelectDeposit or PrivateWhirlStage.SelectRemove)
            return false;
        RequirePrivateWhirl(f);
        return true;
    }
    internal void RequirePrivateWhirl(ProgramSkillFrame f)
    {
        if (f.PrivateWhirl is not { } r || r.Source != WhirlSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.WindowId != f.WindowContext?.ParentFrameId || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Count > _corePersistentPrivateWhirlOperations.PrivateWhirlMaximum(f.SkillId) || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.DrawCount < 0 || r.DrawCount > _corePersistentPrivateWhirlOperations.PrivateWhirlMaximum(f.SkillId))
            throw new InvalidOperationException("Whirl lost its immutable owning source, instruction, selection or paid movement.");
        if (r.OriginalAction is { } original && (original.Type != CardActionType.Use || original.ActorSeat != f.OwnerSeat || original.ActionId != f.WindowContext?.CardUse?.CardActionId || r.UseId != f.WindowContext?.CardUse?.ParentCardUseFrameId || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId!.Value)?.Action is not { } actual || !CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(original, actual)))
            throw new InvalidOperationException("Whirl lost its exact committed actual use.");
        if (r.Stage is PrivateWhirlStage.SelectDeposit or PrivateWhirlStage.SelectRemove)
            return;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrivateWhirlSegmentIssuedEvent>().Where(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.Stage == r.Stage && e.Before == r.Before && e.After == r.After).ToArray();
        if (facts is not [var fact] || fact.ActionId != r.OriginalAction?.ActionId || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat)
            throw new InvalidOperationException("Whirl cannot resume without its exact once-issued segment.");
        if (r.Stage is PrivateWhirlStage.FirstDrawChildren or PrivateWhirlStage.ExtraDrawChildren)
        {
            if (fact.Count != r.DrawCount || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.private-whirl.draw") != r.DrawCount)
                throw new InvalidOperationException("Whirl draw lost its once-paid native movement.");
        }
        else if (fact.Count != r.Deposits.Count || r.Deposits.Any(d => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == d.CardId && m.Sequence > r.Before && m.Sequence <= r.After && m.From == (r.Stage == PrivateWhirlStage.DepositChildren ? CardLocation.Hand(f.OwnerSeat) : d.Location) && m.To == (r.Stage == PrivateWhirlStage.DepositChildren ? d.Location : CardLocation.DiscardPile)) != 1))
            throw new InvalidOperationException("Whirl deposit/removal lost its physical invoice.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 80)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 90)]
    internal void CleanupLostPrivateWhirlPiles()
    {
        if (!HasPrivateWhirlCapability || !_cardZonesOperations._cardZones.HasPrivatePersistentCards())
            return;
        foreach (var d in _coreMatchParticipantsPlayersAccess._players.SelectMany(p => LiveWhirl(p.Seat)).Where(d => !_coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat].SkillGrants.Grants.Any(g => g.SkillId == d.Source.SkillId && g.SkillInstanceId == d.Source.SkillInstanceId)).ToArray())
        {
            if (_cardZonesOperations._cardZones.GetLocation(d.CardId) != d.Location)
                continue;
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(d.CardId), d.Location, CardLocation.DiscardPile, new("skill-program.private-whirl.source-lost"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateWhirlRemovedEvent(0, d, null, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, "source-lost"));
        }
    }

    internal IReadOnlyList<PrivateWhirlPileSnapshot>? ProjectPrivateWhirlPiles(int owner, int viewer, bool revealAll)
    {
        var deposits = LiveWhirl(owner);
        if (deposits.Length == 0)
            return null;
        return Array.AsReadOnly(deposits.GroupBy(d => (d.Source.SkillId, d.Source.SkillInstanceId)).Select(g => new PrivateWhirlPileSnapshot { SkillId = g.Key.SkillId, Count = g.Count(), Cards = owner == viewer || revealAll ? g.Select(d => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreCardAttackStateOperations.GetAttackCard(d.CardId))).ToArray() : null }).ToArray());
    }

    SkillProgramStepOutcome IPrivateWhirlHost.BeginPrivateWhirl(ProgramSkillFrame f, SkillProgramEffect effect) => BeginPrivateWhirl(f, effect);
}
