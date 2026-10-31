#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramOwnedZoneDiscardRuntimePortsRuntime
{
    bool global::CardGame.Core.IProgramOwnedZoneDiscardFacts.HasAtomicDiscardReplacement => _coreHellMengpoProgramsOperations.HasHellShiyouCapability;

    CardLocation? global::CardGame.Core.IProgramOwnedZoneDiscardFacts.DiscardSource(CardMovementRecord movement) => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement);
    IProgramOwnedZoneDiscardInvocation? global::CardGame.Core.IProgramOwnedZoneDiscardReceipts.Capture(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _characterBuZhiOlOperations.CaptureNativeBuZhiDiscardInvocation(frame, zones, reason);
    void global::CardGame.Core.IProgramOwnedZoneDiscardReceipts.Complete(IProgramOwnedZoneDiscardInvocation? invocation, IReadOnlyList<int> actualIds) => _characterBuZhiOlOperations.CompleteNativeBuZhiDiscardInvocation((global::CardGame.Core.GameEngine.NativeBuZhiDiscardInvocationDraft? )invocation, actualIds);
}
