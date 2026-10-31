#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRandomAreaAcquisitionProofsRuntime))]
internal sealed partial class CoreGameEngineRandomAreaAcquisitionProofsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionProofsRuntime> _resolve;
    private CoreGameEngineRandomAreaAcquisitionProofsEntries(Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionProofsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRandomAreaAcquisitionProofsEntries Create(Func<EngineOwners.CoreGameEngineRandomAreaAcquisitionProofsRuntime> resolve) => new(resolve);
}
