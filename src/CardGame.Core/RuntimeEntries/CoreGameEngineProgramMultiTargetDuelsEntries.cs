#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime))]
internal sealed partial class CoreGameEngineProgramMultiTargetDuelsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> _resolve;
    private CoreGameEngineProgramMultiTargetDuelsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramMultiTargetDuelsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> resolve) => new(resolve);
}
