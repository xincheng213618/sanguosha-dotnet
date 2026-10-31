#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed partial class CoreGameEngineDelayedNullificationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _resolve;
    private CoreGameEngineDelayedNullificationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDelayedNullificationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> resolve) => new(resolve);
}
