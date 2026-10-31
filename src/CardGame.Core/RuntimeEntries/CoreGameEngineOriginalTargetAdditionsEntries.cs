#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalTargetAdditionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> _resolve;
    private CoreGameEngineOriginalTargetAdditionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalTargetAdditionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalTargetAdditionsRuntime> resolve) => new(resolve);
}
