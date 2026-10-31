#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceAgricultureOfficerEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceAgricultureOfficerRuntime> _resolve;
    private CoreGameEngineSourceAgricultureOfficerEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceAgricultureOfficerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceAgricultureOfficerEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceAgricultureOfficerRuntime> resolve) => new(resolve);
}
