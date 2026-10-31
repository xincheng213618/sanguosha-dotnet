#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime))]
internal sealed partial class CoreGameEngineRevealRecastProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> _resolve;
    private CoreGameEngineRevealRecastProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRevealRecastProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> resolve) => new(resolve);
}
