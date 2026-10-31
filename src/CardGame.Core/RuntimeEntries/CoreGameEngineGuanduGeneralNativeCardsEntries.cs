#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralNativeCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> _resolve;
    private CoreGameEngineGuanduGeneralNativeCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralNativeCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> resolve) => new(resolve);
}
