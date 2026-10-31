#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangDeckCardsEntries
{
    internal void AssertWushuangDeckGain(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertWushuangDeckGain(f, plan);
    internal bool IsWushuangDeckGainMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsWushuangDeckGainMovement(f, effect, pending);
    internal global::CardGame.Core.SkillProgramStepOutcome ObtainWushuangDeckCards(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ObtainWushuangDeckCards(supplied, effect);
    internal bool ReturnWushuangDeckGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWushuangDeckGain(f);
    internal bool ValidWushuangDeckGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWushuangDeckGain(f);
}
