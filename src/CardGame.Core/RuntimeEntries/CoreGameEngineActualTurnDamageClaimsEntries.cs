#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime))]
internal sealed partial class CoreGameEngineActualTurnDamageClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> _resolve;
    private CoreGameEngineActualTurnDamageClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnDamageClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> resolve) => new(resolve);
}
