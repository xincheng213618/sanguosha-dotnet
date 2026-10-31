#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime))]
internal sealed partial class CoreGameEngineProgramMultiTargetDuelsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> _resolve;
    private CoreGameEngineProgramMultiTargetDuelsEntries(Func<EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramMultiTargetDuelsEntries Create(Func<EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> resolve) => new(resolve);
}
