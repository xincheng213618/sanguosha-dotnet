#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime))]
internal sealed partial class CoreGameEngineProgramIronChainTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> _resolve;
    private CoreGameEngineProgramIronChainTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramIronChainTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> resolve) => new(resolve);
}
