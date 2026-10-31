#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDongZhaoMobileShiRuntime))]
internal sealed partial class CharacterDongZhaoMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterDongZhaoMobileShiRuntime> _resolve;
    private CharacterDongZhaoMobileShiEntries(Func<EngineOwners.CharacterDongZhaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongZhaoMobileShiEntries Create(Func<EngineOwners.CharacterDongZhaoMobileShiRuntime> resolve) => new(resolve);
}
