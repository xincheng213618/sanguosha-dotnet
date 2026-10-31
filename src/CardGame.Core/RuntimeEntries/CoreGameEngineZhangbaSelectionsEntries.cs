#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineZhangbaSelectionsRuntime))]
internal sealed partial class CoreGameEngineZhangbaSelectionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineZhangbaSelectionsRuntime> _resolve;
    private CoreGameEngineZhangbaSelectionsEntries(Func<EngineOwners.CoreGameEngineZhangbaSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineZhangbaSelectionsEntries Create(Func<EngineOwners.CoreGameEngineZhangbaSelectionsRuntime> resolve) => new(resolve);
}
