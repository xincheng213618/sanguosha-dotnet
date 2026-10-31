#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime))]
internal sealed partial class CoreMatchParticipantSetupEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> _resolve;
    private CoreMatchParticipantSetupEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchParticipantSetupEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> resolve) => new(resolve);
}
