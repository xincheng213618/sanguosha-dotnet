#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanBoundaryMobileRuntime
{
    internal const string BoundarySunQuanMobileJiuyuan = "boundary:jiuyuan-mobile";
    internal const string BoundarySunQuanMobilePolicy = "redirect-self-peach";
    internal IReadOnlyList<RecoveryReplacementCandidate> BoundarySunQuanMobileRecoveryCandidates(long parentFrameId, int sourceSeat, int targetSeat, RecoveryAttemptCompletion completion, IReadOnlyList<RecoveryReplacementCandidate> ordinary)
    {
        var retained = ordinary.Where(candidate => candidate.SkillId != BoundarySunQuanMobileJiuyuan);
        if (!IsBoundarySunQuanMobileSelfPeach(parentFrameId, sourceSeat, targetSeat, completion) || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0 || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[targetSeat].HasHpValue || !_characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoAnyWounded(targetSeat))
            return Array.AsReadOnly(retained.ToArray());
        var provider = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var thresholds = provider.HappyQiaoHealth is { } dual ? new[]
        {
            dual.DaQiaoHp,
            dual.XiaoQiaoHp
        }

        : new[]
        {
            provider.Hp
        };
        var offered = _coreMatchParticipantsPlayersAccess._players.Where(owner => owner.Seat != targetSeat && owner.IsAlive && owner.HasHpValue && thresholds.Any(hp => owner.HappyQiaoHealth is { } other ? other.DaQiaoHp < hp || other.XiaoQiaoHp < hp : owner.Hp < hp)).OrderBy(owner => _coreTableOrderOperations.RelativeTableOrder(targetSeat, owner.Seat)).SelectMany(owner => _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery).Where(item => item.Source.SkillId == BoundarySunQuanMobileJiuyuan && item.Policy.Id == BoundarySunQuanMobilePolicy && item.Policy.FactionId == _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider)).Select(item => new RecoveryReplacementCandidate(owner.Seat, item.Source.SkillId, item.Source.SkillInstanceId, item.Policy.Id, item.Policy.Value, item.Policy.ProviderDrawCount)));
        return Array.AsReadOnly(retained.Concat(offered).GroupBy(candidate => (candidate.OwnerSeat, candidate.SkillId, candidate.PolicyId)).Select(group => group.First()).ToArray());
    }

    internal bool IsBoundarySunQuanMobileSelfPeach(long parentFrameId, int sourceSeat, int targetSeat, RecoveryAttemptCompletion completion) => sourceSeat == targetSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) && completion.Producer is (RecoveryAttemptProducer.CardUse or RecoveryAttemptProducer.VirtualBasic) && _coreCardUseLifecycleOperations.LifecycleCardUse(parentFrameId)is { SourceSeat: var actor, CardKind: CardKind.Peach, Action: { Type: CardActionType.Use, EffectiveKind: CardKind.Peach } action } && actor == sourceSeat && action.ActorSeat == sourceSeat;
    internal bool IsBoundarySunQuanMobileRecoveryCandidateCurrent(RecoveryReplacementFrame frame, RecoveryReplacementCandidate candidate)
    {
        var attempt = frame.Attempt;
        if (candidate.SkillId != BoundarySunQuanMobileJiuyuan || candidate.PolicyId != BoundarySunQuanMobilePolicy || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attempt.TargetSeat) || candidate.OwnerSeat == attempt.TargetSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].HasHpValue || !_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat].HasHpValue || attempt.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || attempt.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].Hp >= _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoRecoveryHpBefore(frame) || !IsBoundarySunQuanMobileSelfPeach(frame.ParentFrameId, attempt.SourceSeat, attempt.TargetSeat, attempt.Completion))
            return false;
        return _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery).Any(item => item.Source.SkillId == candidate.SkillId && item.Source.SkillInstanceId == candidate.SkillInstanceId && item.Policy.Id == candidate.PolicyId && item.Policy.FactionId == _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[attempt.TargetSeat]) && item.Policy.Value == candidate.RecoveryAmount && item.Policy.ProviderDrawCount == candidate.ProviderDrawCount);
    }
}
