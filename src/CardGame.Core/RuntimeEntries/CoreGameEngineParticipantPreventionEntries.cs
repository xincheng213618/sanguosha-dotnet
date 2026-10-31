#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineParticipantPreventionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantPreventionRuntime> _resolve;
    private CoreGameEngineParticipantPreventionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantPreventionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineParticipantPreventionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantPreventionRuntime> resolve) => new(resolve);
}
