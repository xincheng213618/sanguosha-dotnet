#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSimultaneousPindianRuntime))]
internal sealed partial class CoreGameEngineSimultaneousPindianEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSimultaneousPindianRuntime> _resolve;
    private CoreGameEngineSimultaneousPindianEntries(Func<EngineOwners.CoreGameEngineSimultaneousPindianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSimultaneousPindianEntries Create(Func<EngineOwners.CoreGameEngineSimultaneousPindianRuntime> resolve) => new(resolve);
}
