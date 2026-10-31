#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramChildResolutionRuntime
{
    internal bool ProgramChildResolutionEdge(int index)
    {
        if (index < 1 || index >= _resolutionStackOperations._resolutionStack.Count)
            return false;
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        if (parent is ProgramSkillFrame && child is CardsMovedTriggerWindowFrame or HpChangedTriggerWindowFrame || child is ProgramSkillFrame && parent is CardsMovedTriggerWindowFrame or HpChangedTriggerWindowFrame)
            return _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child) && (child is not CardsMovedTriggerWindowFrame movement || movement.Batch.Movements.All(_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains));
        return _resolutionStackOperations._resolutionStack.HasIssuedLink(index);
    }
}
