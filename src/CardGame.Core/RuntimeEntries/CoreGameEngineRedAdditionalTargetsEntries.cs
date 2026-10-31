#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRedAdditionalTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> _resolve;
    private CoreGameEngineRedAdditionalTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRedAdditionalTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> resolve) => new(resolve);
}
