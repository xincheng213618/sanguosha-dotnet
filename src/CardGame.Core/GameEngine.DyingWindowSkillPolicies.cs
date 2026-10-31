namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref (long Base, long Frame, int Victim, int Turn, int TurnOwner, long SourceRevision, int SourceHp, bool SourceAlive) _dyingQualificationDependencies => ref _dyingWindowSkillPolicies.ReferenceToDyingQualificationDependencies;
    private ref long _dyingQualificationStamp => ref _dyingWindowSkillPolicies.ReferenceToDyingQualificationStamp;
}
