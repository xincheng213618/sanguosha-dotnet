#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime))]
internal sealed partial class CoreGameEngineGuanduBasicConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime> _resolve;
    private CoreGameEngineGuanduBasicConversionsEntries(Func<EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduBasicConversionsEntries Create(Func<EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime> resolve) => new(resolve);
}
