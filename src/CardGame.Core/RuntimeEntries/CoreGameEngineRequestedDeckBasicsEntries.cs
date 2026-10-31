#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed partial class CoreGameEngineRequestedDeckBasicsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _resolve;
    private CoreGameEngineRequestedDeckBasicsEntries(Func<EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRequestedDeckBasicsEntries Create(Func<EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> resolve) => new(resolve);
}
