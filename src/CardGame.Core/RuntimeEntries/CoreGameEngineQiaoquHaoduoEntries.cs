#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineQiaoquHaoduoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQiaoquHaoduoRuntime> _resolve;
    private CoreGameEngineQiaoquHaoduoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQiaoquHaoduoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineQiaoquHaoduoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQiaoquHaoduoRuntime> resolve) => new(resolve);
}
