#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBeiMiHuOlRuntime
{
    internal bool HasBeiMiHuPreludeBindings(CharacterState player, SkillProgramTriggerWindow window, SkillProgramEffectOp operation) => player.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(player).GetInstanceTriggers(window).Any(binding => ProgramInstructionResolver.Default.Features(binding.Trigger).HasOperation(operation));
    internal bool TryBeginBeiMiHuActualTurnPrelude(CharacterState owner)
    {
        if (!NeedsBeforeTurnPrelude(owner))
            return false;
        var oldRound = _coreActualRoundProgressRoundNumberAccess._roundNumber;
        _coreMouLuMengOperations.Value.BeginRoundForTurn(owner);
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var roundIndex = Array.FindLastIndex(events, item => item is RoundStartedEvent round && round.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var proof = new BeiMiHuBeforeTurnProof(id, checked(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber + 1), _coreActualRoundProgressRoundNumberAccess._roundNumber, owner.Seat, roundIndex + 1L);
        _coreRuntimeOperations.PushRuntimeFrame(new BeiMiHuTurnPreludeFrame(id, proof, _coreActualRoundProgressRoundNumberAccess._roundNumber != oldRound));
        _characterLuSuMobileShiOperations.ExpireShiLsHandGrants(owner.Seat, id);
        _coreRuntimeOperations.AdvanceRuntimeFrame(id);
        return true;
    }

    private bool NeedsBeforeTurnPrelude(CharacterState owner)
    {
        const SkillProgramTriggerWindow ownWindow = SkillProgramTriggerWindow.TurnBeforeStart;
        const SkillProgramTriggerWindow roundWindow = SkillProgramTriggerWindow.RoundStarting;

        return HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.MarkUnmarkedOtherBeforeActualTurn) ||
            _coreMatchParticipantsPlayersAccess._players.Any(player =>
                HasBeiMiHuPreludeBindings(player, roundWindow, SkillProgramEffectOp.MarkLowestHpUnmarkedOtherAtRoundStart)) ||
            _coreMatchParticipantsPlayersAccess._players.Any(player =>
                HasBeiMiHuPreludeBindings(player, roundWindow, SkillProgramEffectOp.ResolveAnniversaryLiuLiDehua)) ||
            (_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn) &&
                HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn) &&
                _eventHistory.CountActualTurnStarts(owner.Seat) == 0) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ResolveShenTaishiCiPowei) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ResolveHappyTaishiCiChongwei) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ObtainHappyTaishiCiStartSlash) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.EditDinghanNames) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.AwakenJianghuo) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ObtainWushuangDeckCards) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng) ||
            _characterLuSuMobileShiOperations.HasShiLsHandGrantsToExpire(owner.Seat) ||
            _characterChenZhiMobileShiOperations.ShiCzPendingExtra(owner.Seat) is not null ||
            _characterDongZhaoMobileShiOperations.ShiDzPendingExtra(owner.Seat) is not null;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 30, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(BeiMiHuTurnPreludeFrame), 70)]
    internal void ContinueBeiMiHuTurnPrelude()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not BeiMiHuTurnPreludeFrame parent || !_coreActualTurnsOperations.ValidBeforeTurnPrelude(parent.Proof) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Before-turn progression lost its active typed parent.");
        while (parent.Stage < 2)
        {
            var isRound = parent.Stage == 0;
            var window = isRound ? SkillProgramTriggerWindow.RoundStarting : SkillProgramTriggerWindow.TurnBeforeStart;
            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[parent.Proof.UpcomingOwnerSeat].IsAlive && (!isRound || parent.RoundAdvanced))
            {
                var participants = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && (isRound || player.Seat == parent.Proof.UpcomingOwnerSeat)).ToArray();
                var facts = participants.ToDictionary(player => player.Seat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts);
                var candidates = participants
                    .SelectMany(player => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(player, window, facts[player.Seat]))
                    .Where(candidate => IsBeforeTurnPreludeCandidate(candidate, isRound))
                    .OrderBy(candidate => _coreTableOrderOperations.RelativeTableOrder(parent.Proof.UpcomingOwnerSeat, candidate.OwnerSeat))
                    .ThenByDescending(candidate => candidate.Priority)
                    .ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal)
                    .ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal)
                    .ToArray();
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

    private bool IsBeforeTurnPreludeCandidate(ProgramTriggerCandidate candidate, bool isRound)
    {
        var features = ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate));
        if (isRound)
        {
            return features.HasOperation(SkillProgramEffectOp.MarkLowestHpUnmarkedOtherAtRoundStart) ||
                features.HasOperation(SkillProgramEffectOp.ResolveAnniversaryLiuLiDehua) ||
                features.HasOperation(SkillProgramEffectOp.ResolveLinjieRound) ||
                features.HasOperation(SkillProgramEffectOp.IssueHappyPangTongPlan) ||
                (candidate.SkillId == "happy:huanshu" && candidate.BindingId == "round-phantoms" &&
                    features.HasOperation(SkillProgramEffectOp.GainHappyZuoCiPhantoms));
        }

        return features.HasOperation(SkillProgramEffectOp.MarkUnmarkedOtherBeforeActualTurn) ||
            (features.HasOperation(SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn) &&
                _eventHistory.CountActualTurnStarts(candidate.OwnerSeat) == 0) ||
            features.HasOperation(SkillProgramEffectOp.ResolveShenTaishiCiPowei) ||
            features.HasOperation(SkillProgramEffectOp.ResolveHappyTaishiCiChongwei) ||
            features.HasOperation(SkillProgramEffectOp.ObtainHappyTaishiCiStartSlash) ||
            features.HasOperation(SkillProgramEffectOp.EditDinghanNames) ||
            features.HasOperation(SkillProgramEffectOp.AwakenJianghuo) ||
            features.HasOperation(SkillProgramEffectOp.ObtainWushuangDeckCards) ||
            features.HasOperation(SkillProgramEffectOp.ResolveMobileShiChenZhiQuanchong) ||
            features.HasOperation(SkillProgramEffectOp.ResolveMobileShiDongZhaoYingjia) ||
            features.HasOperation(SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng);
    }
}
