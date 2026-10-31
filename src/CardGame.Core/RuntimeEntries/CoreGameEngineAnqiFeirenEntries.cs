#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAnqiFeirenRuntime))]
internal sealed partial class CoreGameEngineAnqiFeirenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAnqiFeirenRuntime> _resolve;
    private CoreGameEngineAnqiFeirenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAnqiFeirenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAnqiFeirenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAnqiFeirenRuntime> resolve) => new(resolve);
}
