#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterQingheGongzhuOlRuntime))]
internal sealed partial class CharacterQingheGongzhuOlEntries
{
    private readonly Func<EngineOwners.CharacterQingheGongzhuOlRuntime> _resolve;
    private CharacterQingheGongzhuOlEntries(Func<EngineOwners.CharacterQingheGongzhuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQingheGongzhuOlEntries Create(Func<EngineOwners.CharacterQingheGongzhuOlRuntime> resolve) => new(resolve);
}
