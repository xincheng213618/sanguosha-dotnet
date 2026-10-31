#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalizedCardTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> _resolve;
    private CoreGameEngineFinalizedCardTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalizedCardTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> resolve) => new(resolve);
}
