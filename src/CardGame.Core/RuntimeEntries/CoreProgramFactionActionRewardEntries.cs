#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramFactionActionRewardEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> _resolve;
    private CoreProgramFactionActionRewardEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramFactionActionRewardEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> resolve) => new(resolve);
}
