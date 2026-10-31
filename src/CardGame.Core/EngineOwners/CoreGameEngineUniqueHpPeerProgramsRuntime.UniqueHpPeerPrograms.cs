#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineUniqueHpPeerProgramsRuntime
{
    internal CardConversionSource RecipientContestSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal long RecipientContestSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal bool RecipientContestUnissuedSource(ProgramSkillFrame f) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    internal static string UniqueHpPeerCostReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_UniqueHpPeerPrograms.UniqueHpPeerCostReason(f);
    internal static string UniqueHpPeerDrawReason(ProgramSkillFrame f, int seat) => RuntimeSkills.RuntimeRules_Core_UniqueHpPeerPrograms.UniqueHpPeerDrawReason(f, seat);
    internal IEnumerable<(Card Card, CardLocation From)> UniqueHpPeerLegalCards(ProgramSkillFrame f, int actor) => new[]
    {
        CardLocation.Hand(actor),
        CardLocation.Equipment(actor)
    }.SelectMany(from => _cardZonesOperations._cardZones.CardsAt(from).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, c, from, OwnedCardMoveIntent.Discard) && !(actor == f.OwnerSeat && from.Zone == CardZoneKind.Equipment && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(actor, f.SkillId, f.SkillInstanceId, c))).Select(c => (c, from)));
    internal int UniqueHpPeerRequired(int seat) => Math.Min(2, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Count);
    internal bool UniqueHpPeerPayable(ProgramSkillFrame f, int seat) => UniqueHpPeerLegalCards(f, seat).Count() >= UniqueHpPeerRequired(seat);
    internal bool ExactUniqueHpPeerParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.OtherActualUseTargeted, ActualUseTarget: { } identity } context || !CoreNativeCardClassificationsRuntime.IsSlashCard(identity.EffectiveKind) || context.OwnerSeat != f.OwnerSeat || identity.TargetSeat != f.OwnerSeat || context.TargetSeat != identity.TargetSeat || context.SourceSeat != identity.ActorSeat || _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.ParentFrameId != identity.CardUseFrameId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates.Count != parent.Contexts.Count || !_coreActualUseTargetProgramsOperations.MatchesActualUseTarget(identity))
            return false;
        return CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) && parent.Contexts[parent.CandidateIndex].ActualUseTarget == identity && parent.Contexts[parent.CandidateIndex] == context;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 730)]
    internal bool CanRunUniqueHpPeer(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardDrawAndOfferUniqueHpPeer) || context.ActualUseTarget is { } identity && CoreNativeCardClassificationsRuntime.IsSlashCard(identity.EffectiveKind) && identity.TargetSeat == c.OwnerSeat && _coreActualUseTargetProgramsOperations.CanRunActualUseTarget(c, context) && UniqueHpPeerRequired(c.OwnerSeat) <= new[]
    {
        CardLocation.Hand(c.OwnerSeat),
        CardLocation.Equipment(c.OwnerSeat)
    }.Sum(from => _cardZonesOperations._cardZones.CardsAt(from).Count(x => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(c.OwnerSeat, x, from, OwnedCardMoveIntent.Discard) && !(from.Zone == CardZoneKind.Equipment && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(c.OwnerSeat, c.SkillId, c.SkillInstanceId, x))));
    internal SkillProgramStepOutcome BeginUniqueHpPeer(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.UniqueHpPeer is not null || f.InstructionIndex != 1 || !ExactUniqueHpPeerParent(f) || !RecipientContestUnissuedSource(f))
            throw new InvalidOperationException("The target discard/draw requires its exact real actual-target Slash candidate.");
        var context = f.WindowContext!;
        var identity = context.ActualUseTarget!;
        var r = new UniqueHpPeerReceipt(1, RecipientContestSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, context.ParentFrameId, identity.CardUseFrameId, identity.ActionId, identity, UniqueHpPeerStage.SelectingCost, f.OwnerSeat, UniqueHpPeerRequired(f.OwnerSeat), [], []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UniqueHpPeer = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UniqueHpPeerStartedEvent(f.Id, r.Source, r.GameplayHash, r.TurnNumber, r.TurnOwnerSeat, r.WindowFrameId, r.CardUseFrameId, r.CardActionId, r.TargetIdentity));
        StartUniqueHpPeerCost(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void StartUniqueHpPeerCost(ProgramSkillFrame f)
    {
        var r = f.UniqueHpPeer!;
        if (!RecipientContestUnissuedSource(f) || !_coreMatchParticipantsPlayersAccess._players[r.ChooserSeat].IsAlive || !UniqueHpPeerPayable(f, r.ChooserSeat))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "原来源或完整实际HE成本失效，未付后继取消。");
            return;
        }

        r = r with
        {
            Stage = UniqueHpPeerStage.SelectingCost,
            RequiredCount = UniqueHpPeerRequired(r.ChooserSeat),
            SelectedCardIds = [],
            SelectedLocations = []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UniqueHpPeer = r });
        if (r.RequiredCount == 0)
        {
            PayUniqueHpPeerCost(f);
            return;
        }

        PublishUniqueHpPeer(f);
    }

    internal IReadOnlyList<PromptChoice> UniqueHpPeerChoices(ProgramSkillFrame f)
    {
        var r = f.UniqueHpPeer!;
        var choices = new List<PromptChoice>();
        void Add(string branch, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets, CardLocation? from = null) => choices.Add(new(new($"unique-hp.{(f.Id)}.{(r.ChooserSeat)}.{(r.Stage)}.{(r.SelectedCardIds.Count)}.{(branch)}.{(cards.FirstOrDefault())}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "unique-hp-peer", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["source-zone"] = from?.Zone.ToString() ?? "" }));
        if (r.Stage == UniqueHpPeerStage.PeerOffer)
        {
            if (UniqueHpPeerPayable(f, r.ChooserSeat))
                Add("accept", "弃两张牌（不足全弃），然后摸两张牌", [], []);
            Add("decline", "不执行", [], []);
        }
        else if (r.Stage == UniqueHpPeerStage.SelectingCost)
            foreach (var item in UniqueHpPeerLegalCards(f, r.ChooserSeat).Where(x => !r.SelectedCardIds.Contains(x.Card.Id)))
                Add("card", $"选择弃置【{(item.Card.DisplayName)}】", [item.Card.Id], [r.ChooserSeat], item.From);
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishUniqueHpPeer(ProgramSkillFrame f)
    {
        var r = f.UniqueHpPeer!;
        var choices = UniqueHpPeerChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "实际HE成本不足，未支付。");
            return;
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.ChooserSeat, r.Stage == UniqueHpPeerStage.PeerOffer ? "你是结算后唯一最高体力者，可以如此做。" : $"选择 {(r.RequiredCount)} 张实际HE牌一起弃置。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [r.ChooserSeat], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.ChooserSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[r.ChooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("unique-hp-peer")]
    internal void ResolveUniqueHpPeerChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.UniqueHpPeer!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.ChooserSeat || !UniqueHpPeerChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets)))
            throw new InvalidOperationException("The private HE answer changed its exact original actor or entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!RecipientContestUnissuedSource(f) || !_coreMatchParticipantsPlayersAccess._players[r.ChooserSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "尚未支付的来源或参与者失效。");
            return;
        }

        var branch = choice.Parameters.GetValueOrDefault("branch");
        if (r.Stage == UniqueHpPeerStage.PeerOffer)
        {
            if (branch == "decline")
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return;
            }

            if (branch != "accept")
                throw new InvalidOperationException("Invalid unique-HP peer option.");
            StartUniqueHpPeerCost(f);
            return;
        }

        if (r.Stage != UniqueHpPeerStage.SelectingCost || branch != "card" || choice.Cards is not [var id] || choice.Targets is not [var seat] || seat != r.ChooserSeat || r.SelectedCardIds.Contains(id))
            throw new InvalidOperationException("Invalid exact HE selection.");
        var item = UniqueHpPeerLegalCards(f, seat).Single(x => x.Card.Id == id);
        r = r with
        {
            SelectedCardIds = r.SelectedCardIds.Append(id).ToArray(),
            SelectedLocations = r.SelectedLocations.Append(item.From).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UniqueHpPeer = r });
        if (r.SelectedCardIds.Count == r.RequiredCount)
            PayUniqueHpPeerCost(f);
        else
            PublishUniqueHpPeer(f);
    }

    internal void PayUniqueHpPeerCost(ProgramSkillFrame f)
    {
        var r = f.UniqueHpPeer!;
        if (r.SelectedCardIds.Count != r.RequiredCount || r.RequiredCount != UniqueHpPeerRequired(r.ChooserSeat) || r.SelectedCardIds.Where((id, i) => !UniqueHpPeerLegalCards(f, r.ChooserSeat).Any(x => x.Card.Id == id && x.From == r.SelectedLocations[i])).Any())
            throw new InvalidOperationException("The full frozen HE cost must still be legally payable once.");
        var invoice = new UniqueHpPeerInvoice(r.ChooserSeat, r.RequiredCount, r.SelectedCardIds, r.SelectedLocations, RecipientContestSequence, RecipientContestSequence);
        r = r with
        {
            Stage = UniqueHpPeerStage.CostChildren,
            SelectedCardIds = [],
            SelectedLocations = [],
            OwnerInvoice = r.ChooserSeat == f.OwnerSeat ? invoice : r.OwnerInvoice,
            PeerInvoice = r.ChooserSeat == f.OwnerSeat ? r.PeerInvoice : invoice
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UniqueHpPeer = r, PendingMovementContinuation = new(r.ChooserSeat, 0, null) });
        void Commit(long after)
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = current.UniqueHpPeer!;
            var paid = invoice with
            {
                CostAfter = after
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { UniqueHpPeer = receipt with { OwnerInvoice = r.ChooserSeat == f.OwnerSeat ? paid : receipt.OwnerInvoice, PeerInvoice = r.ChooserSeat == f.OwnerSeat ? receipt.PeerInvoice : paid } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UniqueHpPeerCostPaidEvent(f.Id, r.ChooserSeat, r.RequiredCount, paid.CostBefore, paid.CostAfter));
        }

        if (r.RequiredCount == 0)
            Commit(RecipientContestSequence);
        else
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.ChooserSeat, invoice.CardIds, new(UniqueHpPeerCostReason(f)), (_, records) => Commit(records.Max(m => m.Sequence)));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 370)]
    internal bool ResumeUniqueHpPeer(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.UniqueHpPeer is not { } r)
            return false;
        if (r.Stage is UniqueHpPeerStage.SelectingCost or UniqueHpPeerStage.PeerOffer)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishUniqueHpPeer(f);
            return true;
        }

        if (r.Stage is UniqueHpPeerStage.CostChildren or UniqueHpPeerStage.DrawChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (!RecipientContestUnissuedSource(f) || !_coreMatchParticipantsPlayersAccess._players[r.ChooserSeat].IsAlive)
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "保留真实已付款及孩子，取消尚未发行的摸牌或同做机会。");
                return true;
            }

            if (r.Stage == UniqueHpPeerStage.CostChildren)
            {
                var invoice = r.ChooserSeat == f.OwnerSeat ? r.OwnerInvoice! : r.PeerInvoice!;
                var before = RecipientContestSequence;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UniqueHpPeer = r with { Stage = UniqueHpPeerStage.DrawChildren }, PendingMovementContinuation = new(r.ChooserSeat, 0, null) });
                var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.ChooserSeat], 2, true, new(UniqueHpPeerDrawReason(f, r.ChooserSeat))).Count;
                invoice = invoice with
                {
                    DrawIssued = true,
                    DrawBefore = before,
                    DrawAfter = RecipientContestSequence,
                    ActualDrawCount = count
                };
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { UniqueHpPeer = current.UniqueHpPeer!with { OwnerInvoice = r.ChooserSeat == f.OwnerSeat ? invoice : r.OwnerInvoice, PeerInvoice = r.ChooserSeat == f.OwnerSeat ? r.PeerInvoice : invoice } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UniqueHpPeerDrawIssuedEvent(id, r.ChooserSeat, before, invoice.DrawAfter, count));
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return true;
            }

            if (r.ChooserSeat != f.OwnerSeat)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            var living = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToArray();
            var maximum = living.Max(p => p.Hp);
            var peers = living.Where(p => p.Hp == maximum).ToArray();
            var peer = peers.Length == 1 && peers[0].Seat != f.OwnerSeat ? peers[0] : null;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UniqueHpPeerOfferedEvent(id, peer?.Seat, peer?.Hp));
            if (peer is null)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { UniqueHpPeer = r with { Stage = UniqueHpPeerStage.PeerOffer, ChooserSeat = peer.Seat, RequiredCount = 0, PeerSeat = peer.Seat, PeerHp = peer.Hp } });
            PublishUniqueHpPeer(f);
            return true;
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.DiscardDrawAndOfferUniqueHpPeer, SkillProgramEffectOp.GiveAllHandAndStartRecipientPindian)]
    internal PromptChoice SelectAiRecipientContest(PendingDecision decision, ProgramSkillFrame f)
    {
        if (f.UniqueHpPeer is { } r)
        {
            if (r.Stage == UniqueHpPeerStage.PeerOffer)
            {
                var cost = UniqueHpPeerLegalCards(f, r.ChooserSeat).Select(x => CoreNativeAiDecisionsRuntime.GetKeepValue(x.Card, _coreMatchParticipantsPlayersAccess._players[r.ChooserSeat])).Order().Take(UniqueHpPeerRequired(r.ChooserSeat)).Sum();
                return decision.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == (cost < 24d && decision.Choices.Any(x => x.Parameters.GetValueOrDefault("branch") == "accept") ? "accept" : "decline"));
            }

            return decision.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[r.ChooserSeat])).ThenBy(c => c.Cards.Single()).First();
        }

        // A one-point damage to the selected target is what this effect promises;
        // score it through the shared target-loss hint shape.
        return decision.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), c.Targets.Single(), new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false))).ThenBy(c => c.Targets.Single()).First();
    }
}
