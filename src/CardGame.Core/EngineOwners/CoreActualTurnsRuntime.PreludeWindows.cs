#nullable enable

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreActualTurnsRuntime
{
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 30, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(BeiMiHuTurnPreludeFrame), 70)]
    internal void ContinueBeforeTurnPrelude()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not BeiMiHuTurnPreludeFrame parent || !ValidBeforeTurnPrelude(parent.Proof) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Before-turn progression lost its active typed parent.");
        while (parent.Stage < 2)
        {
            var isRound = parent.Stage == 0;
            var window = isRound ? SkillProgramTriggerWindow.RoundStarting : SkillProgramTriggerWindow.TurnBeforeStart;
            if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[parent.Proof.UpcomingOwnerSeat].IsAlive && (!isRound || parent.RoundAdvanced))
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
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return;
        if (!owner.IsAlive)
        {
            BeginTurn();
            return;
        }

        BeginTurnAfterBeforeTurnPrelude(owner);
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
