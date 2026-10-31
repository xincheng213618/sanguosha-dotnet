#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime))]
internal sealed partial class CoreGameEngineShanheTrickResponseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime> _resolve;
    private CoreGameEngineShanheTrickResponseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheTrickResponseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheTrickResponseRuntime> resolve) => new(resolve);
}
