#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiMouOlEntries
{
    internal int SequentialDeckKindGrowthGrantAmount(CardConversionSource source, long frameId, int shortfall) => _resolve().SequentialDeckKindGrowthGrantAmount(source, frameId, shortfall);
    internal int SequentialDeckKindGrowthRequestedCount(SkillProgramEffect effect, CharacterState owner) => _resolve().SequentialDeckKindGrowthRequestedCount(effect, owner);
}
