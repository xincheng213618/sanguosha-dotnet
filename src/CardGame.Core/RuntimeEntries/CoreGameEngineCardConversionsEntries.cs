#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed partial class CoreGameEngineCardConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _resolve;
    private CoreGameEngineCardConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> resolve) => new(resolve);
}
