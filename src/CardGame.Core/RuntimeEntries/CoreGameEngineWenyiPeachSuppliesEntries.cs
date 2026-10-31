#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime))]
internal sealed partial class CoreGameEngineWenyiPeachSuppliesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> _resolve;
    private CoreGameEngineWenyiPeachSuppliesEntries(Func<EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenyiPeachSuppliesEntries Create(Func<EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> resolve) => new(resolve);
}
