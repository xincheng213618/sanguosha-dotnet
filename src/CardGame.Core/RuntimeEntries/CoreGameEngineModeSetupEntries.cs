#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupRuntime))]
internal sealed partial class CoreGameEngineModeSetupEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupRuntime> _resolve;
    private CoreGameEngineModeSetupEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeSetupEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupRuntime> resolve) => new(resolve);
}
