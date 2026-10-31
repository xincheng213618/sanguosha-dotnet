#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeFactionDefenseRuntime))]
internal sealed partial class CoreNativeFactionDefenseEntries
{
    private readonly Func<EngineOwners.CoreNativeFactionDefenseRuntime> _resolve;
    private CoreNativeFactionDefenseEntries(Func<EngineOwners.CoreNativeFactionDefenseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFactionDefenseEntries Create(Func<EngineOwners.CoreNativeFactionDefenseRuntime> resolve) => new(resolve);
}
