namespace CardGame.Core;
public sealed record LordSkillProjectionChangedEvent(int OwnerSeat, string GrantId, string SkillId, LordSkillProjectionSource Source, bool Added) : IGameEvent;
public sealed partial class GameEngine
{
    private ref bool _synchronizingLordProjections => ref _lordSkillProjections.ReferenceToSynchronizingLordProjections;

    internal readonly record struct ProjectionDependency(long Revision, bool Alive, int Hp, Role Role, bool PrimarySelected, bool PrimaryRevealed, bool SecondarySelected, bool SecondaryRevealed);
    private ref global::CardGame.Core.GameEngine.ProjectionDependency[] _lordProjectionDependencies => ref _lordSkillProjections.ReferenceToLordProjectionDependencies;
    private ref long _lordProjectionDependencyRevision => ref _lordSkillProjections.ReferenceToLordProjectionDependencyRevision;
}
