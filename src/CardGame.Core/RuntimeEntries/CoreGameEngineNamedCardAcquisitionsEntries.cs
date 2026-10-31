#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime))]
internal sealed partial class CoreGameEngineNamedCardAcquisitionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime> _resolve;
    private CoreGameEngineNamedCardAcquisitionsEntries(Func<EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedCardAcquisitionsEntries Create(Func<EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime> resolve) => new(resolve);
}
