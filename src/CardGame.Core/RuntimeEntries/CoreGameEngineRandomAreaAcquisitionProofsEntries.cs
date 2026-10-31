#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionProofsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionProofsRuntime> _resolve;
    private CoreGameEngineRandomAreaAcquisitionProofsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionProofsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomAreaAcquisitionProofsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionProofsRuntime> resolve) => new(resolve);
}
