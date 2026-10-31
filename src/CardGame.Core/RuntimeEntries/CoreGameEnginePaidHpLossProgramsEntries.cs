#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidHpLossProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> _resolve;
    private CoreGameEnginePaidHpLossProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidHpLossProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> resolve) => new(resolve);
}
