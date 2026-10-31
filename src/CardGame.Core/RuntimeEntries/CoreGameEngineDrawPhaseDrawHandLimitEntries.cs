#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawPhaseDrawHandLimitEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseDrawHandLimitRuntime> _resolve;
    private CoreGameEngineDrawPhaseDrawHandLimitEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseDrawHandLimitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawPhaseDrawHandLimitEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseDrawHandLimitRuntime> resolve) => new(resolve);
}
