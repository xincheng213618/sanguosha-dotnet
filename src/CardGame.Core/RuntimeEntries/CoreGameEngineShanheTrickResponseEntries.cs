#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheTrickResponseRuntime))]
internal sealed partial class CoreGameEngineShanheTrickResponseEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheTrickResponseRuntime> _resolve;
    private CoreGameEngineShanheTrickResponseEntries(Func<EngineOwners.CoreGameEngineShanheTrickResponseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheTrickResponseEntries Create(Func<EngineOwners.CoreGameEngineShanheTrickResponseRuntime> resolve) => new(resolve);
}
