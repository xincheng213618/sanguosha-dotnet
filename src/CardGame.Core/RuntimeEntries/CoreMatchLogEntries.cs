#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreMatchLogRuntime))]
internal sealed partial class CoreMatchLogEntries
{
    private readonly Func<EngineOwners.CoreMatchLogRuntime> _resolve;
    private CoreMatchLogEntries(Func<EngineOwners.CoreMatchLogRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchLogEntries Create(Func<EngineOwners.CoreMatchLogRuntime> resolve) => new(resolve);
}
