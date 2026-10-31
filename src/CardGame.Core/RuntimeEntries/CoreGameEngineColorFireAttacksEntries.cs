#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineColorFireAttacksRuntime))]
internal sealed partial class CoreGameEngineColorFireAttacksEntries
{
    private readonly Func<EngineOwners.CoreGameEngineColorFireAttacksRuntime> _resolve;
    private CoreGameEngineColorFireAttacksEntries(Func<EngineOwners.CoreGameEngineColorFireAttacksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineColorFireAttacksEntries Create(Func<EngineOwners.CoreGameEngineColorFireAttacksRuntime> resolve) => new(resolve);
}
