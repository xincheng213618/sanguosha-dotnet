#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoiceProofsRuntime))]
internal sealed partial class CoreGameEngineProgramChoiceProofsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoiceProofsRuntime> _resolve;
    private CoreGameEngineProgramChoiceProofsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoiceProofsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramChoiceProofsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoiceProofsRuntime> resolve) => new(resolve);
}
