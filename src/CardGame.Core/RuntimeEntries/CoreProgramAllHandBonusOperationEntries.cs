#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramAllHandBonusOperationRuntime))]
internal sealed partial class CoreProgramAllHandBonusOperationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramAllHandBonusOperationRuntime> _resolve;
    private CoreProgramAllHandBonusOperationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramAllHandBonusOperationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramAllHandBonusOperationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramAllHandBonusOperationRuntime> resolve) => new(resolve);
}
