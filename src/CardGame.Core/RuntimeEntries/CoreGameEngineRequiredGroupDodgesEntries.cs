#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequiredGroupDodgesRuntime))]
internal sealed partial class CoreGameEngineRequiredGroupDodgesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequiredGroupDodgesRuntime> _resolve;
    private CoreGameEngineRequiredGroupDodgesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequiredGroupDodgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRequiredGroupDodgesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequiredGroupDodgesRuntime> resolve) => new(resolve);
}
