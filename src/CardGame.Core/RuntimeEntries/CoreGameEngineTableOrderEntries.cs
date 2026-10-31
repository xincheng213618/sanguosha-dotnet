#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed partial class CoreGameEngineTableOrderEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTableOrderRuntime> _resolve;
    private CoreGameEngineTableOrderEntries(Func<EngineOwners.CoreGameEngineTableOrderRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTableOrderEntries Create(Func<EngineOwners.CoreGameEngineTableOrderRuntime> resolve) => new(resolve);
}
