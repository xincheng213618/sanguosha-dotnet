#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeTrickUseRuntime))]
internal sealed partial class CoreNativeTrickUseEntries
{
    private readonly Func<EngineOwners.CoreNativeTrickUseRuntime> _resolve;
    private CoreNativeTrickUseEntries(Func<EngineOwners.CoreNativeTrickUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeTrickUseEntries Create(Func<EngineOwners.CoreNativeTrickUseRuntime> resolve) => new(resolve);
}
