#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime))]
internal sealed partial class CoreGameEngineSoleTargetRedirectEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> _resolve;
    private CoreGameEngineSoleTargetRedirectEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetRedirectEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> resolve) => new(resolve);
}
