#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceAgricultureOfficerEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceAgricultureOfficerRuntime> _resolve;
    private CoreGameEngineSourceAgricultureOfficerEntries(Func<EngineOwners.CoreGameEngineSourceAgricultureOfficerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceAgricultureOfficerEntries Create(Func<EngineOwners.CoreGameEngineSourceAgricultureOfficerRuntime> resolve) => new(resolve);
}
