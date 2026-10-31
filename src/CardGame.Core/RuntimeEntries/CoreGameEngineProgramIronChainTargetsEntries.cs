#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime))]
internal sealed partial class CoreGameEngineProgramIronChainTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> _resolve;
    private CoreGameEngineProgramIronChainTargetsEntries(Func<EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramIronChainTargetsEntries Create(Func<EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> resolve) => new(resolve);
}
