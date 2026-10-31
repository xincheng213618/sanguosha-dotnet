#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime))]
internal sealed partial class CoreGameEngineRequestedDeckBasicOwnershipEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> _resolve;
    private CoreGameEngineRequestedDeckBasicOwnershipEntries(Func<EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRequestedDeckBasicOwnershipEntries Create(Func<EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime> resolve) => new(resolve);
}
