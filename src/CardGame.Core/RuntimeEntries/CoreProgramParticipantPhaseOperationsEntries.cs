#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed partial class CoreProgramParticipantPhaseOperationsEntries
{
    private readonly Func<EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _resolve;
    private CoreProgramParticipantPhaseOperationsEntries(Func<EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramParticipantPhaseOperationsEntries Create(Func<EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> resolve) => new(resolve);
}
