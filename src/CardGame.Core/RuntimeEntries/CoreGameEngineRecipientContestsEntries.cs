#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientContestsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestsRuntime> _resolve;
    private CoreGameEngineRecipientContestsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientContestsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestsRuntime> resolve) => new(resolve);
}
