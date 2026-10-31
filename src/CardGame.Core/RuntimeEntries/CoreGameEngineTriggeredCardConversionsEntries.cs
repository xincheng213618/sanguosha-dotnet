#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime))]
internal sealed partial class CoreGameEngineTriggeredCardConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime> _resolve;
    private CoreGameEngineTriggeredCardConversionsEntries(Func<EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTriggeredCardConversionsEntries Create(Func<EngineOwners.CoreGameEngineTriggeredCardConversionsRuntime> resolve) => new(resolve);
}
