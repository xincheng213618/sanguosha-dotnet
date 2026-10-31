#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnTrickDrawTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> _resolve;
    private CoreGameEngineOwnTrickDrawTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnTrickDrawTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawTargetsRuntime> resolve) => new(resolve);
}
