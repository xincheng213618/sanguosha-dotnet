#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentTurnSkillSuppressionsRuntime
{

    internal long CaptureCurrentTurnQualificationStamp()
    {
        return _coreGuanduActivitiesOperations.CaptureGuanduQualificationStamp(_coreNativeFactionChangesOperations.CaptureNativeFactionQualificationStamp(_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision));
    }

}
