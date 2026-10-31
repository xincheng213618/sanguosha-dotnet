#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime))]
internal sealed partial class CoreGameEngineProgramSelectedGlobalCardUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime> _resolve;
    private CoreGameEngineProgramSelectedGlobalCardUsesEntries(Func<EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramSelectedGlobalCardUsesEntries Create(Func<EngineOwners.CoreGameEngineProgramSelectedGlobalCardUsesRuntime> resolve) => new(resolve);
}
