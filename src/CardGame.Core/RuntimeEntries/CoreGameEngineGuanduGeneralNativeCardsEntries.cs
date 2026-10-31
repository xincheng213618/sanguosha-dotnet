#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralNativeCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> _resolve;
    private CoreGameEngineGuanduGeneralNativeCardsEntries(Func<EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralNativeCardsEntries Create(Func<EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> resolve) => new(resolve);
}
