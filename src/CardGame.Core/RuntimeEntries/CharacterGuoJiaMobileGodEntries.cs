#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoJiaMobileGodRuntime))]
internal sealed partial class CharacterGuoJiaMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterGuoJiaMobileGodRuntime> _resolve;
    private CharacterGuoJiaMobileGodEntries(Func<EngineOwners.CharacterGuoJiaMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoJiaMobileGodEntries Create(Func<EngineOwners.CharacterGuoJiaMobileGodRuntime> resolve) => new(resolve);
}
