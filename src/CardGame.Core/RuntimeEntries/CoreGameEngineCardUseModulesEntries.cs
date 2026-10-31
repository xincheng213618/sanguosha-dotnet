#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed partial class CoreGameEngineCardUseModulesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardUseModulesRuntime> _resolve;
    private CoreGameEngineCardUseModulesEntries(Func<EngineOwners.CoreGameEngineCardUseModulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUseModulesEntries Create(Func<EngineOwners.CoreGameEngineCardUseModulesRuntime> resolve) => new(resolve);
}
