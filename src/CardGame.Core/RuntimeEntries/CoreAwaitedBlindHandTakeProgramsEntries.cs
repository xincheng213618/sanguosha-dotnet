#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAwaitedBlindHandTakeProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreAwaitedBlindHandTakeProgramsRuntime> _resolve;
    private CoreAwaitedBlindHandTakeProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreAwaitedBlindHandTakeProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreAwaitedBlindHandTakeProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreAwaitedBlindHandTakeProgramsRuntime> resolve) => new(resolve);
}
