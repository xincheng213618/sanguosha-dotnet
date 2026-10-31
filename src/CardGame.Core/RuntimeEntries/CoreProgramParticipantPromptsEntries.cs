#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramParticipantPromptsRuntime))]
internal sealed partial class CoreProgramParticipantPromptsEntries
{
    private readonly Func<EngineOwners.CoreProgramParticipantPromptsRuntime> _resolve;
    private CoreProgramParticipantPromptsEntries(Func<EngineOwners.CoreProgramParticipantPromptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramParticipantPromptsEntries Create(Func<EngineOwners.CoreProgramParticipantPromptsRuntime> resolve) => new(resolve);
}
