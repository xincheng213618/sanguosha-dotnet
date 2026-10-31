#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiYangAnniversaryRuntime
{
    SkillProgramStepOutcome IAnniversaryCaiYangHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginCaiYang(f, op);
    internal SkillProgramStepOutcome BeginCaiYang(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.AnniversaryCaiYang is not null)
            throw new InvalidOperationException("Cai Yang requires his original unpaid instruction.");
        if (operation == SkillProgramEffectOp.ArmAnniversaryCaiYangXunji)
        {
            if (f.TriggerId is not null || f.ActivationId != AnniversaryCaiYangComposition.Arm || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiArmedEvent>().Any(e => e.Grant.FrameId == f.Id))
                throw new InvalidOperationException("Xunji designation requires its real once-per-Play selected other living target.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangXunjiArmedEvent(new(f.Id, CaiYangSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, target)));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.WindowContext is not { } w)
            throw new InvalidOperationException("Cai Yang lost his native trigger context.");
        if (operation == SkillProgramEffectOp.ResolveAnniversaryCaiYangJiaofeng)
        {
            var first = CaiYangFirstWitness(CaiYangCandidate(f), w) ?? throw new InvalidOperationException("Jiaofeng requires its exact first unfinalized source damage.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCaiYang = new(CaiYangSource(f), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, AnniversaryCaiYangStage.Drawing, FirstDamage: first) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangJiaofengStartedEvent(f.Id, first));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, CharacterCaiYangAnniversaryDefinitions.CaiYangJiaofengDrawReason);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (w.Window == SkillProgramTriggerWindow.TurnEnding)
        {
            var grant = CaiYangEndingWitness(CaiYangCandidate(f), w) ?? throw new InvalidOperationException("Xunji lost its issued next actual turn, true black use or real ending.");
            if (!_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(grant.Target, f.OwnerSeat))
                throw new InvalidOperationException("Xunji's native Duel must retain real current legality.");
            var origin = new CaiYangXunjiOrigin(f.Id, f.InstructionIndex, CaiYangSource(f), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, grant.Target, grant.FrameId, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, 0);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCaiYang = new(CaiYangSource(f), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, AnniversaryCaiYangStage.Duel, Duel: origin) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangXunjiConsumedEvent(grant.FrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, true));
            var native = new ProgramSelectedActorDuelOrigin(f.Id, f.InstructionIndex, f.OwnerSeat, f.OwnerSeat, f.SkillId, f.ActivationId!, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
            {
                InitialTargetSeat = grant.Target,
                CaiYangXunji = origin
            };
            var card = new Card(0, CardKind.Duel, Suit.None, 0);
            var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [grant.Target], CardKind.Duel, physicalCardIds: [], selectedActorDuelOrigin: native);
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
            if (id != origin.UseId || use.Action is not { Type: CardActionType.Use } action)
                throw new InvalidOperationException("Xunji lost its one true declared Duel action.");
            origin = origin with
            {
                ActionId = action.ActionId
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { SelectedActorDuelOrigin = native with { CaiYangXunji = origin } });
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryCaiYang = f.AnniversaryCaiYang!with { Duel = origin } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangXunjiIssuedEvent(origin));
            _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, [grant.Target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var duel = CaiYangDuelDamage(f.OwnerSeat, w) ?? throw new InvalidOperationException("Xunji retaliation requires its own original Duel's applied damage.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCaiYang = new(CaiYangSource(f), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, AnniversaryCaiYangStage.Damage, Duel: duel, DamageId: w.DamageFrameId!.Value, DamageTarget: w.TargetSeat!.Value, DamageAmount: w.Amount) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangXunjiDamageBoundEvent(f.Id, duel.UseId, w.DamageFrameId.Value, w.SourceSeat!.Value, w.TargetSeat.Value, w.Amount));
        if (!_coreMatchParticipantsPlayersAccess._players[w.TargetSeat.Value].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishCaiYang(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        return _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, w.Amount, explicitSourceSeat: w.TargetSeat.Value);
    }

    internal bool DrainCaiYang(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueCaiYang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryCaiYang: { } r } f || f.Id != id)
            return;
        if (r.Stage == AnniversaryCaiYangStage.Drawing)
            return;
        if (r.Stage == AnniversaryCaiYangStage.Duel)
        {
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.Duel!.UseId)is not null)
                return;
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.Duel.UseId && e.CardKind == CardKind.Duel && e.CardId == 0) != 1)
                throw new InvalidOperationException("Xunji must wait until its original native Duel is fully cleaned up.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangXunjiReturnedEvent(f.Id, r.Duel.UseId, r.Duel.ActionId));
            FinishCaiYang(f);
            return;
        }

        if (r.Stage == AnniversaryCaiYangStage.Damage)
        {
            if (f.AttackAttempt is not null)
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCaiYang = r with { Stage = AnniversaryCaiYangStage.DamageReturned } });
            r = f.AnniversaryCaiYang!;
        }

        if (DrainCaiYang(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.AnniversaryCaiYang!;
        if (r.Stage == AnniversaryCaiYangStage.DrawChildren && r.FirstDamage!.LostHp > 2 && !r.RecoveryIssued)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCaiYang = r with { Stage = AnniversaryCaiYangStage.RecoveryChildren, RecoveryIssued = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangJiaofengRecoveryIssuedEvent(f.Id, 1));
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
            if (DrainCaiYang(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        }

        FinishCaiYang(f);
    }

    internal void FinishCaiYang(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCaiYang = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryCaiYangJiaofeng)]
    internal void ReturnCaiYangDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryCaiYang!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryCaiYang = r with { Stage = AnniversaryCaiYangStage.DrawChildren, Draw = draw } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaiYangJiaofengDrawReturnedEvent(f.Id, draw.ActualDrawIds.Count, draw.AttemptFrameId, draw.SequenceBefore, draw.SequenceAfter));
        ContinueCaiYang(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 45600)]
    internal bool ResumeCaiYang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryCaiYang: not null } f || f.Id != id)
            return false;
        ContinueCaiYang(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 45600)]
    internal bool ReturnCaiYangMovement(ProgramSkillFrame f) => f.AnniversaryCaiYang is not null && ResumeCaiYang(f.Id);
    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 45600)]
    internal bool ReturnCaiYangDuel(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryCaiYang: { Stage: AnniversaryCaiYangStage.Duel, Duel: { } o } } f || completion.ResolutionId != o.UseId)
            return false;
        if (completion.ProgramFrameId != f.Id || _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiIssuedEvent>().Count(e => e.Origin == o) != 1)
            throw new InvalidOperationException("Xunji lost its exact original Duel cleanup return.");
        ContinueCaiYang(f.Id);
        return true;
    }
}
