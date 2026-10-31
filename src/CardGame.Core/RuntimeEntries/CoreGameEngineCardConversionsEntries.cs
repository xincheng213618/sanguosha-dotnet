#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed partial class CoreGameEngineCardConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardConversionsRuntime> _resolve;
    private CoreGameEngineCardConversionsEntries(Func<EngineOwners.CoreGameEngineCardConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardConversionsEntries Create(Func<EngineOwners.CoreGameEngineCardConversionsRuntime> resolve) => new(resolve);
}
