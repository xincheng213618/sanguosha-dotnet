#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMobileGodRuntime
{
    internal const string ShenLsReason = "skill-program.mobile-shen-lu-su.";
    internal bool ShenLsTeam => _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.ModeKind == ContentModeKind.Team && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.TeamActivity is null && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.BossActivity is null && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.HulaoActivity is null && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.ShenwuActivity is null && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.NianActivity is null && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.HellActivity is null && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.DragonBoatActivity is null && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.GuanduActivity is null && _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.WenheActivity is null;
    internal bool ShenLsMode => _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.ModeKind == ContentModeKind.Identity || ShenLsTeam;

    internal static CardConversionSource ShenLsSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, f.SkillInstanceId);
    internal int[] ShenLsField(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[seat])).Select(c => c.Id).ToArray();
    internal int[] ShenLsGiveable(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Transfer)).Select(c => c.Id).ToArray();
    internal bool ShenLsDingzhouTarget(int owner, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && owner != target && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && ShenLsField(target).Length is> 0 and var count && ShenLsGiveable(owner).Length >= count;
    internal int[] ShenLsPeers(int owner, string skillId) => _coreTableOrderOperations.CurrentTableOrder().Where(s => s != owner && _coreMatchParticipantsPlayersAccess._players[s].IsAlive && (skillId == "happy:zhimeng" || !ShenLsTeam || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count + 1)).ToArray();
    SkillProgramStepOutcome global::CardGame.Core.IShenLuSuHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenLs(f, op);
    bool global::CardGame.Core.IShenLuSuSelection.DingzhouTarget(int owner, int target) => ShenLsDingzhouTarget(owner, target);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 38900, SkillProgramEffectOp.ResolveShenLuSuTamo, SkillProgramEffectOp.ResolveShenLuSuZhimeng)]
    internal bool ShenLsEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => ShenLsMode && (t.Effects[0].Op == SkillProgramEffectOp.ResolveShenLuSuTamo ? w.Window == SkillProgramTriggerWindow.GameStarting && _coreTableOrderOperations.CurrentTableOrder().Count - (_coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.ModeKind == ContentModeKind.Identity ? 1 : 0) > 1 : w.Window == SkillProgramTriggerWindow.AfterTurnEnded && w.SourceSeat == c.OwnerSeat && ShenLsPeers(c.OwnerSeat, c.SkillId).Length > 0);
    internal SkillProgramStepOutcome BeginShenLs(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenLuSu is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Shen Lu Su requires his original unpaid instruction.");
        var target = op == SkillProgramEffectOp.ResolveShenLuSuDingzhou ? f.SelectedTargetSeats.Single() : -1;
        if (target >= 0 && !ShenLsDingzhouTarget(f.OwnerSeat, target))
            throw new InvalidOperationException("Dingzhou requires a real other field and its exact payable exchange.");
        var stage = op == SkillProgramEffectOp.ResolveShenLuSuTamo ? ShenLuSuStage.SeatChoice : op == SkillProgramEffectOp.ResolveShenLuSuDingzhou ? ShenLuSuStage.GiftChoice : ShenLuSuStage.PeerChoice;
        var r = new ShenLuSuReceipt(ShenLsSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0, stage)
        {
            Target = target,
            Amount = target >= 0 ? ShenLsField(target).Length : 0,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (stage == ShenLuSuStage.SeatChoice)
        {
            var order = _coreTableOrderOperations.CurrentTableOrder();
            var fixedSeat = _coreMatchDefinitionModeDefinitionPlayerCountAccess._modeDefinition.ModeKind == ContentModeKind.Identity ? _coreMatchParticipantsPlayersAccess._players.Single(p => p.Role == Role.Lord).Seat : -1;
            r = r with
            {
                FixedSeat = fixedSeat,
                OriginalOrder = Array.AsReadOnly(order.ToArray()),
                Order = fixedSeat < 0 ? [] : Array.AsReadOnly(new[] { fixedSeat })
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenLuSu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenLuSuStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, target, r.Amount));
        ContinueShenLs(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool DrainShenLs(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    internal void ContinueShenLs(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShenLuSu: { } r } f && f.Id == id)
        {
            if (DrainShenLs(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShenLuSu!;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishShenLs(f);
                return;
            }

            if (r.Stage == ShenLuSuStage.GiftChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
                {
                    FinishShenLs(f);
                    return;
                }

                var field = ShenLsField(r.Target);
                if (field.Length == 0)
                {
                    FinishShenLs(f);
                    return;
                }

                MoveShenLs(f, field, ShenLuSuStage.ObtainChildren, CardLocation.Hand(f.OwnerSeat), null);
                continue;
            }

            if (r.Stage is ShenLuSuStage.ObtainChildren or ShenLuSuStage.DivideChildren)
            {
                FinishShenLs(f);
                return;
            }

            if (r.Stage == ShenLuSuStage.SeatChoice && r.Order.Count == r.OriginalOrder.Count)
            {
                ApplyShenLsOrder(f);
                FinishShenLs(f);
                return;
            }

            if (r.Stage == ShenLuSuStage.GiftChoice && r.Cards.Count == r.Amount)
            {
                MoveShenLs(f, r.Cards, ShenLuSuStage.GiftChildren, CardLocation.Hand(r.Target), null);
                continue;
            }

            PublishShenLs(f);
            return;
        }
    }

    internal void FinishShenLs(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenLuSu = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void MoveShenLs(ProgramSkillFrame f, IReadOnlyList<int> cards, ShenLuSuStage stage, CardLocation destination, Func<Card, CardLocation, CardLocation>? perCard)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenLuSu = f.ShenLuSu!with { Stage = stage, Paid = false, Before = before, After = before, BatchId = 0, MovementCount = cards.Count, MovementInvoices = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var reason = ShenLsReason + stage;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, destination, new(reason), (batch, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { ShenLuSu = parent.ShenLuSu!with { Paid = true, BatchId = batch, After = moves[^1].Sequence, MovementInvoices = Array.AsReadOnly(moves.Select(m => new ShenLuSuMoveInvoice(m.CardId, m.From, m.To, m.Sequence)).ToArray()) } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenLuSuMovementPaidEvent(f.Id, stage, batch, moves.Count, before, moves[^1].Sequence));
        }, perCard, stage == ShenLuSuStage.ObtainChildren ? CardMovementDisclosure.FaceUp : CardMovementDisclosure.FaceDown);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 38900)]
    internal bool ResumeShenLs(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenLuSu: not null } f || f.Id != id)
            return false;
        ContinueShenLs(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 38900)]
    internal bool ReturnShenLsMovement(ProgramSkillFrame f) => f.ShenLuSu is not null && ResumeShenLs(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 38900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 38900)]
    internal bool KeepShenLsPaid(ProgramSkillFrame f) => f.ShenLuSu is not null;
}
