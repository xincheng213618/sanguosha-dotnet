#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime))]
internal sealed partial class CoreGameEngineSimultaneousPindianSelectionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime> _resolve;
    private CoreGameEngineSimultaneousPindianSelectionsEntries(Func<EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSimultaneousPindianSelectionsEntries Create(Func<EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime> resolve) => new(resolve);
}
