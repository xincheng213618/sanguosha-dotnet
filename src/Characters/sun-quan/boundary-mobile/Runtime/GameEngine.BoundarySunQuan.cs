namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundarySunQuanMobileJiuyuan = "boundary:jiuyuan-mobile";
    private const string BoundarySunQuanMobilePolicy = "redirect-self-peach";

    // The existing policy is the native recovery-replacement capability and
    // payment contract. This edition replaces only its admission rule.
    private IReadOnlyList<RecoveryReplacementCandidate> BoundarySunQuanMobileRecoveryCandidates(long parentFrameId,
        int sourceSeat, int targetSeat, RecoveryAttemptCompletion completion, IReadOnlyList<RecoveryReplacementCandidate> ordinary)
    {
        var retained = ordinary.Where(candidate => candidate.SkillId != BoundarySunQuanMobileJiuyuan);
        if (!IsBoundarySunQuanMobileSelfPeach(parentFrameId, sourceSeat, targetSeat, completion) || _winner != Winner.None || _turnNumber <= 0 ||
            !_players[targetSeat].IsAlive || !_players[targetSeat].HasHpValue || !HappyQiaoAnyWounded(targetSeat)) return Array.AsReadOnly(retained.ToArray());
        var provider = _players[targetSeat];
        var thresholds = provider.HappyQiaoHealth is { } dual ? new[] { dual.DaQiaoHp, dual.XiaoQiaoHp } : new[] { provider.Hp };
        var offered = _players.Where(owner => owner.Seat != targetSeat && owner.IsAlive && owner.HasHpValue && thresholds.Any(hp =>
                owner.HappyQiaoHealth is { } other ? other.DaQiaoHp < hp || other.XiaoQiaoHp < hp : owner.Hp < hp))
            .OrderBy(owner => RelativeTableOrder(targetSeat, owner.Seat))
            .SelectMany(owner => CardPolicies(owner, SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery)
                .Where(item => item.Source.SkillId == BoundarySunQuanMobileJiuyuan && item.Policy.Id == BoundarySunQuanMobilePolicy &&
                    item.Policy.FactionId == GetEffectiveFactionId(provider))
                .Select(item => new RecoveryReplacementCandidate(owner.Seat, item.Source.SkillId, item.Source.SkillInstanceId,
                    item.Policy.Id, item.Policy.Value, item.Policy.ProviderDrawCount)));
        return Array.AsReadOnly(retained.Concat(offered).GroupBy(candidate =>
            (candidate.OwnerSeat, candidate.SkillId, candidate.PolicyId)).Select(group => group.First()).ToArray());
    }

    private bool IsBoundarySunQuanMobileSelfPeach(long parentFrameId, int sourceSeat, int targetSeat, RecoveryAttemptCompletion completion) =>
        sourceSeat == targetSeat && IsValidPlayerSeat(sourceSeat) && completion.Producer is (RecoveryAttemptProducer.CardUse or RecoveryAttemptProducer.VirtualBasic) &&
        LifecycleCardUse(parentFrameId) is { SourceSeat: var actor, CardKind: CardKind.Peach, Action: { Type: CardActionType.Use, EffectiveKind: CardKind.Peach } action } &&
        actor == sourceSeat && action.ActorSeat == sourceSeat;

    private bool IsBoundarySunQuanMobileRecoveryCandidateCurrent(RecoveryReplacementFrame frame, RecoveryReplacementCandidate candidate)
    {
        var attempt = frame.Attempt;
        if (candidate.SkillId != BoundarySunQuanMobileJiuyuan || candidate.PolicyId != BoundarySunQuanMobilePolicy || _winner != Winner.None ||
            !IsValidPlayerSeat(candidate.OwnerSeat) || !IsValidPlayerSeat(attempt.TargetSeat) || candidate.OwnerSeat == attempt.TargetSeat ||
            !_players[candidate.OwnerSeat].IsAlive || !_players[attempt.TargetSeat].IsAlive ||
            !_players[candidate.OwnerSeat].HasHpValue || !_players[attempt.TargetSeat].HasHpValue ||
            attempt.ActualTurnNumber != _turnNumber || attempt.ActualTurnOwnerSeat != _currentSeat ||
            _players[candidate.OwnerSeat].Hp >= HappyQiaoRecoveryHpBefore(frame) ||
            !IsBoundarySunQuanMobileSelfPeach(frame.ParentFrameId, attempt.SourceSeat, attempt.TargetSeat, attempt.Completion)) return false;
        return CardPolicies(_players[candidate.OwnerSeat], SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery).Any(item =>
            item.Source.SkillId == candidate.SkillId && item.Source.SkillInstanceId == candidate.SkillInstanceId && item.Policy.Id == candidate.PolicyId &&
            item.Policy.FactionId == GetEffectiveFactionId(_players[attempt.TargetSeat]) && item.Policy.Value == candidate.RecoveryAmount &&
            item.Policy.ProviderDrawCount == candidate.ProviderDrawCount);
    }
}

internal static class BoundarySunQuanMobileComposition
{
    [ProgramCatalogValidation(59209)]
    private static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "boundary:jiuyuan-mobile") return;
        if (program.Activations.Count != 0 || program.Triggers.Count != 0 || program.CardPolicies is not [
                { Id: "redirect-self-peach", Kind: SkillProgramCardPolicyKind.RedirectOwnTurnFactionRecovery, FactionId: "wu", OwnerRole: Role.Lord,
                    Value: 1, ProviderDrawCount: 1, DiscardCost: 0, CardKinds.Count: 0, RequiredCardKinds.Count: 0, Condition.Kind: SkillProgramConditionKind.Always }])
            throw new InvalidOperationException(path + ": mobile Jiuyuan requires its native self-Peach recovery replacement with one recovery and one provider draw.");
    }
}
