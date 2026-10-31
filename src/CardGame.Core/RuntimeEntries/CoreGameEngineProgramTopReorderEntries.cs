#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramTopReorderRuntime))]
internal sealed partial class CoreGameEngineProgramTopReorderEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramTopReorderRuntime> _resolve;
    private CoreGameEngineProgramTopReorderEntries(Func<EngineOwners.CoreGameEngineProgramTopReorderRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTopReorderEntries Create(Func<EngineOwners.CoreGameEngineProgramTopReorderRuntime> resolve) => new(resolve);
}
