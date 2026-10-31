#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime))]
internal sealed partial class CoreGameEngineOtherParticipantHealthReplacementEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime> _resolve;
    private CoreGameEngineOtherParticipantHealthReplacementEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOtherParticipantHealthReplacementEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOtherParticipantHealthReplacementRuntime> resolve) => new(resolve);
}
