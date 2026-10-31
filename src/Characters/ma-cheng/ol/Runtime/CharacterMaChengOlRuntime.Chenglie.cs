#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChengOlRuntime
{
    SkillProgramStepOutcome IMaChengHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginChenglie(f, op);
    internal SkillProgramStepOutcome BeginChenglie(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var outcome = op == SkillProgramEffectOp.ResolveChenglieRedHolderOutcome;
        if (f.Chenglie is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context)
            throw new InvalidOperationException("Chenglie requires its exact original native single instruction.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0);
        CardUseFrame use;
        if (outcome)
            use = ChenglieOutcomeUse(candidate, context) ?? throw new InvalidOperationException("Chenglie cannot borrow an unissued completed-use entitlement.");
        else if (!ChenglieInitialEligible(candidate, context) || !TryGetChenglieUse(context, out use, out _))
            throw new InvalidOperationException("Chenglie requires at least one genuinely additional legal Slash target.");
        var r = new ChenglieReceipt(ChenglieSource(f), f.GameplayHash, use.Id, use.Action!.ActionId, context.ParentFrameId, outcome ? use.ChenglieLease!.SetupFrameId : f.Id, outcome, outcome ? ChenglieStage.Outcomes : ChenglieStage.Targets)
        {
            Original = outcome ? use.ChenglieLease!.Original : use.TargetSeats,
            Added = outcome ? use.ChenglieLease!.Added : [],
            Paid = outcome
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieStartedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.WindowId, r.SetupFrameId, outcome, r.Original.Count));
        if (!outcome)
        {
            for (var i = 0; i < r.Original.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieTargetSlotEvent(f.Id, false, i, r.Original[i]));
            PublishChenglie(f);
        }
        else
            ContinueChenglie(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice ChenglieChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"chenglie.{(f.Id)}.{(branch)}.{(string.Join("-", cards ?? []))}.{(string.Join("-", targets ?? []))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { { "program-action", "chenglie" }, { "branch", branch } });
    internal IReadOnlyList<PromptChoice> ChenglieChoices(ProgramSkillFrame f)
    {
        var r = f.Chenglie!;
        var use = RequireChenglieParent(f);
        var result = new List<PromptChoice>();
        if (r.Stage == ChenglieStage.Targets)
        {
            if (r.Added.Count < 2)
                foreach (var seat in ChenglieLegalTargets(use, r.Added))
                    result.Add(ChenglieChoice(f, "target", $"额外指定{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}", targets: [seat]));
            result.Add(ChenglieChoice(f, r.Added.Count == 0 ? "cancel" : "targets-done", r.Added.Count == 0 ? "取消发动" : "完成额外目标"));
        }
        else if (r.Stage == ChenglieStage.Exchange)
        {
            foreach (var hand in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                foreach (var id in r.Pool.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing))
                    result.Add(ChenglieChoice(f, "exchange", $"以【{(hand.DisplayName)}】秘密交换【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", [hand.Id, id]));
        }
        else if (r.Stage == ChenglieStage.Assignment)
        {
            var targets = r.Original.Concat(r.Added).ToArray();
            if (r.Index < targets.Length)
                foreach (var id in r.Pool.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing))
                    result.Add(ChenglieChoice(f, "assign", $"将【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】暗置于{(_coreMatchParticipantsPlayersAccess._players[targets[r.Index]].Name)}", [id], [targets[r.Index]]));
        }
        else if (r.Stage == ChenglieStage.Gift && use.ChenglieLease is { } lease && r.Index < lease.Cards.Count && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            var holder = lease.Cards[r.Index].HolderSeat;
            if (_coreMatchParticipantsPlayersAccess._players[holder].IsAlive)
                foreach (var old in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, holder, holder, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (_, card) => !card.IsGeneralWeapon, destination: SkillProgramCardDestination.SelectedTargetHand, destinationSeat: f.OwnerSeat))
                    result.Add(old with { Id = new($"chenglie.{(f.Id)}.give.{(old.Id.Value)}"), Parameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string>(old.Parameters) { ["program-action"] = "chenglie", ["branch"] = "give" }) });
        }

        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishChenglie(ProgramSkillFrame f)
    {
        var r = f.Chenglie!;
        var choices = ChenglieChoices(f);
        if (choices.Count == 0)
        {
            ContinueChenglie(f.Id);
            return;
        }

        var holder = r.Stage == ChenglieStage.Gift ? RequireChenglieParent(f).ChenglieLease!.Cards[r.Index].HolderSeat : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, holder, r.Stage switch
        {
            ChenglieStage.Targets => "骋烈：额外指定一至两名合法目标。",
            ChenglieStage.Exchange => "骋烈：秘密交换一张手牌与一张展示牌。",
            ChenglieStage.Assignment => "骋烈：秘密分配原牌。",
            _ => "骋烈：交给来源一张手牌或装备牌。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage != ChenglieStage.Targets,
            Choices = choices,
            TargetSeat = holder,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[holder].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("chenglie")]
    internal void AnswerChenglie(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Chenglie: { } r } f || !ChenglieChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Chenglie requires its original chooser, exact current secret entity or native target.");
        var expected = r.Stage == ChenglieStage.Gift ? RequireChenglieParent(f).ChenglieLease!.Cards[r.Index].HolderSeat : f.OwnerSeat;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != expected)
            throw new InvalidOperationException("Only Chenglie's genuine current participant may answer.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "cancel")
        {
            FinishChenglie(f);
            return;
        }

        if (branch == "target")
        {
            var seat = choice.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieTargetSlotEvent(f.Id, true, r.Added.Count, seat));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Added = r.Added.Append(seat).ToArray() } });
            if (f.Chenglie!.Added.Count == 2 || ChenglieLegalTargets(RequireChenglieParent(f), f.Chenglie.Added).Count == 0)
                ApplyChenglieTargets(f);
            else
                PublishChenglie(f);
            return;
        }

        if (branch == "targets-done")
        {
            ApplyChenglieTargets(f);
            return;
        }

        if (branch == "exchange")
        {
            var hand = choice.Cards[0];
            var top = choice.Cards[1];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieSwapSelectedEvent(f.Id, hand, top));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { SwapHand = hand, SwapTop = top } });
            StartChenglieMovement(ref f, ChenglieStage.ExchangeChildren, f.OwnerSeat);
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(hand), CardLocation.Hand(f.OwnerSeat), CardLocation.Processing, new(ChenglieSwapOutReason), m => RecordChenglieMove(f.Id, m));
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(top), CardLocation.Processing, CardLocation.Hand(f.OwnerSeat), new(ChenglieSwapInReason), m => RecordChenglieMove(f.Id, m));
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { Chenglie = current.Chenglie!with { Pool = r.Pool.Select(id => id == top ? hand : id).ToArray() } });
            FinishChenglieMovement(f.Id);
            return;
        }

        if (branch == "assign")
        {
            PlaceChenglieAttachment(f, choice.Cards.Single());
            return;
        }

        if (branch == "give")
        {
            if (!Enum.TryParse<CardZoneKind>(choice.Parameters.GetValueOrDefault("source-zone"), out var zone) || !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), out var slot))
                throw new InvalidOperationException("Chenglie gift lost its original owned slot.");
            var from = new CardLocation(zone, expected);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { GiftId = card.Id, GiftFrom = from } });
            StartChenglieMovement(ref f, ChenglieStage.GiftChildren, expected);
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new(ChenglieGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, expected, f.OwnerSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, m => RecordChenglieMove(f.Id, m)));
            FinishChenglieMovement(f.Id);
            return;
        }

        throw new InvalidOperationException("Unknown Chenglie choice.");
    }

    internal void ApplyChenglieTargets(ProgramSkillFrame f)
    {
        var r = f.Chenglie!;
        var use = RequireChenglieParent(f);
        if (r.Added.Count is < 1 or > 2 || r.Added.Any(s => !ChenglieLegalTargets(use, []).Contains(s)))
            throw new InvalidOperationException("Chenglie requires one or two still-legal additional targets.");
        var targets = r.Original.Concat(r.Added).ToArray();
        var a = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
        var lease = new ChenglieLease(f.Id, use.Id, a.ActionId, r.Source, r.GameplayHash, a.ActorSeat, a.ProviderSeat)
        {
            Original = r.Original,
            Added = r.Added
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Paid = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieTargetsAppliedEvent(f.Id, use.Id, a.ActionId, r.Source, r.GameplayHash, a.ActorSeat, a.ProviderSeat, r.Original.Count, r.Added.Count));
        var rebase = RebaseChenglieSlashTargets(use, targets);
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(rebase with { TargetSeats = targets, Action = a, ChenglieLease = lease, TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, a);
        _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!, r.Added);
        StartChenglieMovement(ref f, ChenglieStage.RevealChildren, f.OwnerSeat);
        var cards = new List<Card>();
        for (var i = 0; i < targets.Length; i++)
        {
            if (_corePhysicalDrawFacadesOperations.DrawOneToProcessing(new(ChenglieRevealReason))is not { } card)
                break;
            cards.Add(card);
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(x => x.CardId == card.Id && x.To == CardLocation.Processing && x.Reason.Value == ChenglieRevealReason);
            RecordChenglieMove(f.Id, m);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieRevealedCardEvent(f.Id, i, card.Id, card.Kind, card.Suit, card.Rank));
        }

        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { Chenglie = active.Chenglie!with { Revealed = cards.Select(c => c.Id).ToArray(), Pool = cards.Select(c => c.Id).ToArray() } });
        if (cards.Count > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, "chenglie-top", Array.AsReadOnly(cards.Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray())));
        FinishChenglieMovement(f.Id);
    }

    internal CardUseFrame RebaseChenglieSlashTargets(CardUseFrame use, IReadOnlyList<int> targets)
    {
        if (use.CardAttack is not { } original)
            return use;
        CardAttackState Fresh(int seat) => new()
        {
            Active = original.Active,
            SourceSeat = use.SourceSeat,
            CardUserSeat = use.SourceSeat,
            TargetSeat = seat,
            DamageAmount = use.ProgramAdjustedSlashBaseDamage ?? use.Continuations.FangtianHalberd?.DamageAmount ?? original.DamageAmount,
            IgnoresArmor = use.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[seat], use.CardKind),
            EffectiveCardKind = use.CardKind,
            IsDelayedJudgmentDamage = original.IsDelayedJudgmentDamage,
            IsSourceLess = original.IsSourceLess,
            ProgramJudgmentFrameId = original.ProgramJudgmentFrameId,
            ProgramSkillFrameId = original.ProgramSkillFrameId,
            ProgramSkillCardUseFrameId = original.ProgramSkillCardUseFrameId,
            DelayedJudgmentSeat = original.DelayedJudgmentSeat,
            DamageNatureOverride = original.DamageNatureOverride,
            ConversionSource = original.ConversionSource,
            CardId = original.CardId,
            AppearanceKind = original.AppearanceKind,
            AppearanceSuit = original.AppearanceSuit,
            AppearanceRank = original.AppearanceRank,
            PhysicalCardIds = Array.AsReadOnly(original.PhysicalCardIds.ToArray()),
            AdjustedSlashReturn = original.AdjustedSlashReturn
        };
        IReadOnlyList<CardAttackState>? prepared = use.PreparedTargetAttacks is { } old ? Array.AsReadOnly(targets.Select(seat => old.FirstOrDefault(x => x.TargetSeat == seat) ?? Fresh(seat)).ToArray()) : null;
        return use with
        {
            TargetIndex = 0,
            PreparedTargetAttacks = prepared,
            CardAttack = prepared is { Count: > 0 } ? prepared[0] : original,
            Continuations = use.Continuations with
            {
                FangtianHalberd = use.Continuations.FangtianHalberd is { Active: true } multi ? multi with
                {
                    TargetSeats = Array.AsReadOnly(targets.ToArray()),
                    CurrentAttackOwnerId = use.Id
                }

                : use.Continuations.FangtianHalberd
            }
        };
    }

    internal void StartChenglieMovement(ref ProgramSkillFrame f, ChenglieStage stage, int participant)
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        var r = f.Chenglie!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Stage = stage, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = 0 }, PendingMovementContinuation = new(participant, 0, null) });
    }

    internal void RecordChenglieMove(long id, CardMovementRecord m)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.Chenglie!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieCardMovedEvent(id, r.Stage, r.Index, m.CardId, m.From, m.To, m.Reason, m.Sequence));
    }

    internal void FinishChenglieMovement(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.Chenglie!;
        var count = _coreProgramEventHistoryOperations.ProgramEventHistory<ChenglieCardMovedEvent>().Count(e => e.FrameId == id && e.Stage == r.Stage && e.Index == r.Index && e.Sequence > r.Before && e.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieMovementPaidEvent(id, r.Stage, r.Index, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
        ContinueChenglie(id);
    }

    internal void PlaceChenglieAttachment(ProgramSkillFrame f, int cardId)
    {
        var r = f.Chenglie!;
        var use = RequireChenglieParent(f);
        var lease = use.ChenglieLease!;
        var target = r.Original.Concat(r.Added).ElementAt(r.Index);
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId);
        var location = new CardLocation(CardZoneKind.PrivatePersistentPile, target, $"chenglie:{(use.Id)}:{(r.SetupFrameId)}:{(r.Index)}");
        _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
        StartChenglieMovement(ref f, ChenglieStage.AssignmentChildren, target);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, location, new(ChenglieAssignReason), m =>
        {
            RecordChenglieMove(f.Id, m);
            var attachment = new ChenglieAttachment(r.Index, target, card.Id, card.Kind, card.Suit, CoreNativeCardClassificationsRuntime.IsRedSuit(card.Suit), location, m.Sequence);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieAttachmentPlacedEvent(r.SetupFrameId, use.Id, r.ActionId, r.Index, target, card.Id, card.Kind, card.Suit, attachment.Red, location, m.Sequence));
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ChenglieLease = u.ChenglieLease!with { Cards = u.ChenglieLease.Cards.Append(attachment).ToArray() } });
        });
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.DiscardPile, new(ChenglieCleanupReason), m => RecordChenglieMove(f.Id, m));
        var now = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(now with { Chenglie = now.Chenglie!with { Pool = r.Pool.Where(id => id != cardId).ToArray() } });
        FinishChenglieMovement(f.Id);
    }

    internal bool DrainChenglie(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueChenglie(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Chenglie: { } r } f || f.Id != id)
            return;
        if (f.PendingMovementContinuation is not null)
        {
            if (DrainChenglie(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.Chenglie!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        }

        if (r.Stage == ChenglieStage.CleanupChildren)
        {
            SettleChenglieLease(f);
            FinishChenglie(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return;
        }

        if (!r.Outcome && !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.Paid)
        {
            BeginChenglieCleanup(f);
            return;
        }

        if (r.Stage == ChenglieStage.Targets)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishChenglie(f);
            return;
        }

        if (r.Stage == ChenglieStage.RevealChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Stage = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count > 0 && r.Pool.Count > 0 ? ChenglieStage.Exchange : ChenglieStage.Assignment, Index = 0 } });
            ContinueChenglie(id);
            return;
        }

        if (r.Stage == ChenglieStage.Exchange)
        {
            if (ChenglieChoices(f).Count > 0)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishChenglie(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Stage = ChenglieStage.Assignment } });
            ContinueChenglie(id);
            return;
        }

        if (r.Stage == ChenglieStage.ExchangeChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Stage = ChenglieStage.Assignment, Index = 0 } });
            ContinueChenglie(id);
            return;
        }

        if (r.Stage == ChenglieStage.AssignmentChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Stage = ChenglieStage.Assignment, Index = r.Index + 1 } });
            ContinueChenglie(id);
            return;
        }

        if (r.Stage == ChenglieStage.Assignment)
        {
            if (r.Pool.Count == 0 || r.Index >= r.Original.Count + r.Added.Count)
            {
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(r.UseId, u => u with { ChenglieLease = u.ChenglieLease!with { Ready = true } });
                var lease = _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)!.ChenglieLease!;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieSetupReadyEvent(r.SetupFrameId, r.UseId, r.ActionId, lease.Cards.Count));
                FinishChenglie(f);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[r.Original.Concat(r.Added).ElementAt(r.Index)].IsAlive)
            {
                PlaceChenglieAttachment(f, r.Pool[0]);
                return;
            }

            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishChenglie(f);
            return;
        }

        if (r.Stage is ChenglieStage.GiftChildren or ChenglieStage.RecoveryChildren)
        {
            CompleteChenglieOutcome(f);
            return;
        }

        if (r.Stage == ChenglieStage.Gift)
        {
            if (ChenglieChoices(f).Count > 0)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishChenglie(f);
                return;
            }

            CompleteChenglieOutcome(f);
            return;
        }

        if (r.Stage == ChenglieStage.Outcomes)
        {
            var lease = RequireChenglieParent(f).ChenglieLease!;
            if (r.Index >= lease.Cards.Count || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                BeginChenglieCleanup(f);
                return;
            }

            var card = lease.Cards[r.Index];
            var responded = ChenglieResponded(lease, card.HolderSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieOutcomeIssuedEvent(f.Id, r.SetupFrameId, r.Index, card.HolderSeat, card.Red, responded));
            if (!card.Red || !_coreMatchParticipantsPlayersAccess._players[card.HolderSeat].IsAlive)
            {
                CompleteChenglieOutcome(f);
                return;
            }

            if (responded)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Stage = ChenglieStage.Gift } });
                ContinueChenglie(id);
                return;
            }

            StartChenglieMovement(ref f, ChenglieStage.RecoveryChildren, card.HolderSeat);
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, card.HolderSeat, 1, null, null);
            FinishChenglieMovement(id);
            return;
        }

        throw new InvalidOperationException("Chenglie lost its typed continuation stage.");
    }

    internal void CompleteChenglieOutcome(ProgramSkillFrame f)
    {
        var r = f.Chenglie!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieOutcomeCompletedEvent(f.Id, r.SetupFrameId, r.Index));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = r with { Stage = ChenglieStage.Outcomes, Index = r.Index + 1, GiftId = null, GiftFrom = null } });
        ContinueChenglie(f.Id);
    }

    internal void BeginChenglieCleanup(ProgramSkillFrame f)
    {
        var r = f.Chenglie!;
        var use = RequireChenglieParent(f);
        StartChenglieMovement(ref f, ChenglieStage.CleanupChildren, f.OwnerSeat);
        foreach (var card in use.ChenglieLease!.Cards)
            if (_cardZonesOperations._cardZones.GetLocation(card.CardId) == card.Location)
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId), card.Location, CardLocation.DiscardPile, new(ChenglieCleanupReason), m => RecordChenglieMove(f.Id, m));
        foreach (var id in r.Pool)
            if (_cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing)
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.DiscardPile, new(ChenglieCleanupReason), m => RecordChenglieMove(f.Id, m));
        FinishChenglieMovement(f.Id);
    }

    internal void SettleChenglieLease(ProgramSkillFrame f)
    {
        var r = f.Chenglie!;
        var use = RequireChenglieParent(f);
        if (use.ChenglieLease!.Cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == c.Location) || r.Pool.Any(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing))
            throw new InvalidOperationException("Chenglie cannot complete while exact temporary entities remain.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ChenglieLease = u.ChenglieLease!with { Ready = true, Settled = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieLeaseSettledEvent(f.Id, r.SetupFrameId, r.UseId));
    }

    internal void FinishChenglie(ProgramSkillFrame f)
    {
        var r = f.Chenglie!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieCompletedEvent(f.Id, r.SetupFrameId, r.Outcome));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chenglie = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27560)]
    internal bool ResumeChenglie(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Chenglie: not null } f || f.Id != id)
            return false;
        ContinueChenglie(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27560)]
    internal bool ReturnChenglieMovement(ProgramSkillFrame f)
    {
        if (f.Chenglie is null)
            return false;
        ContinueChenglie(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27560)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27560)]
    internal bool ContinuePaidChenglie(ProgramSkillFrame f) => ShouldAllowChenglie(f);
    internal bool ShouldAllowChenglie(ProgramSkillFrame f)
    {
        if (f.Chenglie is { Paid: true })
        {
            return true;
        }

        if (f.Chenglie is not null || f.TriggerId != "red-holder-outcomes" || f.WindowContext is not { } c || f.InstructionIndex is not (0 or 1))
            return false;
        return IsIssuedChenglieCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), c);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveChenglieExtraTargetsAndHiddenCards, SkillProgramEffectOp.ResolveChenglieRedHolderOutcome)]
    internal PromptChoice ChooseChenglie(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.Chenglie!;
        if (r.Stage == ChenglieStage.Targets)
            return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "target") ?? p.Choices[0];
        if (r.Stage == ChenglieStage.Gift)
            return p.Choices.OrderBy(c => c.Cards.Count == 0 ? 0 : CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])).First();
        return p.Choices[0];
    }
}
