#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineModeSetupRuntime))]
internal sealed partial class CoreGameEngineModeSetupEntries
{
    private readonly Func<EngineOwners.CoreGameEngineModeSetupRuntime> _resolve;
    private CoreGameEngineModeSetupEntries(Func<EngineOwners.CoreGameEngineModeSetupRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeSetupEntries Create(Func<EngineOwners.CoreGameEngineModeSetupRuntime> resolve) => new(resolve);
}
