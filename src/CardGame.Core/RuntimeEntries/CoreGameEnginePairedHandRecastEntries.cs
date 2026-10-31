#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedHandRecastEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastRuntime> _resolve;
    private CoreGameEnginePairedHandRecastEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedHandRecastEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastRuntime> resolve) => new(resolve);
}
