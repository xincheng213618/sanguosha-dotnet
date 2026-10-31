#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed partial class CoreGameEngineTableOrderEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _resolve;
    private CoreGameEngineTableOrderEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTableOrderEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> resolve) => new(resolve);
}
