#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheCampaignsRuntime))]
internal sealed partial class CoreGameEngineShanheCampaignsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheCampaignsRuntime> _resolve;
    private CoreGameEngineShanheCampaignsEntries(Func<EngineOwners.CoreGameEngineShanheCampaignsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheCampaignsEntries Create(Func<EngineOwners.CoreGameEngineShanheCampaignsRuntime> resolve) => new(resolve);
}
