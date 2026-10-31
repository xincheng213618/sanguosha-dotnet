#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCompletedGiftQinglongRidesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> _resolve;
    private CoreCompletedGiftQinglongRidesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreCompletedGiftQinglongRidesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> resolve) => new(resolve);
}
