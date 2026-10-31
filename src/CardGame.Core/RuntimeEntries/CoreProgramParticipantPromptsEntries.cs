#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime))]
internal sealed partial class CoreProgramParticipantPromptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> _resolve;
    private CoreProgramParticipantPromptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramParticipantPromptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPromptsRuntime> resolve) => new(resolve);
}
