#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref long _dyingQualificationStamp { get; }

    ref (long Base, long Frame, int Victim, int Turn, int TurnOwner, long SourceRevision, int SourceHp, bool SourceAlive) _dyingQualificationDependencies { get; }
}
