#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramOwnedZoneDiscardRuntimePortsRuntime
{
    bool IProgramOwnedZoneDiscardFacts.HasAtomicDiscardReplacement => _coreHellMengpoProgramsOperations.HasHellShiyouCapability;

    CardLocation? IProgramOwnedZoneDiscardFacts.DiscardSource(CardMovementRecord movement) => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement);
    IProgramOwnedZoneDiscardInvocation? IProgramOwnedZoneDiscardReceipts.Capture(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _characterBuZhiOlOperations.CaptureNativeBuZhiDiscardInvocation(frame, zones, reason);
    void IProgramOwnedZoneDiscardReceipts.Complete(IProgramOwnedZoneDiscardInvocation? invocation, IReadOnlyList<int> actualIds) => _characterBuZhiOlOperations.CompleteNativeBuZhiDiscardInvocation((GameEngine.NativeBuZhiDiscardInvocationDraft? )invocation, actualIds);
}
