#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsRuntime
{
    internal void IssueCurrentTurnNonLockedSkillSuppression(ProgramSkillFrame frame, int targetSeat)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var issued = _programPipelineHooksOperations._programPipelineHooks.AdmitTurnSuppression(frame, targetSeat);
        if (!ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == frame.OwnerSeat && !issued || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !issued && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            throw new InvalidOperationException("Suppression requires its actual owned program frame and living other target.");
        var fact = new CurrentTurnNonLockedSkillSuppression(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), targetSeat);
        var existing = _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressions.SingleOrDefault(s => s.ParentFrameId == fact.ParentFrameId && s.EffectIndex == fact.EffectIndex && s.TargetSeat == targetSeat);
        if (existing is not null)
        {
            if (existing != fact)
                throw new InvalidOperationException("Suppression issue identity changed meaning.");
            return;
        }

        _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressions.Add(fact);
        _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentTurnNonLockedSkillSuppressionIssuedEvent(fact));
    }

    internal bool IsCurrentTurnSkillGrantQualified(CharacterState owner, SkillGrant grant) => _coreShenwuExpandedPoliciesOperations.ShenwuFengdongQualifiesGrant(owner, grant) && _characterCaoPiMouOperations.MouCaoPiSkillsQualified(owner, grant) && _coreAlcoholSlashSuppressionsOperations.IsCurrentTurnOwnSkillGrantQualified(owner, grant) && _coreWushuangOriginalSkillRulesOperations.WushuangYashiGrantQualified(owner, grant) && (!_characterZhongHuiAnniversaryGodOperations.IsLishiPhaseSuppressed(owner.Seat) || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked)) && (!_characterCaoPiAnniversaryGodOperations.IsYinzhiSkillSuppressed(owner.Seat) || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked)) && (!_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressions.Any(s => s.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && s.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && s.TargetSeat == owner.Seat) || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked)) && _coreDyingWindowSkillPoliciesOperations.IsDyingWindowGrantQualified(owner, grant) && _characterWangGuanOlOperations.IsMiuyanGrantQualified(owner, grant) && _characterZouShiLeOperations.YunzhengNonLockedGrantQualified(owner, grant) && _characterZhangLiaoMobileGodOperations.IsMobileDuoruiGrantQualified(owner, grant) && _characterMaChaoMobileGodOperations.IsMobileMaChaoTingweiGrantQualified(owner, grant) && _characterXiahouJieAnniversaryOperations.IsAnniversaryXiahouJieGrantQualified(owner, grant) && _characterCuiLieAnniversaryOperations.IsAnniversaryCuiLieGrantQualified(owner, grant) && _coreAnqiFeirenOperations.IsAnqiFeirenGrantQualified(owner, grant) && _characterZhangRenMobileShiOperations.ShiZrFuanGrantQualified(owner, grant);
    internal long CaptureCurrentTurnQualificationStamp()
    {
        // Upstream owners affect derived grants only. Their separate shard stamp
        // avoids scanning the whole table for every ordinary policy lookup.
        var projection = _coreShenwuExpandedPoliciesOperations.CaptureShenwuFengdongStamp();
        if (_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision == 0)
            return _characterCuiLieAnniversaryOperations.CaptureAnniversaryCuiLieQualificationStamp(_characterXiahouJieAnniversaryOperations.CaptureAnniversaryXiahouJieQualificationStamp(_characterZouShiLeOperations.CaptureYunzhengQualificationStamp(_coreGuanduActivitiesOperations.CaptureGuanduQualificationStamp(_coreNativeFactionChangesOperations.CaptureNativeFactionQualificationStamp(_characterWangGuanOlOperations.CaptureMiuyanQualificationStamp(_coreDyingWindowSkillPoliciesOperations.CaptureDyingWindowQualificationStamp(projection)))))));
        var current = (projection, _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision);
        if (current != _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationDependencies)
        {
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationDependencies = current;
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationStamp++;
        }

        // Disjoint from the nonnegative legacy projection stamp.
        return _characterCuiLieAnniversaryOperations.CaptureAnniversaryCuiLieQualificationStamp(_characterXiahouJieAnniversaryOperations.CaptureAnniversaryXiahouJieQualificationStamp(_characterZouShiLeOperations.CaptureYunzhengQualificationStamp(_coreGuanduActivitiesOperations.CaptureGuanduQualificationStamp(_coreNativeFactionChangesOperations.CaptureNativeFactionQualificationStamp(_characterWangGuanOlOperations.CaptureMiuyanQualificationStamp(_coreDyingWindowSkillPoliciesOperations.CaptureDyingWindowQualificationStamp(-_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationStamp)))))));
    }

    internal void ExpireCurrentTurnNonLockedSkillSuppressions(int turnNumber, int turnOwnerSeat)
    {
        if (_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressions.RemoveAll(s => s.TurnNumber == turnNumber && s.TurnOwnerSeat == turnOwnerSeat) == 0)
            return;
        _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentTurnNonLockedSkillSuppressionsExpiredEvent(turnNumber, turnOwnerSeat));
    }
}
