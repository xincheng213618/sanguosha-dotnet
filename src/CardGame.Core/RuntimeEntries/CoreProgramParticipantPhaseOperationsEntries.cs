#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramParticipantPhaseOperationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _resolve;
    private CoreProgramParticipantPhaseOperationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramParticipantPhaseOperationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> resolve) => new(resolve);
}
