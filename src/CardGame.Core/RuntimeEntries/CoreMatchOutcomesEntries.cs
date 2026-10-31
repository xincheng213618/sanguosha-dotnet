#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreMatchOutcomesRuntime))]
internal sealed partial class CoreMatchOutcomesEntries
{
    private readonly Func<EngineOwners.CoreMatchOutcomesRuntime> _resolve;
    private CoreMatchOutcomesEntries(Func<EngineOwners.CoreMatchOutcomesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchOutcomesEntries Create(Func<EngineOwners.CoreMatchOutcomesRuntime> resolve) => new(resolve);
}
