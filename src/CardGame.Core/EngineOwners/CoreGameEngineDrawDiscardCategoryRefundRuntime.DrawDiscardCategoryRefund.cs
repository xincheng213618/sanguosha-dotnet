#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDrawDiscardCategoryRefundRuntime
{
    internal long DrawDiscardCategorySequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static string DrawDiscardCategoryReason(ProgramSkillFrame f, string part) => RuntimeSkills.RuntimeRules_Core_DrawDiscardCategoryRefund.DrawDiscardCategoryReason(f, part);
    internal bool DrawDiscardCategoryTargetAvailable(int owner, string skill, string stateId, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawDiscardCategoryTargetBannedEvent>().Any(e => e.OwnerSeat == owner && e.SkillId == skill && e.StateId == stateId && e.TargetSeat == target && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
    internal SkillProgramStepOutcome BeginDrawDiscardCategoryRefund(ProgramSkillFrame supplied, int target, int amount, string stateId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var activation = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation;
        if (activation is null || f.TriggerId is not null || f.InstructionIndex != 1 || f.DrawDiscardCategoryRefund is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var original] || original != target || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || !DrawDiscardCategoryTargetAvailable(f.OwnerSeat, f.SkillId, stateId, target) || activation.Effects is not [var configured] || configured.Op != SkillProgramEffectOp.DrawThenDiscardDistinctCategories || configured.Amount != amount || configured.StateId != stateId || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, activation.UsageGroup)) < 1)
            throw new InvalidOperationException("The draw/discard activation lost its exact live target, phase debit or sole instruction.");
        var r = new ProgramDrawDiscardCategoryReceipt(f.InstructionIndex, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, stateId, activation.UsageGroup, target, amount, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, DrawDiscardCategoryStage.Drawing);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DrawDiscardCategoryRefund = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawDiscardCategoryStartedEvent(f.Id, r.Source, r.GameplayHash, stateId, r.UsageGroup, target, amount, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.PhaseInstanceId));
        IssueDrawDiscardCategoryDraw(f, false);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void IssueDrawDiscardCategoryDraw(ProgramSkillFrame f, bool bonus)
    {
        var r = f.DrawDiscardCategoryRefund!;
        var before = DrawDiscardCategorySequence;
        if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || bonus && (!r.DistinctNonEmpty || r.BonusIssued) || !bonus && r.DrawIssued)
            throw new InvalidOperationException("A draw/discard draw may only be issued once to its living original target.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DrawDiscardCategoryRefund = r with { Stage = bonus ? DrawDiscardCategoryStage.BonusChildren : DrawDiscardCategoryStage.Drawing }, PendingMovementContinuation = new(r.TargetSeat, 0, null) });
        var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], bonus ? 1 : r.RequestedCount, true, new(DrawDiscardCategoryReason(f, bonus ? "bonus" : "draw"))).Count;
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = active.DrawDiscardCategoryRefund!;
        r = bonus ? r with
        {
            BonusIssued = true,
            BonusActual = actual,
            BonusBefore = before,
            BonusAfter = DrawDiscardCategorySequence
        }

        : r with
        {
            DrawIssued = true,
            DrawActual = actual,
            DrawBefore = before,
            DrawAfter = DrawDiscardCategorySequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { DrawDiscardCategoryRefund = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawDiscardCategoryDrawIssuedEvent(f.Id, r.TargetSeat, bonus, bonus ? 1 : r.RequestedCount, actual, before, DrawDiscardCategorySequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal DrawDiscardCategoryMaterial[] DrawDiscardCategoryLegalMaterials(int seat) => new[]
    {
        CardLocation.Hand(seat),
        CardLocation.Equipment(seat)
    }.SelectMany(from => _cardZonesOperations._cardZones.CardsAt(from).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, from, OwnedCardMoveIntent.Discard)).Select(c => new DrawDiscardCategoryMaterial(c.Id, from, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind)))).ToArray();
    internal IReadOnlyList<PromptChoice> DrawDiscardCategoryChoices(ProgramSkillFrame f)
    {
        var r = f.DrawDiscardCategoryRefund!;
        return Array.AsReadOnly(r.EligibleMaterials.Where(m => !r.SelectedCardIds.Contains(m.CardId)).Select(m => new PromptChoice(new($"draw-discard-category.{(f.Id)}.{(r.SelectedCardIds.Count)}.{(m.CardId)}"), $"弃置【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】", [m.CardId], [], new Dictionary<string, string> { ["program-action"] = "draw-discard-category", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal void PublishDrawDiscardCategoryChoice(ProgramSkillFrame f) => _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.DrawDiscardCategoryRefund!.TargetSeat, DrawDiscardCategoryChoices(f), $"选择 {(f.DrawDiscardCategoryRefund.RequiredCount)} 张手牌或装备牌，选齐后同时弃置。", f.DrawDiscardCategoryRefund.TargetSeat);
    [ProgramAnswer("draw-discard-category")]
    internal void ResolveDrawDiscardCategoryChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Draw/discard payment lost its owning program.");
        var r = f.DrawDiscardCategoryRefund!;
        if (r.Stage != DrawDiscardCategoryStage.ChoosingDiscard || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.TargetSeat || _corePendingDecisionPolicyPipelineOperations._pendingDecision.IsPrivate != true || choice.Parameters.GetValueOrDefault("program-action") != "draw-discard-category" || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Cards is not [var id] || choice.Targets.Count != 0 || r.SelectedCardIds.Count >= r.RequiredCount || !DrawDiscardCategoryChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards)))
            throw new InvalidOperationException("The mandatory private discard answer changed its target, frozen entity or cursor.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        r = r with
        {
            SelectedCardIds = r.SelectedCardIds.Append(id).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DrawDiscardCategoryRefund = r });
        if (r.SelectedCardIds.Count == r.RequiredCount)
            PayDrawDiscardCategory(f);
        else
            PublishDrawDiscardCategoryChoice(f);
    }

    internal void PayDrawDiscardCategory(ProgramSkillFrame f)
    {
        var r = f.DrawDiscardCategoryRefund!;
        var legal = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive ? DrawDiscardCategoryLegalMaterials(r.TargetSeat) : [];
        if (r.DiscardIssued || r.SelectedCardIds.Count != r.RequiredCount || r.SelectedCardIds.Any(id => !r.EligibleMaterials.Any(m => m.CardId == id && legal.Contains(m))))
            throw new InvalidOperationException("The complete frozen discard batch is no longer an exact payable set.");
        var before = DrawDiscardCategorySequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DrawDiscardCategoryRefund = r with { Stage = DrawDiscardCategoryStage.DiscardChildren, DiscardIssued = true, PaymentBefore = before, PaymentAfter = before }, PendingMovementContinuation = new(r.TargetSeat, 0, null) });
        void Commit(long? batch, IReadOnlyList<CardMovementRecord> records)
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = current.DrawDiscardCategoryRefund!;
            var invoice = records.Where(m => IsDrawDiscardCategoryPaidDestination(f, m) && m.From.OwnerSeat == r.TargetSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.Reason.Value == DrawDiscardCategoryReason(f, "discard")).Select(m => new DrawDiscardCategoryDiscard(m.CardId, m.From, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(m.CardKind), m.Sequence)).ToArray();
            var distinct = invoice.Length > 0 && invoice.Select(m => m.Category).Distinct().Count() == invoice.Length;
            paid = paid with
            {
                ActualDiscards = invoice,
                DistinctNonEmpty = distinct,
                PaymentBatchId = batch,
                PaymentAfter = records.Count == 0 ? before : records.Max(m => m.Sequence)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { DrawDiscardCategoryRefund = paid });
            foreach (var m in invoice)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawDiscardCategoryEntityDiscardedEvent(f.Id, r.TargetSeat, m.CardId, m.From, m.Category, m.MovementSequence));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawDiscardCategoryDiscardPaidEvent(f.Id, r.TargetSeat, r.RequiredCount, invoice.Length, distinct, batch, before, paid.PaymentAfter));
        }

        if (r.RequiredCount == 0)
            Commit(null, []);
        else
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TargetSeat, r.SelectedCardIds, new(DrawDiscardCategoryReason(f, "discard")), (batch, records) => Commit(batch, records));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool IsDrawDiscardCategoryPaidDestination(ProgramSkillFrame frame, CardMovementRecord movement) => movement.To == CardLocation.DiscardPile || _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(movement) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Where(e => e.CardId == movement.CardId).ToArray()is [var destroyed] && destroyed.MovementSequence == movement.Sequence && destroyed.ParentFrameId == frame.Id;
    [ProgramContinuation(ProgramContinuationKind.Resume, 210)]
    internal bool ResumeDrawDiscardCategoryRefund(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.DrawDiscardCategoryRefund is not { } r)
            return false;
        if (r.Stage == DrawDiscardCategoryStage.ChoosingDiscard)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishDrawDiscardCategoryChoice(f);
            return true;
        }

        if (r.Stage is DrawDiscardCategoryStage.Drawing or DrawDiscardCategoryStage.DiscardChildren or DrawDiscardCategoryStage.BonusChildren)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == DrawDiscardCategoryStage.Drawing)
            {
                var eligible = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive ? DrawDiscardCategoryLegalMaterials(r.TargetSeat) : [];
                r = r with
                {
                    Stage = DrawDiscardCategoryStage.ChoosingDiscard,
                    EligibleMaterials = eligible,
                    RequiredCount = Math.Min(r.RequestedCount, eligible.Length),
                    SelectedCardIds = []
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DrawDiscardCategoryRefund = r });
                if (r.RequiredCount == 0)
                    PayDrawDiscardCategory(f);
                else
                    PublishDrawDiscardCategoryChoice(f);
                return true;
            }

            if (r.Stage == DrawDiscardCategoryStage.DiscardChildren && r.DistinctNonEmpty && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            {
                IssueDrawDiscardCategoryDraw(f, true);
                return true;
            }

            if (r.DistinctNonEmpty)
                RefundDrawDiscardCategory(f);
            else
                CompleteDrawDiscardCategory(f);
            return true;
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    internal void RefundDrawDiscardCategory(ProgramSkillFrame f)
    {
        var r = f.DrawDiscardCategoryRefund!;
        if (!r.DistinctNonEmpty || r.TargetBanned || r.QuotaRefunded)
            throw new InvalidOperationException("A successful actual discard may refund its original debit and ban its target only once.");
        var key = (f.OwnerSeat, f.SkillId, r.UsageGroup);
        if (r.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && r.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && r.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play)
        {
            var before = _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key);
            if (before < 1)
                throw new InvalidOperationException("The accepted draw/discard phase debit disappeared before its refund.");
            _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = before - 1;
            r = r with
            {
                QuotaRefunded = true
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DrawDiscardCategoryRefund = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawDiscardCategoryRefundedEvent(f.Id, f.OwnerSeat, f.SkillId, r.StateId, f.ActivationId, r.UsageGroup, r.TargetSeat, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.PhaseInstanceId, before, before - 1));
        }

        r = r with
        {
            TargetBanned = true
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DrawDiscardCategoryRefund = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawDiscardCategoryTargetBannedEvent(f.Id, f.OwnerSeat, f.SkillId, r.StateId, r.TargetSeat, r.ActualTurnNumber, r.ActualTurnOwnerSeat));
        CompleteDrawDiscardCategory(f);
    }

    internal void CompleteDrawDiscardCategory(ProgramSkillFrame f)
    {
        var r = f.DrawDiscardCategoryRefund!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DrawDiscardCategoryRefund = r with { Stage = DrawDiscardCategoryStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DrawDiscardCategoryCompletedEvent(f.Id, r.DistinctNonEmpty, r.BonusIssued, r.QuotaRefunded, r.TargetBanned, _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal bool ValidDrawDiscardCategoryReceipt(ProgramSkillFrame f)
    {
        if (f.DrawDiscardCategoryRefund is not { } r)
            return false;
        var activation = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation;
        if (activation is null || f.TriggerId is not null || f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || activation.Effects is not [var e] || e.Op != SkillProgramEffectOp.DrawThenDiscardDistinctCategories || e.Amount != r.RequestedCount || e.StateId != r.StateId || activation.UsageGroup != r.UsageGroup || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || target != r.TargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !Enum.IsDefined(r.Stage) || r.RequestedCount is < 1 or > 20 || r.ActualTurnNumber < 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActualTurnOwnerSeat) || r.PhaseInstanceId < 0 || !r.DrawIssued || r.DrawActual < 0 || r.DrawActual > r.RequestedCount || r.DrawBefore < 0 || r.DrawAfter < r.DrawBefore || r.DrawAfter > DrawDiscardCategorySequence || r.RequiredCount < 0 || r.RequiredCount > r.RequestedCount || r.SelectedCardIds.Count > r.RequiredCount || r.SelectedCardIds.Distinct().Count() != r.SelectedCardIds.Count || r.EligibleMaterials.Select(m => m.CardId).Distinct().Count() != r.EligibleMaterials.Count || r.EligibleMaterials.Any(m => m.From.OwnerSeat != target || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || m.Category is not (SkillProgramCardCategory.Basic or SkillProgramCardCategory.Trick or SkillProgramCardCategory.Equipment)) || r.SelectedCardIds.Any(id => !r.EligibleMaterials.Any(m => m.CardId == id)))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<ProgramSkillStartedEvent>().Count(x => x.FrameId == f.Id && x.OwnerSeat == f.OwnerSeat && x.SkillId == f.SkillId && x.ActivationId == f.ActivationId) != 1)
            return false;
        var started = history.OfType<DrawDiscardCategoryStartedEvent>().Where(x => x.FrameId == f.Id).ToArray();
        if (started is not [var s] || s.Source != r.Source || s.GameplayHash != r.GameplayHash || s.StateId != r.StateId || s.UsageGroup != r.UsageGroup || s.TargetSeat != target || s.RequestedCount != r.RequestedCount || s.ActualTurnNumber != r.ActualTurnNumber || s.ActualTurnOwnerSeat != r.ActualTurnOwnerSeat || s.PhaseInstanceId != r.PhaseInstanceId)
            return false;
        var draws = history.OfType<DrawDiscardCategoryDrawIssuedEvent>().Where(x => x.FrameId == f.Id).ToArray();
        if (draws.Count(x => !x.Bonus) != 1 || draws.Count(x => x.Bonus) != (r.BonusIssued ? 1 : 0) || draws.Any(x => x.TargetSeat != target || x.RequestedCount != (x.Bonus ? 1 : r.RequestedCount) || x.ActualCount != (x.Bonus ? r.BonusActual : r.DrawActual) || x.SequenceBefore != (x.Bonus ? r.BonusBefore : r.DrawBefore) || x.SequenceAfter != (x.Bonus ? r.BonusAfter : r.DrawAfter)))
            return false;
        foreach (var draw in draws)
        {
            var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && m.Reason.Value == DrawDiscardCategoryReason(f, draw.Bonus ? "bonus" : "draw")).ToArray();
            if (draw.SequenceAfter < draw.SequenceBefore || draw.SequenceAfter > DrawDiscardCategorySequence || moves.Length != draw.ActualCount || moves.Any(m => m.To != CardLocation.Hand(target) || m.From != CardLocation.DrawPile || m.TurnNumber != r.ActualTurnNumber))
                return false;
        }

        var payments = history.OfType<DrawDiscardCategoryDiscardPaidEvent>().Where(x => x.FrameId == f.Id).ToArray();
        var entities = history.OfType<DrawDiscardCategoryEntityDiscardedEvent>().Where(x => x.FrameId == f.Id).ToArray();
        if (!r.DiscardIssued)
        {
            if (payments.Length != 0 || entities.Length != 0 || r.ActualDiscards.Count != 0 || r.DistinctNonEmpty || r.BonusIssued || r.QuotaRefunded || r.TargetBanned || r.Stage is not (DrawDiscardCategoryStage.Drawing or DrawDiscardCategoryStage.ChoosingDiscard))
                return false;
        }
        else
        {
            if (payments is not [var p] || p.TargetSeat != target || p.RequiredCount != r.RequiredCount || p.ActualDiscardCount != r.ActualDiscards.Count || p.DistinctNonEmpty != r.DistinctNonEmpty || p.BatchId != r.PaymentBatchId || p.SequenceBefore != r.PaymentBefore || p.SequenceAfter != r.PaymentAfter || r.PaymentBefore < r.DrawAfter || r.PaymentAfter < r.PaymentBefore || r.PaymentAfter > DrawDiscardCategorySequence || r.SelectedCardIds.Count != r.RequiredCount || r.ActualDiscards.Count > r.RequiredCount || r.ActualDiscards.Select(m => m.CardId).Distinct().Count() != r.ActualDiscards.Count || entities.Length != r.ActualDiscards.Count || r.RequiredCount == 0 != (r.PaymentBatchId is null) || r.DistinctNonEmpty != (r.ActualDiscards.Count > 0 && r.ActualDiscards.Select(m => m.Category).Distinct().Count() == r.ActualDiscards.Count))
                return false;
            var movements = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.PaymentBefore && m.Sequence <= r.PaymentAfter && m.Reason.Value == DrawDiscardCategoryReason(f, "discard")).ToArray();
            if (!movements.Select(m => m.CardId).SequenceEqual(r.SelectedCardIds) || movements.Any(m => !IsDrawDiscardCategoryPaidDestination(f, m) || m.TurnNumber != r.ActualTurnNumber || !r.EligibleMaterials.Any(x => x.CardId == m.CardId && x.From == m.From && x.Category == CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(m.CardKind))) || !movements.Select(m => new DrawDiscardCategoryDiscard(m.CardId, m.From, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(m.CardKind), m.Sequence)).SequenceEqual(r.ActualDiscards) || !entities.Select(x => new DrawDiscardCategoryDiscard(x.CardId, x.From, x.Category, x.MovementSequence)).SequenceEqual(r.ActualDiscards) || entities.Any(x => x.TargetSeat != target))
                return false;
        }

        if (r.Stage == DrawDiscardCategoryStage.Drawing && (r.DiscardIssued || r.EligibleMaterials.Count != 0 || r.RequiredCount != 0) || r.Stage == DrawDiscardCategoryStage.ChoosingDiscard && r.DiscardIssued || r.Stage == DrawDiscardCategoryStage.DiscardChildren && (!r.DiscardIssued || r.BonusIssued || r.QuotaRefunded || r.TargetBanned) || r.Stage == DrawDiscardCategoryStage.BonusChildren && (!r.DiscardIssued || !r.BonusIssued || r.QuotaRefunded || r.TargetBanned) || r.Stage == DrawDiscardCategoryStage.Complete && (!r.DiscardIssued || r.TargetBanned != r.DistinctNonEmpty) || r.BonusIssued && (!r.DiscardIssued || !r.DistinctNonEmpty || r.BonusActual is < 0 or > 1 || r.BonusBefore < r.PaymentAfter) || !r.BonusIssued && (r.BonusActual != 0 || r.BonusBefore != 0 || r.BonusAfter != 0) || (r.QuotaRefunded || r.TargetBanned) && !r.DistinctNonEmpty || history.OfType<DrawDiscardCategoryRefundedEvent>().Count(x => x.FrameId == f.Id) != (r.QuotaRefunded ? 1 : 0) || history.OfType<DrawDiscardCategoryTargetBannedEvent>().Count(x => x.FrameId == f.Id) != (r.TargetBanned ? 1 : 0))
            return false;
        if (history.OfType<DrawDiscardCategoryRefundedEvent>().Any(x => x.FrameId == f.Id && (x.OwnerSeat != f.OwnerSeat || x.SkillId != f.SkillId || x.StateId != r.StateId || x.ActivationId != f.ActivationId || x.UsageGroup != r.UsageGroup || x.TargetSeat != target || x.ActualTurnNumber != r.ActualTurnNumber || x.ActualTurnOwnerSeat != r.ActualTurnOwnerSeat || x.PhaseInstanceId != r.PhaseInstanceId || x.BeforeUsage < 1 || x.AfterUsage != x.BeforeUsage - 1)) || history.OfType<DrawDiscardCategoryTargetBannedEvent>().Any(x => x.FrameId == f.Id && (x.OwnerSeat != f.OwnerSeat || x.SkillId != f.SkillId || x.StateId != r.StateId || x.TargetSeat != target || x.ActualTurnNumber != r.ActualTurnNumber || x.ActualTurnOwnerSeat != r.ActualTurnOwnerSeat)))
            return false;
        if (r.Stage == DrawDiscardCategoryStage.ChoosingDiscard && (r.RequiredCount != Math.Min(r.RequestedCount, r.EligibleMaterials.Count) || f.PendingMovementContinuation is not null || !r.EligibleMaterials.SequenceEqual(DrawDiscardCategoryLegalMaterials(target))))
            return false;
        return f.PendingMovementContinuation is null || f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == target && r.Stage is DrawDiscardCategoryStage.Drawing or DrawDiscardCategoryStage.DiscardChildren or DrawDiscardCategoryStage.BonusChildren;
    }

    [ProgramContinuation(ProgramContinuationKind.MovementTail, 10)]
    internal bool ReturnDrawDiscardCategoryMovement(ProgramSkillFrame f)
    {
        if (f.DrawDiscardCategoryRefund is null || f.PendingMovementContinuation is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 680)]
    internal bool CanContinueDrawDiscardCategoryRefund(ProgramSkillFrame f) => f.DrawDiscardCategoryRefund is { DrawIssued: true } && ValidDrawDiscardCategoryReceipt(f);
    [ProgramAiChoice(SkillProgramEffectOp.DrawThenDiscardDistinctCategories)]
    internal PromptChoice SelectAiDrawDiscardCategory(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.DrawDiscardCategoryRefund!;
        var selected = r.SelectedCardIds.Select(id => r.EligibleMaterials.Single(m => m.CardId == id).Category).ToArray();
        var available = r.EligibleMaterials.Where(m => !r.SelectedCardIds.Contains(m.CardId) && !selected.Contains(m.Category)).Select(m => m.Category).Distinct().Count();
        var distinctPossible = selected.Distinct().Count() == selected.Length && available >= r.RequiredCount - selected.Length;
        return decision.Choices.OrderBy(c => distinctPossible && selected.Contains(r.EligibleMaterials.Single(m => m.CardId == c.Cards.Single()).Category) ? 1 : 0).ThenBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[r.TargetSeat])).ThenBy(c => c.Cards.Single()).First();
    }

    internal bool DrawDiscardCategoryFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        var r = f.DrawDiscardCategoryRefund!;
        var before = r.Stage == DrawDiscardCategoryStage.Drawing ? r.DrawBefore : r.Stage == DrawDiscardCategoryStage.BonusChildren ? r.BonusBefore : r.PaymentBefore;
        var after = r.Stage == DrawDiscardCategoryStage.Drawing ? r.DrawAfter : r.Stage == DrawDiscardCategoryStage.BonusChildren ? r.BonusAfter : r.PaymentAfter;
        var reason = DrawDiscardCategoryReason(f, r.Stage == DrawDiscardCategoryStage.Drawing ? "draw" : r.Stage == DrawDiscardCategoryStage.BonusChildren ? "bonus" : "discard");
        bool PaidEquipment(CardKind kind) => r.Stage == DrawDiscardCategoryStage.DiscardChildren && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > before && m.Sequence <= after && m.CardKind == kind && r.SelectedCardIds.Contains(m.CardId) && m.From == CardLocation.Equipment(r.TargetSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == reason);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > before && m.Sequence <= after && m.Reason.Value == reason && (r.Stage != DrawDiscardCategoryStage.DiscardChildren || moved.Batch.Id == r.PaymentBatchId) || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(r.TargetSeat) && m.To == CardLocation.DiscardPile && PaidEquipment(CardKind.WoodenOx)));
        if (!PaidEquipment(CardKind.SilverLion))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.TargetSeat && hp.Change.TargetSeat == r.TargetSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.TargetSeat && recovery.Attempt.TargetSeat == r.TargetSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == reason;
    }

    internal ProgramSkillFrame? CategoryActivationObserverRoot(bool awakening)
    {
        for (var index = 0; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || (awakening ? !_coreGameActivationAwakeningOperations.ValidGameActivationAwakeningReceipt(root) || !_coreGameActivationAwakeningOperations.GameActivationAwakeningFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1]) : !ValidDrawDiscardCategoryReceipt(root) || !DrawDiscardCategoryFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1])))
                continue;
            var aligned = true;
            for (var child = index + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (_resolutionStackOperations._resolutionStack[child] is ProgramLifecycleTriggerWindowFrame changed && _resolutionStackOperations._resolutionStack[child - 1] is ProgramSkillFrame owner && changed.Window == SkillProgramTriggerWindow.SkillsChanged && changed.ResumeProgramFrameId == owner.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && changed.CandidateIndex >= 0 && changed.CandidateIndex <= changed.Candidates.Count)
                    continue;
                if (!_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) && !_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child))
                {
                    aligned = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && child + 1 < _resolutionStackOperations._resolutionStack.Count && ((_coreDyingSuitsAndHistoricalUseObserversOperations.IsOriginalDyingSuspendedByDyingSuits(dying) || _coreRecipientCategoryMarkContinuationsOperations.Value.IsOriginalDyingSuspendedByRecipientCategoryMark(dying)) || _coreOwnedDeathBenefitReturnsOperations.Value.IsOriginalDyingSuspendedByOwnedDeathBenefit(dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying)))
                    break;
            }

            if (aligned)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 150)]
    internal bool IsDrawDiscardCategoryProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && CategoryActivationObserverRoot(false)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    SkillProgramStepOutcome IDrawDiscardCategoryProgramHost.DrawThenDiscardDistinctCategories(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => BeginDrawDiscardCategoryRefund(f, targetSeat, amount, stateId);
}
