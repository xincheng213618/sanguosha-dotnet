#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangDeckCardsEntries
{
    internal SkillProgramStepOutcome ObtainWushuangDeckCards(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ObtainWushuangDeckCards(supplied, effect);
    internal bool ReturnWushuangDeckGain(ProgramSkillFrame f) => _resolve().ReturnWushuangDeckGain(f);
    internal bool ValidWushuangDeckGain(ProgramSkillFrame f) => _resolve().ValidWushuangDeckGain(f);
}
