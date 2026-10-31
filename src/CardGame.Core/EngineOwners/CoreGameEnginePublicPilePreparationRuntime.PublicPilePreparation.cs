#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePublicPilePreparationRuntime
{
    internal static string PreparationReason(ProgramSkillFrame f, string step) => RuntimeSkills.RuntimeRules_Core_PublicPilePreparation.PreparationReason(f, step);
    internal long PreparationSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal bool PreparationGameEnded() => _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._status == EngineStatus.Completed && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Finished && _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winner != Winner.None && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameEndedEvent>().Count(e => e.Winner == _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winner && e.TeamId == (_coreNativeDamageHandlesOperations.Value.IsTeamMode ? _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId : null) && e.FactionId == (_coreNativeDamageHandlesOperations.Value.IsNationalWarMode ? _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId : null)) == 1;
    internal bool PreparationSourceAlive(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winner == Winner.None && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f);
    internal PublicPersistentPileSource? PreparationReferencedPile(ProgramSkillFrame f, SkillProgramEffect effect)
    {
        var reference = _corePublicPileEquipmentSequencesOperations.SelectDomainReference(f, effect.SkillIds.Single());
        return reference.Instance is { } instance ? _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((f.OwnerSeat, effect.SkillIds.Single(), instance)) : null;
    }

    internal bool ExactPreparationParent(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [var effect] || effect.Op != op)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1].Id != c.ParentFrameId)
            return false;
        if (op == SkillProgramEffectOp.RemovePublicPileAfterAttackDamage)
        {
            if (_resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame w || w.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || w.TargetSeat != f.OwnerSeat || c.TargetSeat != f.OwnerSeat || c.Window != w.TriggerWindow || c.Amount <= 0 || w.SourceCard is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash or CardKind.Duel) || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].ToProgramCandidate()))
                return false;
            if (_resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == w.ParentFrameId)is not { } applied || c.DamageFrameId != applied.Id || applied.SourceSeat != w.SourceSeat || applied.TargetSeat != f.OwnerSeat || applied.Amount != c.Amount)
                return false;
            // The original attack may be suspended by a paid movement observer. Resolve
            // its existing owning frame, never the global newest active attack/window.
            var original = _resolutionStackOperations._resolutionStack.FirstOrDefault(x => x.Id == applied.ParentFrameId) switch
            {
                CardUseFrame use => use.CardAttack,
                ProgramSkillFrame program => program.CardAttack,
                JudgmentFrame judgment => judgment.CardAttack,
                _ => null
            };
            return original is { DamageWasApplied: true } && original.EffectiveCardKind == w.SourceCard && original.SourceSeat == applied.SourceSeat && original.TargetSeat == applied.TargetSeat && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Count(e => e.ResolutionId == applied.Id && e.TargetSeat == applied.TargetSeat && e.SourceSeat == applied.SourceSeat && e.Amount == applied.Amount && e.Nature == applied.Nature) == 1;
        }

        if (f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || c.SourceSeat != f.OwnerSeat || c.TargetSeat != f.OwnerSeat)
            return false;
        if (op == SkillProgramEffectOp.StoreNonBasicOwnedPublicPile)
            return c.Window == SkillProgramTriggerWindow.TurnEnding && _resolutionStackOperations._resolutionStack[index - 1] is TurnEndingBoundaryFrame ending && ending.OwnerSeat == f.OwnerSeat && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
        return c.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame start && start.Window == c.Window && start.Continuation == ProgramLifecycleContinuation.NormalTurnStart && start.OwnerSeat == f.OwnerSeat && start.CandidateIndex >= 0 && start.CandidateIndex < start.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, start.Candidates[start.CandidateIndex]);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 190)]
    internal bool CanRunPublicPilePreparation(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [{ Op: SkillProgramEffectOp.RemovePublicPileAfterAttackDamage } e])
            return true;
        var reference = _corePublicPileSourcesOperations.ReferencedPublicPileSources(candidate.OwnerSeat, e.SkillIds.Single(), candidate.SkillInstanceId);
        return context.Amount > 0 && reference.Any(p => _corePublicPileSourcesOperations.PublicPileCards(p).Count > 0);
    }

    internal SkillProgramStepOutcome BeginPublicPilePreparation(SkillProgramEffect effect, ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.PublicPilePreparation is not null || !ExactPreparationParent(f, effect.Op))
            throw new InvalidOperationException("A preparation pile producer requires its exact real owner window.");
        if (!PreparationSourceAlive(f))
            return SkillProgramStepOutcome.Continue;
        var pile = effect.Op == SkillProgramEffectOp.StoreNonBasicOwnedPublicPile ? _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue) : PreparationReferencedPile(f, effect);
        if (effect.Op == SkillProgramEffectOp.RemovePublicPileAfterAttackDamage && (pile is null || _corePublicPileSourcesOperations.PublicPileCards(pile).Count == 0))
            return SkillProgramStepOutcome.Continue;
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        var issuer = new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicPilePreparation = new() { InstructionIndex = 1, Operation = effect.Op, Issuer = issuer, GameplayHash = f.GameplayHash, ActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, ParentId = f.WindowContext!.ParentFrameId, Stage = PublicPilePreparationStage.Choosing, Pile = pile } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PublicPilePreparationStartedEvent(f.Id, effect.Op, issuer, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, f.WindowContext.ParentFrameId, pile));
        PublishPublicPilePreparation(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IEnumerable<Card> PreparationStoreCards(ProgramSkillFrame f) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Where(c => !CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(c.Kind, SkillProgramCardCategory.Basic) && !c.IsGeneralWeapon && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c));
    internal bool PreparationPileCurrent(PublicPersistentPileSource p) => _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((p.OwnerSeat, p.SkillId, p.SkillInstanceId)) == p;
    internal IReadOnlyList<PromptChoice> PublicPilePreparationChoices(ProgramSkillFrame f)
    {
        var r = f.PublicPilePreparation!;
        var list = new List<PromptChoice>();
        void Add(string token, string text, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => list.Add(new(new($"pile-preparation.{(f.Id)}.{(token)}"), text, cards, targets, new Dictionary<string, string> { ["program-action"] = "public-pile-preparation", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["pile-action"] = token }));
        if (r.Operation == SkillProgramEffectOp.StoreNonBasicOwnedPublicPile)
        {
            foreach (var c in PreparationStoreCards(f).Where(c => !r.SelectedIds.Contains(c.Id)))
                Add("store-" + c.Id, "存入【" + CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(c) + "】", [c.Id], []);
            Add("finish", "完成存牌", [], []);
        }
        else if (r.Operation == SkillProgramEffectOp.RemovePublicPileAfterAttackDamage)
        {
            if (r.Pile is { } p && PreparationPileCurrent(p))
                foreach (var c in _corePublicPileSourcesOperations.PublicPileCards(p))
                    Add("remove-" + c.Id, "移去【" + CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(c) + "】", [c.Id], []);
        }
        else
        {
            Add("discard", "移去所有引兵牌，将手牌摸至体力上限", [], []);
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && p.Hp <= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp))
                Add("gift-" + target.Seat, "令 " + target.Name + " 获得所有引兵牌、回复体力并摸牌", [], [target.Seat]);
        }

        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishPublicPilePreparation(ProgramSkillFrame f)
    {
        if (PreparationGameEnded())
            return;
        var r = f.PublicPilePreparation!;
        var choices = PublicPilePreparationChoices(f);
        if (!PreparationSourceAlive(f) || choices.Count == 0)
        {
            FinishPublicPilePreparation(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Operation == SkillProgramEffectOp.StoreNonBasicOwnedPublicPile,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("public-pile-preparation")]
    internal void ResolvePublicPilePreparationChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A preparation choice lost its owning frame.");
        var r = f.PublicPilePreparation!;
        if (r.Stage != PublicPilePreparationStage.Choosing || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [PublicPilePreparationChoices(f).Single(c => c.Id == choice.Id)]))
            throw new InvalidOperationException("A preparation choice must name its exact current entity or beneficiary.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!PreparationSourceAlive(f))
        {
            FinishPublicPilePreparation(f, false);
            return;
        }

        var token = choice.Parameters["pile-action"];
        if (r.Operation == SkillProgramEffectOp.StoreNonBasicOwnedPublicPile && token.StartsWith("store-", StringComparison.Ordinal))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicPilePreparation = r with { SelectedIds = r.SelectedIds.Append(choice.Cards.Single()).ToArray() } });
            PublishPublicPilePreparation(f);
            return;
        }

        Card[] cards;
        CardLocation to;
        int branch = 0, beneficiary = f.OwnerSeat;
        if (r.Operation == SkillProgramEffectOp.StoreNonBasicOwnedPublicPile)
        {
            if (r.Pile is not { } own || !PreparationPileCurrent(own))
            {
                FinishPublicPilePreparation(f, false);
                return;
            }

            cards = r.SelectedIds.Select(id => PreparationStoreCards(f).Single(c => c.Id == id)).ToArray();
            to = own.Location;
        }
        else
        {
            cards = r.Pile is { } pile && PreparationPileCurrent(pile) ? _corePublicPileSourcesOperations.PublicPileCards(pile).ToArray() : [];
            if (r.Operation == SkillProgramEffectOp.RemovePublicPileAfterAttackDamage)
                cards = cards.Where(c => c.Id == choice.Cards.Single()).ToArray();
            else
            {
                branch = choice.Targets.Count == 0 ? 1 : 2;
                beneficiary = branch == 1 ? f.OwnerSeat : choice.Targets.Single();
            }

            to = branch == 2 ? CardLocation.Hand(beneficiary) : CardLocation.DiscardPile;
        }

        var ids = cards.Select(c => c.Id).ToArray();
        var from = ids.Select(_cardZonesOperations._cardZones.GetLocation).ToArray();
        var before = PreparationSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicPilePreparation = r with { Stage = PublicPilePreparationStage.PaymentChildren, Branch = branch, BeneficiarySeat = beneficiary, SelectedIds = [], PaidIds = ids, PaidFrom = from, FrozenCount = ids.Length, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (ids.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, to, new(PreparationReason(f, "payment")));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.PublicPilePreparation!with
        {
            After = PreparationSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicPilePreparation = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PublicPilePreparationPaidEvent(f.Id, branch, beneficiary, r.Pile, r.PaidIds, r.PaidFrom, before, r.After));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainPublicPilePreparation(ProgramSkillFrame f, bool movement) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 250)]
    internal bool ResumePublicPilePreparation(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.PublicPilePreparation is not { } r)
            return false;
        if (PreparationGameEnded())
            return true;
        if (r.Stage == PublicPilePreparationStage.Choosing)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishPublicPilePreparation(f);
            return true;
        }

        if (DrainPublicPilePreparation(f, f.PendingMovementContinuation is not null))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.PublicPilePreparation!;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Operation != SkillProgramEffectOp.ResolvePreparationPublicPile || !_coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat].IsAlive || r.Stage == PublicPilePreparationStage.DrawChildren)
        {
            FinishPublicPilePreparation(f, true);
            return true;
        }

        if (r.Stage == PublicPilePreparationStage.PaymentChildren && r.Branch == 2)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicPilePreparation = r with { Stage = PublicPilePreparationStage.RecoveryChildren, RecoveryIssued = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PublicPilePreparationRecoveryIssuedEvent(id, r.BeneficiarySeat, 1));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, r.BeneficiarySeat, 1, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        var requested = r.Branch == 2 ? r.FrozenCount : Math.Max(0, _coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat].MaxHp - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat]).Count);
        var before = PreparationSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicPilePreparation = r with { Stage = PublicPilePreparationStage.DrawChildren, DrawRequested = requested, DrawBefore = before, DrawAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat], requested, true, new(PreparationReason(f, "draw"))).Count;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.PublicPilePreparation!with
        {
            DrawActual = actual,
            DrawAfter = PreparationSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicPilePreparation = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PublicPilePreparationDrawIssuedEvent(id, r.BeneficiarySeat, requested, actual, before, r.DrawAfter));
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    internal void FinishPublicPilePreparation(ProgramSkillFrame f, bool completed)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PublicPilePreparation = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 270)]
    internal bool ReturnPublicPilePreparationMovement(ProgramSkillFrame f)
    {
        if (f.PublicPilePreparation is null || f.PendingMovementContinuation is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.StoreNonBasicOwnedPublicPile, SkillProgramEffectOp.RemovePublicPileAfterAttackDamage, SkillProgramEffectOp.ResolvePreparationPublicPile)]
    internal PromptChoice SelectAiPublicPilePreparation(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.PublicPilePreparation!;
        if (r.Operation == SkillProgramEffectOp.StoreNonBasicOwnedPublicPile)
            return p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault() ?? p.Choices.Single();
        if (r.Operation == SkillProgramEffectOp.RemovePublicPileAfterAttackDamage)
            return p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();
        var fill = p.Choices.Single(c => c.Targets.Count == 0);
        var gifts = p.Choices.Where(c => c.Targets.Count == 1).ToArray();
        var count = r.Pile is { } pile ? _corePublicPileSourcesOperations.PublicPileCards(pile).Count : 0;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat];
        var fillValue = brain.ScoreProgramTarget(view, f.OwnerSeat, new(0, 0, 0, Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count), 0, 0, false, false));
        var bestGift = gifts.Select(c => (Choice: c, Value: brain.ScoreProgramTarget(view, c.Targets.Single(), new SkillProgramAiHint(0, 0, 0, count > int.MaxValue / 2 ? int.MaxValue : count * 2, 1, 0, false, false)))).OrderByDescending(item => item.Value).ThenBy(item => item.Choice.Targets.Single()).FirstOrDefault();
        return bestGift.Choice is not null && bestGift.Value > fillValue ? bestGift.Choice : fill;
    }

    SkillProgramStepOutcome IPublicPilePreparationHost.ExecutePublicPilePreparation(SkillProgramEffect e, ProgramSkillFrame f) => BeginPublicPilePreparation(e, f);
}
