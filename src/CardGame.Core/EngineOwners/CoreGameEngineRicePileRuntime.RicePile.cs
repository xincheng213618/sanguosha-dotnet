#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRicePileRuntime
{
    internal const string RiceDrawReason = "skill-program.rice.draw";
    internal const string RiceStoreReason = "skill-program.rice.store";
    internal const string RiceGainReason = "skill-program.rice.obtain";
    internal bool RiceCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawAndStoreTwoPublicRice);

    internal CardConversionSource RiceIssuer(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool RiceProducer(PublicPersistentPileSource p) => p.Capacity == 2 && _contentRegistryOperations._contentRegistry.GetSkill(p.SkillId).Program?.Triggers.Any(t => t.Effects is [{ Op: SkillProgramEffectOp.DrawAndStoreTwoPublicRice }]) == true;
    internal IReadOnlyList<PublicPersistentPileSource> RiceSources(int owner, SkillProgramEffect effect, string instance) => _corePublicPileSourcesOperations.ReferencedPublicPileSources(owner, effect.SkillIds.Single(), instance).Where(p => RiceProducer(p) && _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.Grants.Any(g => g.SkillId == p.SkillId && g.SkillInstanceId == p.SkillInstanceId)).ToArray();
    internal IEnumerable<Card> RiceStorageCards(ProgramSkillFrame f) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Where(c => _corePublicPileCashOutOperations.PublicPileStorageCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c, _cardZonesOperations._cardZones.GetLocation(c.Id)));
    internal PublicPersistentPileSource? ExhaustedRicePile(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.MovementBatch is not { } batch)
            return null;
        var pile = _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((c.OwnerSeat, c.SkillId, c.SkillInstanceId));
        return pile is not null && RiceProducer(pile) && batch.SourceCounts.Any(n => n.Location == pile.Location && n.CountBefore > 0 && n.CountAfter == 0) && batch.Movements.Any(m => m.From == pile.Location && m.To != pile.Location) ? pile : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 280)]
    internal bool CanOfferRicePile(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [var e] || !RicePileContract.IsOperation(e.Op))
            return true;
        if (e.Op == SkillProgramEffectOp.DrawAndStoreTwoPublicRice)
            return c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && context.Window == SkillProgramTriggerWindow.TurnEnding && (_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((c.OwnerSeat, c.SkillId, c.SkillInstanceId))is not { } existing || _corePublicPileSourcesOperations.PublicPileCards(existing).Count == 0);
        if (e.Op == SkillProgramEffectOp.RecoverAfterLastPublicRice)
            return ExhaustedRicePile(c, context)is not null;
        if (e.Op == SkillProgramEffectOp.ReplaceJudgmentFromPublicRice)
            return context.JudgmentReplacement is not null;
        return context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, DamageFrameId: not null, Amount: > 0, TargetSeat: { } target } && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && context.OccurrenceIndex >= 0 && context.OccurrenceIndex < context.Amount && (t.Subject == SkillProgramTriggerSubject.Owner ? target == c.OwnerSeat : context.SourceSeat == c.OwnerSeat && target != c.OwnerSeat) && RiceSources(c.OwnerSeat, e, c.SkillInstanceId).Any(p => _corePublicPileSourcesOperations.PublicPileCards(p).Count > 0);
    }

    internal bool ExactRiceParent(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [var e] || e.Op != op)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1].Id != c.ParentFrameId)
            return false;
        if (op == SkillProgramEffectOp.DrawAndStoreTwoPublicRice)
            return c.Window == SkillProgramTriggerWindow.TurnEnding && f.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _resolutionStackOperations._resolutionStack[index - 1] is TurnEndingBoundaryFrame ending && ending.OwnerSeat == f.OwnerSeat && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
        if (op == SkillProgramEffectOp.OfferRiceAfterDamagePoint)
            return c.Window == SkillProgramTriggerWindow.AfterDamageApplied && c.Amount > 0 && c.OccurrenceIndex >= 0 && c.OccurrenceIndex < c.Amount && _resolutionStackOperations._resolutionStack[index - 1] is DamageTriggerWindowFrame w && w.TriggerWindow == c.Window && w.ParentFrameId == c.DamageFrameId && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate()) && w.TargetSeat == c.TargetSeat && (_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(w).IsSourceLess ? c.SourceSeat is null : w.SourceSeat == c.SourceSeat) && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == w.ParentFrameId)is { } damage && damage.Amount == c.Amount && damage.TargetSeat == c.TargetSeat && (_coreProgramLifecycleOperations.GetProgramTrigger(f).Subject == SkillProgramTriggerSubject.Owner ? c.TargetSeat == f.OwnerSeat : c.SourceSeat == f.OwnerSeat && c.TargetSeat != f.OwnerSeat);
        if (op == SkillProgramEffectOp.RecoverAfterLastPublicRice)
            return c.Window == SkillProgramTriggerWindow.CardsMoved && c.MovementBatch is { } batch && _resolutionStackOperations._resolutionStack[index - 1] is CardsMovedTriggerWindowFrame moved && moved.Batch.Id == batch.Id && moved.CandidateIndex >= 0 && moved.CandidateIndex < moved.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, moved.Candidates[moved.CandidateIndex]) && batch.SourceCounts.Any(n => n.Location == f.RicePile?.Pile.Location && n.CountBefore > 0 && n.CountAfter == 0);
        return c.Window == SkillProgramTriggerWindow.JudgmentReplacing && c.JudgmentReplacement is { } replacement && _resolutionStackOperations._resolutionStack[index - 1] is JudgmentFrame judged && judged.Id == replacement.JudgmentFrameId && judged.TargetSeat == replacement.SubjectSeat && _coreNativeJudgmentOperations.CurrentJudgmentCandidate(judged)is { } jc && jc.OwnerSeat == f.OwnerSeat && jc.ProgramId == f.SkillId && jc.ProgramTriggerId == f.TriggerId && jc.SkillInstanceId == f.SkillInstanceId && jc.GameplayHash == f.GameplayHash;
    }

    internal SkillProgramStepOutcome BeginRicePile(SkillProgramEffect effect, ProgramSkillFrame supplied)
    {
        if (effect.Op == SkillProgramEffectOp.ReplaceJudgmentFromPublicRice)
            return _coreRiceJudgmentOperations.BeginRiceJudgmentReplacement(supplied, effect);
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var context = f.WindowContext!;
        PublicPersistentPileSource? pile;
        if (effect.Op == SkillProgramEffectOp.DrawAndStoreTwoPublicRice)
            pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, 2);
        else if (effect.Op == SkillProgramEffectOp.RecoverAfterLastPublicRice)
            pile = ExhaustedRicePile(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context);
        else
            pile = RiceSources(f.OwnerSeat, effect, f.SkillInstanceId).FirstOrDefault(p => _corePublicPileSourcesOperations.PublicPileCards(p).Count > 0);
        if (pile is null)
            return SkillProgramStepOutcome.Continue;
        var r = new RicePileReceipt
        {
            Operation = effect.Op,
            Source = RiceIssuer(f),
            GameplayHash = f.GameplayHash,
            ParentId = context.ParentFrameId,
            DamageId = context.DamageFrameId,
            OccurrenceIndex = context.OccurrenceIndex,
            ActualTurn = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            Pile = pile,
            BeneficiarySeat = effect.Op == SkillProgramEffectOp.OfferRiceAfterDamagePoint ? context.TargetSeat!.Value : f.OwnerSeat,
            Stage = effect.Op == SkillProgramEffectOp.DrawAndStoreTwoPublicRice ? RicePileStage.DrawChildren : effect.Op == SkillProgramEffectOp.RecoverAfterLastPublicRice ? RicePileStage.RecoveryChildren : RicePileStage.StoreSelection
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = r });
        if (!ExactRiceParent(f, effect.Op))
            throw new InvalidOperationException("Rice requires the exact actual ending, damage point or exhausted source batch.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RicePileStartedEvent(f.Id, r));
        if (effect.Op == SkillProgramEffectOp.DrawAndStoreTwoPublicRice)
        {
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = r with { DrawBefore = before, DrawAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 2, true, new(RiceDrawReason)).Count;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { RicePile = f.RicePile!with { DrawActual = actual, DrawAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RicePileDrawIssuedEvent(f.Id, 2, actual, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
            if (!DrainRiceChildren(f))
                ReturnRiceMovement(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (effect.Op == SkillProgramEffectOp.RecoverAfterLastPublicRice)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = r with { RecoveryIssued = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RicePileRecoveryIssuedEvent(f.Id, context.MovementBatch!.Id, r.Source, pile));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishRiceChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> RiceChoices(ProgramSkillFrame f)
    {
        var r = f.RicePile!;
        var result = new List<PromptChoice>();
        void Add(string token, string label, IReadOnlyList<int> cards) => result.Add(new(new($"rice.{(f.Id)}.{(token)}"), label, cards, [], new Dictionary<string, string> { ["program-action"] = "rice-pile", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["rice-action"] = token }));
        if (r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice)
        {
            if (r.Selected.Count < r.StoreRequired)
                foreach (var card in RiceStorageCards(f).Where(c => !r.Selected.Contains(c.Id)))
                    Add("store-" + card.Id, "置为米：【" + CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(card) + "】", [card.Id]);
            if (r.Selected.Count == r.StoreRequired)
                Add("pay", $"确认将 {(r.Selected.Count)} 张牌置为米", []);
        }
        else
            foreach (var pile in RiceSources(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single(), f.SkillInstanceId))
                foreach (var card in _corePublicPileSourcesOperations.PublicPileCards(pile))
                    Add("obtain-" + card.Id, "获得米：【" + CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(card) + "】", [card.Id]);
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishRiceChoice(ProgramSkillFrame f)
    {
        var r = f.RicePile!;
        var choices = RiceChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice && (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Any(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId) || _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((r.Pile.OwnerSeat, r.Pile.SkillId, r.Pile.SkillInstanceId)) != r.Pile))
        {
            FinishRicePile(f, true);
            return;
        }

        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat].IsAlive)
        {
            FinishRicePile(f, true);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = r with { Choices = choices } });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.BeneficiarySeat, r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice ? $"请选择 {(r.StoreRequired)} 张牌置为米。" : "选择一张米获得。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice,
            TargetSeat = r.BeneficiarySeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("rice-pile")]
    internal void ResolveRiceChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Rice choice lost owner.");
        _coreRicePileReceiptsOperations.RequireRiceReceipt(f);
        var r = f.RicePile!;
        if (r.Stage != RicePileStage.StoreSelection || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.BeneficiarySeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], r.Choices.Where(c => c.Id == choice.Id).ToArray()) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], RiceChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Rice choice must retain its actual chooser and current exact physical entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var token = choice.Parameters["rice-action"];
        if (r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice && token.StartsWith("store-", StringComparison.Ordinal))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = r with { Selected = r.Selected.Append(choice.Cards.Single()).ToArray(), Choices = [] } });
            PublishRiceChoice(f);
            return;
        }

        var ids = r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice ? r.Selected.ToArray() : choice.Cards.ToArray();
        var pile = r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice ? r.Pile : RiceSources(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single(), f.SkillInstanceId).Single(p => _corePublicPileSourcesOperations.PublicPileCards(p).Any(c => c.Id == ids.Single()));
        if (r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice && (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Any(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId) || _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((pile.OwnerSeat, pile.SkillId, pile.SkillInstanceId)) != pile))
        {
            FinishRicePile(f, true);
            return;
        }

        var from = ids.Select(_cardZonesOperations._cardZones.GetLocation).ToArray();
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = r with { Stage = RicePileStage.MovementChildren, Pile = pile, CardIds = ids, From = from, Selected = [], Choices = [], Before = before, After = before }, PendingMovementContinuation = new(r.BeneficiarySeat, 0, null) });
        if (ids.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice ? pile.Location : CardLocation.Hand(r.BeneficiarySeat), new(r.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice ? RiceStoreReason : RiceGainReason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { RicePile = f.RicePile!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RicePilePaymentIssuedEvent(f.Id, f.RicePile!));
        if (!DrainRiceChildren(f))
            ReturnRiceMovement(f);
    }

    internal bool DrainRiceChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is not null ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is not null ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void FinishRicePile(ProgramSkillFrame f, bool completed)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RicePileCompletedEvent(f.Id, f.RicePile!.Operation, completed));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 20800)]
    internal bool ReturnRiceMovement(ProgramSkillFrame f)
    {
        if (f.RicePile is not { Stage: RicePileStage.DrawChildren or RicePileStage.MovementChildren } r || f.PendingMovementContinuation is null)
            return false;
        _coreRicePileReceiptsOperations.RequireRiceReceipt(f);
        if (DrainRiceChildren(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == RicePileStage.DrawChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Any(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId) && _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((r.Pile.OwnerSeat, r.Pile.SkillId, r.Pile.SkillInstanceId)) == r.Pile)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RicePile = r with { Stage = RicePileStage.StoreSelection, StoreRequired = Math.Min(2, RiceStorageCards(f).Count()) } });
            PublishRiceChoice(f);
        }
        else
            FinishRicePile(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 20800)]
    internal bool ResumeRicePile(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RicePile: { } r } f || f.Id != id)
            return false;
        _coreRicePileReceiptsOperations.RequireRiceReceipt(f);
        if (r.Stage == RicePileStage.StoreSelection)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishRiceChoice(f);
            return true;
        }

        if (f.PendingMovementContinuation is not null)
            return ReturnRiceMovement(f);
        if (r.Stage == RicePileStage.RecoveryChildren)
        {
            if (!DrainRiceChildren(f))
                FinishRicePile(f, true);
            return true;
        }

        throw new InvalidOperationException("Rice resumed without a typed draw, payment or recovery child.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.DrawAndStoreTwoPublicRice, SkillProgramEffectOp.OfferRiceAfterDamagePoint)]
    internal PromptChoice SelectAiRiceChoice(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.RicePile!.Operation == SkillProgramEffectOp.DrawAndStoreTwoPublicRice)
            return p.Choices.FirstOrDefault(c => c.Parameters["rice-action"] == "pay") ?? p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();
        return p.Choices.OrderByDescending(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.RicePile.BeneficiarySeat])).First();
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 410)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 410)]
    internal bool CanContinueRicePile(ProgramSkillFrame f)
    {
        if (f.RicePile is not { } r || r.Stage == RicePileStage.StoreSelection && r.Operation != SkillProgramEffectOp.DrawAndStoreTwoPublicRice)
            return false;
        _coreRicePileReceiptsOperations.RequireRiceReceipt(f);
        return true;
    }

    SkillProgramStepOutcome IRicePileProgramHost.ExecuteRicePile(SkillProgramEffect e, ProgramSkillFrame f) => BeginRicePile(e, f);
}
