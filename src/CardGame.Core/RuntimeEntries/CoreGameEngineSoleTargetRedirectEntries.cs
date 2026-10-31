#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSoleTargetRedirectRuntime))]
internal sealed partial class CoreGameEngineSoleTargetRedirectEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> _resolve;
    private CoreGameEngineSoleTargetRedirectEntries(Func<EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetRedirectEntries Create(Func<EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> resolve) => new(resolve);
}
