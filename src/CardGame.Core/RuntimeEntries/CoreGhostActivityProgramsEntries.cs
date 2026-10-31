#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGhostActivityProgramsRuntime))]
internal sealed partial class CoreGhostActivityProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGhostActivityProgramsRuntime> _resolve;
    private CoreGhostActivityProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGhostActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGhostActivityProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGhostActivityProgramsRuntime> resolve) => new(resolve);
}
