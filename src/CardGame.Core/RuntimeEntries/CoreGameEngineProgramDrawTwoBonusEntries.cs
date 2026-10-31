#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime))]
internal sealed partial class CoreGameEngineProgramDrawTwoBonusEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime> _resolve;
    private CoreGameEngineProgramDrawTwoBonusEntries(Func<EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDrawTwoBonusEntries Create(Func<EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime> resolve) => new(resolve);
}
