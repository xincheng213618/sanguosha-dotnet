#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePhaseHandSeizureObserversCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICorePhaseHandSeizureObserversCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePhaseHandSeizureObserversResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePhaseHandSeizureObserversCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICorePhaseHandSeizureObserversCorePhaseHandSeizureReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidPhaseHandSeizureReceipt(global::CardGame.Core.ProgramSkillFrame frame);
    bool ValidPhaseHandDebtReturnReceipt(global::CardGame.Core.ProgramSkillFrame frame);
    bool IsPhaseHandSeizureMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending);
}

internal interface ICorePhaseHandSeizureObserversCoreDyingSuitsAndHistoricalUseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}
