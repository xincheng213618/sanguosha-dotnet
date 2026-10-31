#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionRuntime> _resolve;
    private CoreGameEngineRandomAreaAcquisitionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomAreaAcquisitionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionRuntime> resolve) => new(resolve);
}
