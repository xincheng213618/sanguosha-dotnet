#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreMatchParticipantSetupRuntime))]
internal sealed partial class CoreMatchParticipantSetupEntries
{
    private readonly Func<EngineOwners.CoreMatchParticipantSetupRuntime> _resolve;
    private CoreMatchParticipantSetupEntries(Func<EngineOwners.CoreMatchParticipantSetupRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchParticipantSetupEntries Create(Func<EngineOwners.CoreMatchParticipantSetupRuntime> resolve) => new(resolve);
}
