#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime))]
internal sealed partial class CoreGameEngineUnequalHandHpEndingEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime> _resolve;
    private CoreGameEngineUnequalHandHpEndingEntries(Func<EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnequalHandHpEndingEntries Create(Func<EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime> resolve) => new(resolve);
}
