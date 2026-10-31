#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardEffectProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> _resolve;
    private CoreGameEngineCardEffectProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardEffectProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardEffectProgramsRuntime> resolve) => new(resolve);
}
