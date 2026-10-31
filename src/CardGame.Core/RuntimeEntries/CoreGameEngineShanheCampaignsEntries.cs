#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheCampaignsRuntime))]
internal sealed partial class CoreGameEngineShanheCampaignsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheCampaignsRuntime> _resolve;
    private CoreGameEngineShanheCampaignsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheCampaignsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheCampaignsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheCampaignsRuntime> resolve) => new(resolve);
}
