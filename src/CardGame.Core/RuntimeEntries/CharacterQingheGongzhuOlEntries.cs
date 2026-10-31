#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterQingheGongzhuOlRuntime))]
internal sealed partial class CharacterQingheGongzhuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQingheGongzhuOlRuntime> _resolve;
    private CharacterQingheGongzhuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQingheGongzhuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQingheGongzhuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQingheGongzhuOlRuntime> resolve) => new(resolve);
}
