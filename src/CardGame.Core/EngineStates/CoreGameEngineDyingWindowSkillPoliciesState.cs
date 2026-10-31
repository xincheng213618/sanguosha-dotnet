#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDyingWindowSkillPoliciesState : Contracts.ICoreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess
{
    private (long Base, long Frame, int Victim, int Turn, int TurnOwner, long SourceRevision, int SourceHp, bool SourceAlive) _dyingQualificationDependencies = default !;
    internal ref (long Base, long Frame, int Victim, int Turn, int TurnOwner, long SourceRevision, int SourceHp, bool SourceAlive) ReferenceToDyingQualificationDependencies => ref _dyingQualificationDependencies;

    private long _dyingQualificationStamp = default !;
    internal ref long ReferenceToDyingQualificationStamp => ref _dyingQualificationStamp;

    ref long Contracts.ICoreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess._dyingQualificationStamp => ref _dyingQualificationStamp;

    ref (long Base, long Frame, int Victim, int Turn, int TurnOwner, long SourceRevision, int SourceHp, bool SourceAlive) Contracts.ICoreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess._dyingQualificationDependencies => ref _dyingQualificationDependencies;
}
