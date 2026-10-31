#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime))]
internal sealed partial class CoreGameEngineActualTurnDamageClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> _resolve;
    private CoreGameEngineActualTurnDamageClaimsEntries(Func<EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnDamageClaimsEntries Create(Func<EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> resolve) => new(resolve);
}
