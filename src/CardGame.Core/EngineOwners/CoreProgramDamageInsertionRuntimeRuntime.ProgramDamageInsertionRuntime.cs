#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDamageInsertionRuntimeRuntime
{
    internal ProgramDamageInsertionRuntime DamageInsertions => _programDamageInsertionOperations.Get();

    int IProgramDamageInsertionFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame IProgramDamageInsertionFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int IProgramDamageInsertionFacts.FrameIndex(long frameId) => _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == frameId);
    bool IProgramDamageInsertionFacts.HasIssuedLink(int index) => _resolutionStackOperations._resolutionStack.HasIssuedLink(index);
    bool IProgramDamageInsertionFacts.HasConsistentChild(int index) => _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index);
    long? IProgramDamageInsertionFacts.AttackOwnerFrameId => _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId;

    long? IProgramDamageInsertionFacts.DyingFrameId => _coreDyingCursorRuntimeOperations.ActiveDying?.Id;

    internal bool IsOriginalDyingSuspendedByProgramInsertion(DyingFrame dying) => DamageInsertions.IsSuspended(dying);
}
