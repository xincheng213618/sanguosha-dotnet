#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDrawTwoBonusEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime> _resolve;
    private CoreGameEngineProgramDrawTwoBonusEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDrawTwoBonusEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDrawTwoBonusRuntime> resolve) => new(resolve);
}
