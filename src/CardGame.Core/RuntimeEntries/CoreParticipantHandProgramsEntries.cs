#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime))]
internal sealed partial class CoreParticipantHandProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> _resolve;
    private CoreParticipantHandProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreParticipantHandProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> resolve) => new(resolve);
}
