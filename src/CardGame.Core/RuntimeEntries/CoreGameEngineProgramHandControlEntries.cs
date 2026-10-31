#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramHandControlRuntime))]
internal sealed partial class CoreGameEngineProgramHandControlEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramHandControlRuntime> _resolve;
    private CoreGameEngineProgramHandControlEntries(Func<EngineOwners.CoreGameEngineProgramHandControlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramHandControlEntries Create(Func<EngineOwners.CoreGameEngineProgramHandControlRuntime> resolve) => new(resolve);
}
