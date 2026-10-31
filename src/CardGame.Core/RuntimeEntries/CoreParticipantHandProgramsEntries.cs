#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreParticipantHandProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> _resolve;
    private CoreParticipantHandProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreParticipantHandProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramsRuntime> resolve) => new(resolve);
}
