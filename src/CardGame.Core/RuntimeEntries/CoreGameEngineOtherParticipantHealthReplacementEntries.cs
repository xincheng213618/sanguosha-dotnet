#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime))]
internal sealed partial class CoreGameEngineOtherParticipantHealthReplacementEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime> _resolve;
    private CoreGameEngineOtherParticipantHealthReplacementEntries(Func<EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOtherParticipantHealthReplacementEntries Create(Func<EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime> resolve) => new(resolve);
}
