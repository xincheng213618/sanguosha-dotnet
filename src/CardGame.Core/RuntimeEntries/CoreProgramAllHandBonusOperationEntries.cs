#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramAllHandBonusOperationRuntime))]
internal sealed partial class CoreProgramAllHandBonusOperationEntries
{
    private readonly Func<EngineOwners.CoreProgramAllHandBonusOperationRuntime> _resolve;
    private CoreProgramAllHandBonusOperationEntries(Func<EngineOwners.CoreProgramAllHandBonusOperationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramAllHandBonusOperationEntries Create(Func<EngineOwners.CoreProgramAllHandBonusOperationRuntime> resolve) => new(resolve);
}
