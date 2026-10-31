#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime))]
internal sealed partial class CoreGameEngineUnequalHandHpEndingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime> _resolve;
    private CoreGameEngineUnequalHandHpEndingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnequalHandHpEndingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime> resolve) => new(resolve);
}
