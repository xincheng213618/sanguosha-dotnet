#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsRuntime
{

    internal bool IsCurrentTurnSkillGrantQualified(CharacterState owner, SkillGrant grant) => SkillAvailability.IsEffectiveUnderRestrictions(owner, grant, _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId)) && _coreShenwuExpandedPoliciesOperations.ShenwuFengdongQualifiesGrant(owner, grant) && _characterCaoPiMouOperations.MouCaoPiSkillsQualified(owner, grant) && _coreWushuangOriginalSkillRulesOperations.Value.WushuangYashiGrantQualified(owner, grant) && (!_characterZhongHuiAnniversaryGodOperations.IsLishiPhaseSuppressed(owner.Seat) || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked)) && (!_characterCaoPiAnniversaryGodOperations.IsYinzhiSkillSuppressed(owner.Seat) || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked)) && _coreDyingWindowSkillPoliciesOperations.IsDyingWindowGrantQualified(owner, grant) && _characterWangGuanOlOperations.IsMiuyanGrantQualified(owner, grant) && _characterZouShiLeOperations.YunzhengNonLockedGrantQualified(owner, grant) && _characterZhangLiaoMobileGodOperations.IsMobileDuoruiGrantQualified(owner, grant);
    internal long CaptureCurrentTurnQualificationStamp()
    {
        // Upstream owners affect derived grants only. Their separate shard stamp
        // avoids scanning the whole table for every ordinary policy lookup.
        var projection = _coreShenwuExpandedPoliciesOperations.CaptureShenwuFengdongStamp();
        if (_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision == 0)
            return _characterZouShiLeOperations.CaptureYunzhengQualificationStamp(_coreGuanduActivitiesOperations.CaptureGuanduQualificationStamp(_coreNativeFactionChangesOperations.CaptureNativeFactionQualificationStamp(_characterWangGuanOlOperations.CaptureMiuyanQualificationStamp(_coreDyingWindowSkillPoliciesOperations.CaptureDyingWindowQualificationStamp(projection)))));
        var current = (projection, _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision);
        if (current != _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationDependencies)
        {
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationDependencies = current;
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationStamp++;
        }

        // Disjoint from the nonnegative legacy projection stamp.
        return _characterZouShiLeOperations.CaptureYunzhengQualificationStamp(_coreGuanduActivitiesOperations.CaptureGuanduQualificationStamp(_coreNativeFactionChangesOperations.CaptureNativeFactionQualificationStamp(_characterWangGuanOlOperations.CaptureMiuyanQualificationStamp(_coreDyingWindowSkillPoliciesOperations.CaptureDyingWindowQualificationStamp(-_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationStamp)))));
    }

}
