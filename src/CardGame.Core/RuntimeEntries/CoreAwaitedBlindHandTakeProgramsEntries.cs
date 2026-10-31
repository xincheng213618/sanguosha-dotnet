#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAwaitedBlindHandTakeProgramsEntries
{
    private readonly Func<EngineOwners.CoreAwaitedBlindHandTakeProgramsRuntime> _resolve;
    private CoreAwaitedBlindHandTakeProgramsEntries(Func<EngineOwners.CoreAwaitedBlindHandTakeProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreAwaitedBlindHandTakeProgramsEntries Create(Func<EngineOwners.CoreAwaitedBlindHandTakeProgramsRuntime> resolve) => new(resolve);
}
