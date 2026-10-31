#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramRevealsRuntime))]
internal sealed partial class CoreGameEngineProgramRevealsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramRevealsRuntime> _resolve;
    private CoreGameEngineProgramRevealsEntries(Func<EngineOwners.CoreGameEngineProgramRevealsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRevealsEntries Create(Func<EngineOwners.CoreGameEngineProgramRevealsRuntime> resolve) => new(resolve);
}
