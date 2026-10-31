#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsRuntime
{

    internal bool IsCurrentTurnSkillGrantQualified(CharacterState owner, SkillGrant grant) => _skillBindingIndexOperations._skillBindingIndex.Availability.IsGrantEffectiveUnderStates(owner, grant) && _characterZouShiLeOperations.YunzhengNonLockedGrantQualified(owner, grant);
    internal long CaptureCurrentTurnQualificationStamp()
    {
        return _characterZouShiLeOperations.CaptureYunzhengQualificationStamp(_coreGuanduActivitiesOperations.CaptureGuanduQualificationStamp(_coreNativeFactionChangesOperations.CaptureNativeFactionQualificationStamp(_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision)));
    }

}
