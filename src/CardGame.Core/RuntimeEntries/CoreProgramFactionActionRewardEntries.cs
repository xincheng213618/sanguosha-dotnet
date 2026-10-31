#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime))]
internal sealed partial class CoreProgramFactionActionRewardEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> _resolve;
    private CoreProgramFactionActionRewardEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramFactionActionRewardEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> resolve) => new(resolve);
}
