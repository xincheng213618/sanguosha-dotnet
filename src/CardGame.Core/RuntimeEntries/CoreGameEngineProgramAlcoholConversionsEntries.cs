#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAlcoholConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime> _resolve;
    private CoreGameEngineProgramAlcoholConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAlcoholConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAlcoholConversionsRuntime> resolve) => new(resolve);
}
