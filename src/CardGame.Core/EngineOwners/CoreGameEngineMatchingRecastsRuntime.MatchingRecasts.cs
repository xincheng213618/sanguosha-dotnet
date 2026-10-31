#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineMatchingRecastsRuntime
{
    internal long MatchingRecastSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal MatchingRecastMaterial[] MatchingRecastMaterials(int seat) => new[]
    {
        CardZoneKind.Hand,
        CardZoneKind.Equipment
    }.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, seat)).Select(c => new MatchingRecastMaterial(c.Id, c.Kind, new(zone, seat), c.IsGeneralWeapon))).ToArray();
    internal static bool MatchingRecastGroup(IReadOnlyList<MatchingRecastMaterial> materials) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_MatchingRecasts.MatchingRecastGroup(materials);
    internal bool ValidMatchingRecastSelection(int seat, IReadOnlyList<int> ids)
    {
        var available = MatchingRecastMaterials(seat);
        return ids.All(id => available.Any(m => m.CardId == id)) && MatchingRecastGroup(ids.Select(id => available.Single(m => m.CardId == id)).ToArray());
    }

    internal bool MatchingRecastMaterialsStillOwned(MatchingRecastParticipantReceipt p) => p.Materials.All(m => m.From.OwnerSeat == p.Seat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _cardZonesOperations._cardZones.GetLocation(m.CardId) == m.From && _cardZonesOperations._cardZones.CardsAt(m.From).Any(c => c.Id == m.CardId && c.Kind == m.PrintedKind && c.IsGeneralWeapon == m.IsGeneralWeapon));
    internal static bool MatchingRecastPeerCursor(ProgramMatchingRecastReceipt r) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_MatchingRecasts.MatchingRecastPeerCursor(r);
    internal static MatchingRecastParticipantReceipt CurrentMatchingRecast(ProgramMatchingRecastReceipt r) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_MatchingRecasts.CurrentMatchingRecast(r);
    internal static ProgramMatchingRecastReceipt WithCurrentMatchingRecast(ProgramMatchingRecastReceipt r, MatchingRecastParticipantReceipt p) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_MatchingRecasts.WithCurrentMatchingRecast(r, p);
    internal SkillProgramStepOutcome BeginMatchingRecast(ProgramSkillFrame supplied, int peerSeat, string ledgerId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (plan.Activation is not { } activation || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.MatchingRecast is not null || f.SelectedTargetSeats is not [var target] || target != peerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId <= 0 || _resolutionStackOperations._resolutionStack.Count != 1 || !ValidMatchingRecastSelection(f.OwnerSeat, f.SelectedCardIds) || activation.TargetPhaseLedgerId != ledgerId || !_coreProgramParticipantPhaseOperationsOperations.CanActivateTargetPhaseLedger(f.OwnerSeat, f.SkillId, ledgerId, peerSeat))
            throw new InvalidOperationException("Matching recast requires its original HE group, living target and unused actual play-phase target debit.");
        MatchingRecastComposition.ValidateActivation(f.SkillId, activation);
        var available = MatchingRecastMaterials(f.OwnerSeat);
        var materials = f.SelectedCardIds.Select(id => available.Single(m => m.CardId == id)).ToArray();
        _coreProgramParticipantPhaseOperationsOperations.ConsumeProgramTargetPhaseLedger(f, ledgerId);
        var r = new ProgramMatchingRecastReceipt
        {
            InstructionIndex = f.InstructionIndex,
            Source = new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            UsageGroup = activation.UsageGroup,
            TargetLedgerId = ledgerId,
            ActualTurnNumber = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat,
            PhaseInstanceId = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId,
            Category = global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(materials[0].PrintedKind),
            Stage = MatchingRecastStage.OwnerReady,
            Owner = new(f.OwnerSeat)
            {
                Materials = materials
            },
            Peer = new(peerSeat)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MatchingRecast = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MatchingRecastStartedEvent(f.Id, r.Source, r.GameplayHash, r.UsageGroup, ledgerId, peerSeat, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.PhaseInstanceId));
        IssueMatchingRecastCost(f, false);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void IssueMatchingRecastCost(ProgramSkillFrame f, bool peer)
    {
        var r = f.MatchingRecast!;
        var p = peer ? r.Peer : r.Owner;
        if (p.CostIssued || p.DrawIssued || p.Materials.Count != r.Owner.Materials.Count || !MatchingRecastMaterialsStillOwned(p) || (peer ? r.Stage != MatchingRecastStage.ChoosingPeerCards : r.Stage != MatchingRecastStage.OwnerReady))
            throw new InvalidOperationException("Matching recast can pay only its exact frozen original materials once.");
        var before = MatchingRecastSequence;
        f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, p.Seat, p.Materials.Select(m => m.CardId).ToArray(), peer ? 1 : 0);
        r = r with
        {
            Stage = peer ? MatchingRecastStage.PeerCostChildren : MatchingRecastStage.OwnerCostChildren
        };
        r = WithCurrentMatchingRecast(r, p with { CostIssued = true, CostBefore = before, CostAfter = before });
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MatchingRecast = r, PendingMovementContinuation = new(p.Seat, 0, null) });
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        // Recast bypasses discard eligibility. All physical costs commit before equipment removal hooks run.
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(p.Materials.Select(m => m.CardId).ToArray(), CardLocation.DiscardPile, CardMoveReasons.RecastDiscard, (batch, records) =>
        {
            if (records.Count != p.Materials.Count || !records.Zip(p.Materials).All(pair => pair.First.CardId == pair.Second.CardId && pair.First.CardKind == pair.Second.PrintedKind && pair.First.From == pair.Second.From && pair.First.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(pair.Second.CardId), pair.Second.From, pair.Second.Destination) && pair.First.Reason == CardMoveReasons.RecastDiscard))
                throw new InvalidOperationException("Matching recast changed its paid original physical HE group.");
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = current.MatchingRecast!;
            var after = records.Max(m => m.Sequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MatchingRecast = WithCurrentMatchingRecast(receipt, CurrentMatchingRecast(receipt)with { CostAfter = after, CostBatchId = batch }) });
            foreach (var m in p.Materials)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MatchingRecastPaidEvent(f.Id, peer ? 1 : 0, p.Seat, m.CardId, m.PrintedKind, m.From, batch, before, after));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void IssueMatchingRecastDraw(ProgramSkillFrame f)
    {
        var r = f.MatchingRecast!;
        var p = CurrentMatchingRecast(r);
        var peer = MatchingRecastPeerCursor(r);
        if (r.Stage is not (MatchingRecastStage.OwnerCostChildren or MatchingRecastStage.PeerCostChildren) || !p.CostIssued || p.DrawIssued || f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("A matching recast draw must follow its drained real payment exactly once.");
        var before = MatchingRecastSequence;
        r = r with
        {
            Stage = peer ? MatchingRecastStage.PeerDrawChildren : MatchingRecastStage.OwnerDrawChildren
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MatchingRecast = WithCurrentMatchingRecast(r, p with { DrawIssued = true, DrawBefore = before, DrawAfter = before }), PendingMovementContinuation = new(p.Seat, 0, null) });
        var actual = _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[p.Seat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[p.Seat], p.Materials.Count, true, CardMoveReasons.RecastDraw).Count : 0;
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = current.MatchingRecast!;
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { MatchingRecast = WithCurrentMatchingRecast(r, CurrentMatchingRecast(r)with { ActualDrawCount = actual, DrawAfter = MatchingRecastSequence }) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MatchingRecastDrawIssuedEvent(f.Id, peer ? 1 : 0, p.Seat, actual, before, MatchingRecastSequence));
        for (var i = 0; i < p.Materials.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(p.Seat, p.Materials[i].CardId, p.Materials[i].PrintedKind, i < actual ? 1 : 0));
        _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), actual);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal PromptChoice MatchingRecastChoice(ProgramSkillFrame f, string step, string label, int? card = null) => new(new($"matching-recast.{(f.Id)}.{(step)}.{(card ?? -1)}"), label, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "matching-recast", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["step"] = step });
    internal IReadOnlyList<PromptChoice> MatchingRecastChoices(ProgramSkillFrame f)
    {
        var r = f.MatchingRecast!;
        if (r.Stage == MatchingRecastStage.ChoosingPeerCards)
            return Array.AsReadOnly(r.EligiblePeerMaterials.Where(m => r.Peer.Materials.All(p => p.CardId != m.CardId)).Select(m => MatchingRecastChoice(f, "card", $"重铸【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】", m.CardId)).ToArray());
        if (r.Stage != MatchingRecastStage.ChoosingPeer)
            return[];
        var choices = new List<PromptChoice>();
        if (r.EligiblePeerMaterials.Count >= r.Owner.Materials.Count)
            choices.Add(MatchingRecastChoice(f, "recast", $"重铸{(r.Owner.Materials.Count)}张同类型牌"));
        choices.Add(MatchingRecastChoice(f, "damage", "受到发动者的1点雷电伤害"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMatchingRecastChoice(ProgramSkillFrame f) => _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.MatchingRecast!.Peer.Seat, MatchingRecastChoices(f), f.MatchingRecast.Stage == MatchingRecastStage.ChoosingPeer ? "选择重铸等量同类型牌，或受到1点雷电伤害。" : "选择本次重铸的牌，选齐后同时重铸。", f.MatchingRecast.Peer.Seat);
    [ProgramAnswer("matching-recast")]
    internal void ResolveMatchingRecastChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Matching recast lost its choice owner.");
        _coreMatchingRecastObserversOperations.AssertMatchingRecast(f);
        var r = f.MatchingRecast!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || !_coreMatchingRecastObserversOperations.IsMatchingRecastChoice(f, prompt) || !MatchingRecastChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime.SameNameHandChoicesEqual(c, choice)))
            throw new InvalidOperationException("Matching recast changed its exact participant-only private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.Peer.Seat].IsAlive)
        {
            FinishMatchingRecast(f);
            return;
        }

        if (r.Stage == MatchingRecastStage.ChoosingPeer)
        {
            if (choice.Parameters["step"] == "damage")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MatchingRecast = r with { Stage = MatchingRecastStage.DamageIssued, DamageIssued = true } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MatchingRecastDamageIssuedEvent(f.Id, f.OwnerSeat, r.Peer.Seat, 1, DamageNature.Thunder));
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Peer.Seat, 1, nature: DamageNature.Thunder);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MatchingRecast = r with { Stage = MatchingRecastStage.ChoosingPeerCards } });
            PublishMatchingRecastChoice(f);
            return;
        }

        if (r.Stage != MatchingRecastStage.ChoosingPeerCards || choice.Parameters["step"] != "card")
            throw new InvalidOperationException("Matching recast lost its frozen peer payment stage.");
        var material = r.EligiblePeerMaterials.Single(m => m.CardId == choice.Cards.Single());
        var peer = r.Peer with
        {
            Materials = r.Peer.Materials.Append(material).ToArray()
        };
        if (!MatchingRecastMaterialsStillOwned(peer))
            throw new InvalidOperationException("A peer recast must pay the selected still-owned original HE entities.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MatchingRecast = r with { Peer = peer } });
        if (peer.Materials.Count == r.Owner.Materials.Count)
            IssueMatchingRecastCost(f, true);
        else
            PublishMatchingRecastChoice(f);
    }

    internal bool DrainMatchingRecastChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 60)]
    internal bool ResumeMatchingRecast(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MatchingRecast: { } r } f || f.Id != id)
            return false;
        _coreMatchingRecastObserversOperations.AssertMatchingRecast(f);
        if (r.Stage is MatchingRecastStage.ChoosingPeer or MatchingRecastStage.ChoosingPeerCards)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishMatchingRecastChoice(f);
            return true;
        }

        if (r.Stage is MatchingRecastStage.OwnerCostChildren or MatchingRecastStage.OwnerDrawChildren or MatchingRecastStage.PeerCostChildren or MatchingRecastStage.PeerDrawChildren)
        {
            if (DrainMatchingRecastChildren(f))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage is MatchingRecastStage.OwnerCostChildren or MatchingRecastStage.PeerCostChildren)
            {
                IssueMatchingRecastDraw(f);
                return true;
            }

            if (r.Stage == MatchingRecastStage.OwnerDrawChildren && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[r.Peer.Seat].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MatchingRecast = r with { Stage = MatchingRecastStage.ChoosingPeer, EligiblePeerMaterials = MatchingRecastMaterials(r.Peer.Seat).Where(m => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(m.PrintedKind) == r.Category).ToArray() } });
                PublishMatchingRecastChoice(f);
                return true;
            }
        }

        if (r.Stage == MatchingRecastStage.DamageIssued && f.AttackAttempt is not null)
            return false;
        FinishMatchingRecast(f);
        return true;
    }

    internal void FinishMatchingRecast(ProgramSkillFrame f)
    {
        var r = f.MatchingRecast!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MatchingRecast = r with { Stage = MatchingRecastStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MatchingRecastCompletedEvent(f.Id, r.Owner.CostIssued, r.Peer.CostIssued, r.DamageIssued, r.Owner.ActualDrawCount, r.Peer.ActualDrawCount));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40)]
    internal bool ReturnMatchingRecastMovement(ProgramSkillFrame f)
    {
        if (f.MatchingRecast is not { Stage: MatchingRecastStage.OwnerCostChildren or MatchingRecastStage.OwnerDrawChildren or MatchingRecastStage.PeerCostChildren or MatchingRecastStage.PeerDrawChildren })
            return false;
        _coreMatchingRecastObserversOperations.AssertMatchingRecast(f);
        if (f.PendingMovementContinuation is null)
            throw new InvalidOperationException("A matching recast child lost its exact movement return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 220)]
    internal bool IsMatchingRecastMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.RecastMatchingCardsThenPeerChoice && f.MatchingRecast is { Stage: MatchingRecastStage.OwnerCostChildren or MatchingRecastStage.OwnerDrawChildren or MatchingRecastStage.PeerCostChildren or MatchingRecastStage.PeerDrawChildren } r && f.PendingMovementContinuation == pending && pending.SubjectSeat == CurrentMatchingRecast(r).Seat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && _coreMatchingRecastObserversOperations.ValidMatchingRecastReceipt(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 610)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 630)]
    internal bool CanContinueMatchingRecast(ProgramSkillFrame f) => f.MatchingRecast is { Owner.CostIssued: true } && _coreMatchingRecastObserversOperations.ValidMatchingRecastReceipt(f);
    [ProgramAiChoice(SkillProgramEffectOp.RecastMatchingCardsThenPeerChoice)]
    internal PromptChoice SelectAiMatchingRecast(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderBy(c => c.Parameters["step"] == "damage" ? 1 : 0).ThenBy(c => c.Cards.Count == 1 ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat]) : 0).ThenBy(c => c.Id.Value).First();
    SkillProgramStepOutcome global::CardGame.Core.IMatchingRecastProgramHost.RecastMatchingCardsThenPeerChoice(ProgramSkillFrame f, int peer, string ledger) => BeginMatchingRecast(f, peer, ledger);
}
