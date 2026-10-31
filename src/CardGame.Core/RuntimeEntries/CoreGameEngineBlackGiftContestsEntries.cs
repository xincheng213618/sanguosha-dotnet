#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineBlackGiftContestsRuntime))]
internal sealed partial class CoreGameEngineBlackGiftContestsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBlackGiftContestsRuntime> _resolve;
    private CoreGameEngineBlackGiftContestsEntries(Func<EngineOwners.CoreGameEngineBlackGiftContestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBlackGiftContestsEntries Create(Func<EngineOwners.CoreGameEngineBlackGiftContestsRuntime> resolve) => new(resolve);
}
