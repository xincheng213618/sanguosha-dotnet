#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangDeckCardsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ObtainWushuangDeckCards(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ObtainWushuangDeckCards(supplied, effect);
    internal bool ReturnWushuangDeckGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWushuangDeckGain(f);
    internal bool ValidWushuangDeckGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWushuangDeckGain(f);
}
