#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime))]
internal sealed partial class CoreGameEngineExtraEffectMaterialClaimsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> _resolve;
    private CoreGameEngineExtraEffectMaterialClaimsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraEffectMaterialClaimsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> resolve) => new(resolve);
}
