#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsRuntime
{

    internal bool IsCurrentTurnSkillGrantQualified(CharacterState owner, SkillGrant grant) => _skillBindingIndexOperations._skillBindingIndex.Availability.IsGrantEffectiveUnderStates(owner, grant) && _coreShenwuExpandedPoliciesOperations.ShenwuFengdongQualifiesGrant(owner, grant) && _coreDyingWindowSkillPoliciesOperations.IsDyingWindowGrantQualified(owner, grant) && _characterZouShiLeOperations.YunzhengNonLockedGrantQualified(owner, grant) && _characterZhangLiaoMobileGodOperations.IsMobileDuoruiGrantQualified(owner, grant);
    internal long CaptureCurrentTurnQualificationStamp()
    {
        // Upstream owners affect derived grants only. Their separate shard stamp
        // avoids scanning the whole table for every ordinary policy lookup.
        var projection = _coreShenwuExpandedPoliciesOperations.CaptureShenwuFengdongStamp();
        if (_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision == 0)
            return _characterZouShiLeOperations.CaptureYunzhengQualificationStamp(_coreGuanduActivitiesOperations.CaptureGuanduQualificationStamp(_coreNativeFactionChangesOperations.CaptureNativeFactionQualificationStamp(_coreDyingWindowSkillPoliciesOperations.CaptureDyingWindowQualificationStamp(projection))));
        var current = (projection, _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision);
        if (current != _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationDependencies)
        {
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationDependencies = current;
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationStamp++;
        }

        // Disjoint from the nonnegative legacy projection stamp.
        return _characterZouShiLeOperations.CaptureYunzhengQualificationStamp(_coreGuanduActivitiesOperations.CaptureGuanduQualificationStamp(_coreNativeFactionChangesOperations.CaptureNativeFactionQualificationStamp(_coreDyingWindowSkillPoliciesOperations.CaptureDyingWindowQualificationStamp(-_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnQualificationStamp))));
    }

}
