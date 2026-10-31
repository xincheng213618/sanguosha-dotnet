#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawPhaseDrawHandLimitEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawPhaseDrawHandLimitRuntime> _resolve;
    private CoreGameEngineDrawPhaseDrawHandLimitEntries(Func<EngineOwners.CoreGameEngineDrawPhaseDrawHandLimitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawPhaseDrawHandLimitEntries Create(Func<EngineOwners.CoreGameEngineDrawPhaseDrawHandLimitRuntime> resolve) => new(resolve);
}
