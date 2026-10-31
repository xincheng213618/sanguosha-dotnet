#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramOwnedZoneDiscardRuntimePortsCoreHellMengpoProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHellShiyouCapability { get; }
}

internal interface ICoreProgramOwnedZoneDiscardRuntimePortsCoreProgramDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreProgramOwnedZoneDiscardRuntimePortsCharacterBuZhiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.NativeBuZhiDiscardInvocationDraft? CaptureNativeBuZhiDiscardInvocation(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.CardMoveReason reason);
    void CompleteNativeBuZhiDiscardInvocation(global::CardGame.Core.GameEngine.NativeBuZhiDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<int> actualIds);
}
