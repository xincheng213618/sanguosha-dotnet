#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreProgramFactionActionRewardRuntime))]
internal sealed partial class CoreProgramFactionActionRewardEntries
{
    private readonly Func<EngineOwners.CoreProgramFactionActionRewardRuntime> _resolve;
    private CoreProgramFactionActionRewardEntries(Func<EngineOwners.CoreProgramFactionActionRewardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramFactionActionRewardEntries Create(Func<EngineOwners.CoreProgramFactionActionRewardRuntime> resolve) => new(resolve);
}
