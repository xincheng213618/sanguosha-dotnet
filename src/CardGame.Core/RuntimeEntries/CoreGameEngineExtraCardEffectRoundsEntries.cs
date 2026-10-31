#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime))]
internal sealed partial class CoreGameEngineExtraCardEffectRoundsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> _resolve;
    private CoreGameEngineExtraCardEffectRoundsEntries(Func<EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraCardEffectRoundsEntries Create(Func<EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime> resolve) => new(resolve);
}
