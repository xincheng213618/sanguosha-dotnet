#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualUseTargetProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> _resolve;
    private CoreGameEngineActualUseTargetProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualUseTargetProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> resolve) => new(resolve);
}
