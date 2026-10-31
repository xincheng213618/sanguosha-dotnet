#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime))]
internal sealed partial class CoreGameEngineTriggeredCardConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime> _resolve;
    private CoreGameEngineTriggeredCardConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTriggeredCardConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime> resolve) => new(resolve);
}
