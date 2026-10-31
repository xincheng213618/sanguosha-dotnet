#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramIronChainTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> _resolve;
    private CoreGameEngineProgramIronChainTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramIronChainTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> resolve) => new(resolve);
}
