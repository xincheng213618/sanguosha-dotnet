#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDamageInsertionRuntimeRuntime
{
    internal ProgramDamageInsertionRuntime DamageInsertions => _programDamageInsertionOperations.Get();

    int global::CardGame.Core.IProgramDamageInsertionFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame global::CardGame.Core.IProgramDamageInsertionFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int global::CardGame.Core.IProgramDamageInsertionFacts.FrameIndex(long frameId) => _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == frameId);
    bool global::CardGame.Core.IProgramDamageInsertionFacts.HasIssuedLink(int index) => _resolutionStackOperations._resolutionStack.HasIssuedLink(index);
    bool global::CardGame.Core.IProgramDamageInsertionFacts.HasConsistentChild(int index) => _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index);
    long? global::CardGame.Core.IProgramDamageInsertionFacts.AttackOwnerFrameId => _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId;

    long? global::CardGame.Core.IProgramDamageInsertionFacts.DyingFrameId => _coreDyingCursorRuntimeOperations.ActiveDying?.Id;

    internal bool IsOriginalDyingSuspendedByProgramInsertion(DyingFrame dying) => DamageInsertions.IsSuspended(dying);
}
