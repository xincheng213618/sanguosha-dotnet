#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBeiMiHuOlRuntime
{
    internal bool HasBeiMiHuPreludeBindings(CharacterState player, SkillProgramTriggerWindow window, SkillProgramEffectOp operation) => player.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(player).GetInstanceTriggers(window).Any(binding => ProgramInstructionResolver.Default.Features(binding.Trigger).HasOperation(operation));
    internal bool TryBeginBeiMiHuActualTurnPrelude(CharacterState owner)
    {
        if (!HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.MarkUnmarkedOtherBeforeActualTurn) && !_coreMatchParticipantsPlayersAccess._players.Any(player => HasBeiMiHuPreludeBindings(player, SkillProgramTriggerWindow.RoundStarting, SkillProgramEffectOp.MarkLowestHpUnmarkedOtherAtRoundStart)) && !_coreMatchParticipantsPlayersAccess._players.Any(player => HasBeiMiHuPreludeBindings(player, SkillProgramTriggerWindow.RoundStarting, SkillProgramEffectOp.ResolveAnniversaryLiuLiDehua)) && !(_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn) && HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn) && _characterSimaHuiOlOperations.SimaHuiOwnTurns(owner.Seat) == 0) && !HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.ResolveShenTaishiCiPowei) && !HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.ResolveHappyTaishiCiChongwei) && !HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.ObtainHappyTaishiCiStartSlash) && !HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.EditDinghanNames) && !HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.AwakenJianghuo) && !HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.ObtainWushuangDeckCards) && !HasBeiMiHuPreludeBindings(owner, SkillProgramTriggerWindow.TurnBeforeStart, SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng) && !_characterLuSuMobileShiOperations.HasShiLsHandGrantsToExpire(owner.Seat) && _characterChenZhiMobileShiOperations.ShiCzPendingExtra(owner.Seat)is null && _characterDongZhaoMobileShiOperations.ShiDzPendingExtra(owner.Seat)is null)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.Count != 0 || owner.Seat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat)
            throw new InvalidOperationException("Before-turn work requires a clean actual turn boundary.");
        var oldRound = _coreActualRoundProgressRoundNumberAccess._roundNumber;
        _coreMouLuMengOperations.BeginRoundForTurn(owner);
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var roundIndex = Array.FindLastIndex(events, item => item is RoundStartedEvent round && round.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
        if (roundIndex < 0 || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1)
            throw new InvalidOperationException("Before-turn work requires the actual round-start fact.");
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var proof = new BeiMiHuBeforeTurnProof(id, checked(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber + 1), _coreActualRoundProgressRoundNumberAccess._roundNumber, owner.Seat, roundIndex + 1L);
        _coreRuntimeOperations.PushRuntimeFrame(new BeiMiHuTurnPreludeFrame(id, proof, _coreActualRoundProgressRoundNumberAccess._roundNumber != oldRound));
        _characterLuSuMobileShiOperations.ExpireShiLsHandGrants(owner.Seat, id);
        _coreRuntimeOperations.AdvanceRuntimeFrame(id);
        return true;
    }

    internal bool ValidBeiMiHuBeforeTurnPrelude(BeiMiHuBeforeTurnProof proof)
    {
        if (_resolutionStackOperations._resolutionStack.FirstOrDefault()is not BeiMiHuTurnPreludeFrame parent || parent.Id != proof.PreludeFrameId || parent.Proof != proof || proof.UpcomingOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || proof.UpcomingTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber + 1 || proof.ActualRoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber || proof.RoundStartedEventSequence < 1 || parent.Stage is < 0 or > 2)
            return false;
        var ordinal = proof.RoundStartedEventSequence - 1;
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (ordinal > int.MaxValue || events.ElementAtOrDefault((int)ordinal)is not RoundStartedEvent round || round.RoundNumber != proof.ActualRoundNumber || ordinal != Array.FindLastIndex(events, item => item is RoundStartedEvent latest && latest.RoundNumber == proof.ActualRoundNumber))
            return false;
        var lastTurn = Array.FindLastIndex(events, item => item is TurnStartedEvent);
        // BeginRoundForTurn never advances a round for an Extra turn. A new
        // normal round belongs to this prelude only when its actual fact follows
        // the preceding turn and names this upcoming actor. If that actor died,
        // the next living actor can reuse the round without claiming its start.
        var beganThisRound = _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.Kind == ActualTurnKind.Normal && ordinal > lastTurn && round.ActorSeat == proof.UpcomingOwnerSeat;
        return parent.RoundAdvanced == beganThisRound;
    }

    internal bool ValidBeiMiHuPreludeLifecycleWindow(ProgramLifecycleTriggerWindowFrame window)
    {
        if (window.BeforeTurnPreludeProof is not { } proof || !ValidBeiMiHuBeforeTurnPrelude(proof) || _resolutionStackOperations._resolutionStack.FirstOrDefault()is not BeiMiHuTurnPreludeFrame parent || parent.WindowId != window.Id || window.OwnerSeat != proof.UpcomingOwnerSeat || window.CandidateIndex < 0 || window.CandidateIndex > window.Candidates.Count)
            return false;
        return parent.Stage switch
        {
            0 => parent.RoundAdvanced && window.Window == SkillProgramTriggerWindow.RoundStarting && window.Continuation == ProgramLifecycleContinuation.BeiMiHuPreludeRound,
            1 => window.Window == SkillProgramTriggerWindow.TurnBeforeStart && window.Continuation == ProgramLifecycleContinuation.BeiMiHuPreludeOwn,
            _ => false
        };
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 30, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(BeiMiHuTurnPreludeFrame), 70)]
    internal void ContinueBeiMiHuTurnPrelude()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not BeiMiHuTurnPreludeFrame parent || !ValidBeiMiHuBeforeTurnPrelude(parent.Proof) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Before-turn progression lost its active typed parent.");
        while (parent.Stage < 2)
        {
            var isRound = parent.Stage == 0;
            var window = isRound ? SkillProgramTriggerWindow.RoundStarting : SkillProgramTriggerWindow.TurnBeforeStart;
            var operation = isRound ? SkillProgramEffectOp.MarkLowestHpUnmarkedOtherAtRoundStart : SkillProgramEffectOp.MarkUnmarkedOtherBeforeActualTurn;
            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[parent.Proof.UpcomingOwnerSeat].IsAlive && (!isRound || parent.RoundAdvanced))
            {
                var participants = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && (isRound || player.Seat == parent.Proof.UpcomingOwnerSeat)).ToArray();
                var facts = participants.ToDictionary(player => player.Seat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts);
                var candidates = participants.SelectMany(player => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(player, window, facts[player.Seat])).Where(candidate => ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(operation) || isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ResolveAnniversaryLiuLiDehua) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn) && _characterSimaHuiOlOperations.SimaHuiOwnTurns(candidate.OwnerSeat) == 0 || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ResolveShenTaishiCiPowei) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ResolveHappyTaishiCiChongwei) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ObtainHappyTaishiCiStartSlash) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.EditDinghanNames) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.AwakenJianghuo) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ObtainWushuangDeckCards) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ResolveMobileShiChenZhiQuanchong) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ResolveMobileShiDongZhaoYingjia) || !isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng) || isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.ResolveLinjieRound) || isRound && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.IssueHappyPangTongPlan) || isRound && candidate.SkillId == "happy:huanshu" && candidate.BindingId == "round-phantoms" && ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)).HasOperation(SkillProgramEffectOp.GainHappyZuoCiPhantoms)).OrderBy(candidate => _coreTableOrderOperations.RelativeTableOrder(parent.Proof.UpcomingOwnerSeat, candidate.OwnerSeat)).ThenByDescending(candidate => candidate.Priority).ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal).ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray();
                if (candidates.Length != 0)
                {
                    var child = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.Proof.UpcomingOwnerSeat, window, candidates, isRound ? ProgramLifecycleContinuation.BeiMiHuPreludeRound : ProgramLifecycleContinuation.BeiMiHuPreludeOwn, facts[parent.Proof.UpcomingOwnerSeat])
                    {
                        ParticipantFacts = facts,
                        BeforeTurnPreludeProof = parent.Proof
                    };
                    _coreRuntimeOperations.ReplaceRuntimeTop(parent with { WindowId = child.Id });
                    _coreRuntimeOperations.PushRuntimeFrame(child);
                    _coreRuntimeOperations.AdvanceRuntimeFrame(child.Id);
                    return;
                }
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { Stage = parent.Stage + 1 });
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[parent.Proof.UpcomingOwnerSeat];
        _coreRuntimeOperations.PopResolutionFrame(parent.Id, ResolutionFrameKind.BeiMiHuTurnPrelude);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        if (!owner.IsAlive)
        {
            _coreActualTurnsOperations.BeginTurn();
            return;
        }

        _coreActualTurnsOperations.BeginTurnAfterBeforeTurnPrelude(owner);
    }

    internal void ReturnBeiMiHuTurnPreludeWindow(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!ValidBeiMiHuPreludeLifecycleWindow(window) || window.CandidateIndex != window.Candidates.Count || _resolutionStackOperations._resolutionStack.LastOrDefault()is not BeiMiHuTurnPreludeFrame parent)
            throw new InvalidOperationException("Completed before-turn work lost its singular parent return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Stage = parent.Stage + 1, WindowId = null });
        _coreRuntimeOperations.AdvanceRuntimeFrame(parent.Id);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 180, SkillProgramTriggerWindow.TurnBeforeStart)]
    internal bool ProveBeiMiHuTurnPrelude(CharacterState owner, ProgramSkillWindowContext context) => context.BeforeTurnPreludeProof is { } proof && ValidBeiMiHuBeforeTurnPrelude(proof) && context.SourceSeat == proof.UpcomingOwnerSeat && context.TargetSeat == proof.UpcomingOwnerSeat && owner.Seat == proof.UpcomingOwnerSeat;
}
