#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiMouOlEntries
{
    internal int SequentialDeckKindGrowthGrantAmount(global::CardGame.Core.CardConversionSource source, long frameId, int shortfall) => _resolve().SequentialDeckKindGrowthGrantAmount(source, frameId, shortfall);
    internal int SequentialDeckKindGrowthRequestedCount(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.CharacterState owner) => _resolve().SequentialDeckKindGrowthRequestedCount(effect, owner);
}
