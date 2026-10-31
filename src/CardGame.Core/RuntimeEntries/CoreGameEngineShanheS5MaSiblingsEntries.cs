#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5MaSiblingsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5MaSiblingsRuntime> _resolve;
    private CoreGameEngineShanheS5MaSiblingsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5MaSiblingsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5MaSiblingsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5MaSiblingsRuntime> resolve) => new(resolve);
}
