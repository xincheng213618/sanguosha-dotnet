#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHealthReplacementProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime> _resolve;
    private CoreGameEngineHealthReplacementProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHealthReplacementProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime> resolve) => new(resolve);
}
