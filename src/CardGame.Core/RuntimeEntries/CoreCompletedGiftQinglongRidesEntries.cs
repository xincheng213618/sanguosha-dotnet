#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime))]
internal sealed partial class CoreCompletedGiftQinglongRidesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> _resolve;
    private CoreCompletedGiftQinglongRidesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreCompletedGiftQinglongRidesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreCompletedGiftQinglongRidesRuntime> resolve) => new(resolve);
}
