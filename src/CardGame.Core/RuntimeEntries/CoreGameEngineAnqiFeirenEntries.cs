#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAnqiFeirenRuntime))]
internal sealed partial class CoreGameEngineAnqiFeirenEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAnqiFeirenRuntime> _resolve;
    private CoreGameEngineAnqiFeirenEntries(Func<EngineOwners.CoreGameEngineAnqiFeirenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAnqiFeirenEntries Create(Func<EngineOwners.CoreGameEngineAnqiFeirenRuntime> resolve) => new(resolve);
}
