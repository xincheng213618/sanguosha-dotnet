#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuJinMouMobileRuntime
{
    internal const string MouXiayuan = MouYuJinComposition.Xiayuan, MouJieyue = MouYuJinComposition.Jieyue;
    internal const string MouXiayuanCostReason = "skill-program.mou-mobile:xiayuan.MoveBoundCards";
    internal bool HasMouYuJinCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouYuJinXiayuan) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouYuJinJieyue);

    internal static CardConversionSource MouYuJinSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_yu_jin_mou_mobile_MouYuJinXiayuan.MouYuJinSource(f);
    internal Card[] MouXiayuanLegalCosts(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard)).ToArray();
    internal DamageFrame? MouXiayuanDamage(ProgramSkillWindowContext c) => c.Window == SkillProgramTriggerWindow.AfterDamageApplied && c.DamageFrameId is { } id && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(f => f.Id == id)is { ArmorPayment: { } p } damage && _coreDamageArmorPaymentsOperations.IsIssuedDamageArmorPayment(damage) && c.TargetSeat == p.TargetSeat && c.Amount == p.DamageAmount && c.OccurrenceIndex == 0 && c.SourceSeat == (p.SourceLess ? null : (int? )p.SourceSeat) && p.ArmorBefore > 0 && p.ArmorAbsorbed > 0 && p.ArmorAfter == 0 ? damage : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29300, SkillProgramEffectOp.ResolveMouYuJinXiayuan)]
    internal bool CanOfferMouXiayuan(ProgramTriggerLegalityContext c) => c.SkillId == MouXiayuan && MouYuJinComposition.IsXiayuan(c.Program) && c.Owner.IsAlive && MouXiayuanDamage(c.Context)is { } d && d.TargetSeat != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[d.TargetSeat].IsAlive && MouXiayuanLegalCosts(c.OwnerSeat).Length >= 2;
    internal DamageFrame? MouXiayuanRoot(ProgramSkillFrame f)
    {
        if (f.SkillId != MouXiayuan || f.TriggerId != "restore-after-armor-loss" || f.ActivationId != f.TriggerId || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } c || c.OwnerSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } program || program.GameplayHash != f.GameplayHash || !MouYuJinComposition.IsXiayuan(program) || MouXiayuanDamage(c)is not { } damage || damage.TargetSeat == f.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } window || window.ParentFrameId != damage.Id || window.TriggerWindow != c.Window || window.TargetSeat != damage.TargetSeat || window.SourceSeat != damage.SourceSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex].ToProgramCandidate()is not { } candidate || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate))
            return null;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return i > 1 && _resolutionStackOperations._resolutionStack[i - 1].Id == window.Id && _resolutionStackOperations._resolutionStack[i - 2].Id == damage.Id ? damage : null;
    }

    internal bool ValidMouXiayuan(ProgramSkillFrame f) => f.MouYuJinXiayuan is { } r && MouXiayuanRoot(f)is { } d && r.Source == MouYuJinSource(f) && r.GameplayHash == f.GameplayHash && r.OriginalDamageWindowId == f.WindowContext!.ParentFrameId && r.OriginalPayment == d.ArmorPayment && r.RoundNumber > 0 && r.MovementBefore >= 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouYuJinXiayuanIssuedEvent>().Count(e => e == new MouYuJinXiayuanIssuedEvent(f.Id, r.Source, r.GameplayHash, r.OriginalDamageWindowId, r.OriginalPayment, r.RoundNumber, r.TurnNumber, r.MovementBefore)) == 1 && (!r.CostReturned || MouXiayuanPayment(f)is { } cost && _coreProgramEventHistoryOperations.ProgramEventHistory<MouYuJinXiayuanCostReturnedEvent>().Count(e => e.FrameId == f.Id && e.InvocationId == cost.InvocationId && e.ActualHandCount == 2) == 1) && (!r.Completed || _coreProgramEventHistoryOperations.ProgramEventHistory<MouYuJinXiayuanCompletedEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == d.TargetSeat && (e.RestoredAmount == 0 || e.RestoredAmount == r.OriginalPayment.ArmorAbsorbed)) == 1);
    internal NativeLogicalDiscardInvocationRecordedEvent? MouXiayuanPayment(ProgramSkillFrame f)
    {
        if (f.MouYuJinXiayuan is not { } r || f.CardSetBindings.SingleOrDefault(b => b.Name == "xiayuan-cost")is not { } bind || bind.CardIds.Count != 2 || bind.CardIds.Distinct().Count() != 2 || bind.SourceLocations.Count != 2 || bind.SourceLocations.Any(loc => loc != CardLocation.Hand(f.OwnerSeat)) || bind.SelectionActorSeat != f.OwnerSeat)
            return null;
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ParentFrameId == f.Id && e.InitiatorSeat == f.OwnerSeat && e.ExecutorSeat == f.OwnerSeat && e.SubjectSeat == f.OwnerSeat && e.ActualCount == 2 && e.HandCount == 2 && e.SequenceBefore >= r.MovementBefore && e.TurnNumber == r.TurnNumber && e.Reason.Value == MouXiayuanCostReason).ToArray();
        if (facts is not [var fact])
            return null;
        return bind.CardIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > fact.SequenceBefore && m.Sequence <= fact.SequenceAfter && (m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == MouXiayuanCostReason || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == CardLocation.Hand(f.OwnerSeat) && (m.Reason.Value == MouXiayuanCostReason || m.Reason.Value == MouXiayuanCostReason + "-finished" || m.Reason.Value == MouXiayuanCostReason + "-discard")))) ? fact : null;
    }

    internal SkillProgramStepOutcome ResolveMouXiayuan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (MouXiayuanRoot(f)is not { } damage)
            throw new InvalidOperationException("Xiayuan requires its exact original issued damage Armor payment and native candidate.");
        if (f.InstructionIndex == 1)
        {
            if (f.MouYuJinXiayuan is not null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[damage.TargetSeat].IsAlive || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1)
                throw new InvalidOperationException("Xiayuan cannot reissue or replace its original Round damage invoice.");
            var r = new MouYuJinXiayuanReceipt(MouYuJinSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, damage.ArmorPayment!, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressTurnNumberAccess._turnNumber, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouYuJinXiayuan = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouYuJinXiayuanIssuedEvent(f.Id, r.Source, r.GameplayHash, r.OriginalDamageWindowId, r.OriginalPayment, r.RoundNumber, r.TurnNumber, r.MovementBefore));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.InstructionIndex != 4 || !ValidMouXiayuan(f) || f.MouYuJinXiayuan is not { Completed: false } receipt || f.PendingMovementContinuation is not null || MouXiayuanPayment(f)is not { } paid)
            throw new InvalidOperationException("Xiayuan restores only after two real Hand materials and their native children return.");
        if (!receipt.CostReturned)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouYuJinXiayuan = receipt = receipt with { CostReturned = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouYuJinXiayuanCostReturnedEvent(f.Id, paid.InvocationId, paid.HandCount));
        }

        var target = _coreMatchParticipantsPlayersAccess._players[receipt.OriginalPayment.TargetSeat];
        var amount = target.IsAlive ? receipt.OriginalPayment.ArmorAbsorbed : 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouYuJinXiayuan = receipt with { Completed = true } });
        if (amount > 0)
        {
            var before = target.Armor;
            var after = checked(before + amount);
            target.Armor = after;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorGrantedEvent(f.Id, 3, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), target.Seat, amount, before, after));
            _coreMatchLogOperations.AddLog("ArmorGranted", $"{(target.Name)} 获得 {(amount)} 点护甲，现有 {(after)} 点。", f.OwnerSeat, target.Seat);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouYuJinXiayuanCompletedEvent(f.Id, target.Seat, amount));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29300)]
    internal bool ReturnMouXiayuanMovement(ProgramSkillFrame f)
    {
        if (f.MouYuJinXiayuan is null)
            return false;
        if (!ValidMouXiayuan(f) || f.InstructionIndex != 3 || f.PendingMovementContinuation is not { SubjectSeat: var owner, CoverageResultBind: null } || owner != f.OwnerSeat || MouXiayuanPayment(f)is null)
            throw new InvalidOperationException("Xiayuan cannot resume an unrelated or unpaid bound discard.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal (ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouXiayuanForAi(CharacterState owner, ProgramAiPublicContext context, ProgramSkillWindowContext? window)
    {
        var costs = MouXiayuanLegalCosts(owner.Seat).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(c, owner)).ThenBy(c => c.Id).Take(2).ToArray();
        if (costs.Length != 2 || window is null || MouXiayuanDamage(window)is not { ArmorPayment: { } payment })
            return (new(new(0, 0, 0, 0, 0, 0, false, false), 0, false, "狭援无原始耗尽护甲回执或没有足额两张合法手牌。"), null);
        var condition = new SkillProgramCondition(SkillProgramConditionKind.Always, 0, []);
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
        var target = snapshot.Players.Single(p => p.Seat == payment.TargetSeat);
        // This estimate only grants Armor: use public recipient numbers, never
        // inspect its hidden skill qualifications or private prediction policy.
        var recipient = new PlayerSkillContext(target.Seat, target.Hp, target.MaxHp, target.HandCount, snapshot.Phase, target.Seat == snapshot.CurrentSeat)
        {
            Armor = target.Armor
        };
        var estimate = ProgramCompositionAi.Estimate([new SkillProgramEffect(SkillProgramEffectOp.GrantArmor, SkillProgramEffectTarget.SelectedTarget, payment.ArmorAbsorbed, condition)], _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner), publicContext: context with { SelectedTarget = recipient });
        var value = _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ScoreProgramTarget(snapshot, payment.TargetSeat, estimate.Hint);
        var cost = costs.Sum(c => CoreNativeAiDecisionsRuntime.GetKeepValue(c, owner)) * 0.35d;
        return (estimate with { Score = estimate.Score + value - cost, Approximation = "按公开原始失甲量估算盟友护甲收益，并扣除自己实际两张最低保留价值手牌。" }, payment.TargetSeat);
    }

    internal PromptChoice SelectAiMouXiayuanCost(PendingDecision p, ProgramSkillFrame f) => p.Choices.Where(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards").OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Cards[0]).First();
    SkillProgramStepOutcome IMouYuJinHost.Xiayuan(ProgramSkillFrame f) => ResolveMouXiayuan(f);
    SkillProgramStepOutcome IMouYuJinHost.Jieyue(ProgramSkillFrame f) => ResolveMouJieyue(f);
}
