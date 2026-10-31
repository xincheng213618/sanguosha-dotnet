#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineParticipantPreventionRuntime))]
internal sealed partial class CoreGameEngineParticipantPreventionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineParticipantPreventionRuntime> _resolve;
    private CoreGameEngineParticipantPreventionEntries(Func<EngineOwners.CoreGameEngineParticipantPreventionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineParticipantPreventionEntries Create(Func<EngineOwners.CoreGameEngineParticipantPreventionRuntime> resolve) => new(resolve);
}
