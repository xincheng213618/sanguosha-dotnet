#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineQuotaTopProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> _resolve;
    private CoreGameEngineQuotaTopProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineQuotaTopProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> resolve) => new(resolve);
}
