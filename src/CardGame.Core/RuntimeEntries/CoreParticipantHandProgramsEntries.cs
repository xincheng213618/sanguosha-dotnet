#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreParticipantHandProgramsRuntime))]
internal sealed partial class CoreParticipantHandProgramsEntries
{
    private readonly Func<EngineOwners.CoreParticipantHandProgramsRuntime> _resolve;
    private CoreParticipantHandProgramsEntries(Func<EngineOwners.CoreParticipantHandProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreParticipantHandProgramsEntries Create(Func<EngineOwners.CoreParticipantHandProgramsRuntime> resolve) => new(resolve);
}
