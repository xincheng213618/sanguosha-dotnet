#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJiaXuMouRuntime))]
internal sealed partial class CharacterJiaXuMouEntries
{
    private readonly Func<EngineOwners.CharacterJiaXuMouRuntime> _resolve;
    private CharacterJiaXuMouEntries(Func<EngineOwners.CharacterJiaXuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiaXuMouEntries Create(Func<EngineOwners.CharacterJiaXuMouRuntime> resolve) => new(resolve);
}
