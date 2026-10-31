#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeathProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> _resolve;
    private CoreGameEngineDeathProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeathProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> resolve) => new(resolve);
}
