#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime))]
internal sealed partial class CoreGameEnginePaidColorConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> _resolve;
    private CoreGameEnginePaidColorConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidColorConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> resolve) => new(resolve);
}
