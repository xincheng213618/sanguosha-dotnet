#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreTeamActivityProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> _resolve;
    private CoreTeamActivityProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreTeamActivityProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreTeamActivityProgramsRuntime> resolve) => new(resolve);
}
