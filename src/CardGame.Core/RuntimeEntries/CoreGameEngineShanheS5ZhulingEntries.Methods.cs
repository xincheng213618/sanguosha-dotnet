#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ZhulingEntries
{
    internal bool CanContinueIssuedShanheS5Jixian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedShanheS5Jixian(f);
    internal bool CanOfferShanheS5Jixian(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShanheS5Jixian(c);
    internal bool IsShanheS5JixianTarget(int owner, global::CardGame.Core.CharacterState target) => _resolve().IsShanheS5JixianTarget(owner, target);
    internal void IssueShanheS5JixianSlashUse(long useId, global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardActionContext action) => _resolve().IssueShanheS5JixianSlashUse(useId, f, action);
    internal global::CardGame.Core.ShanheS5JixianFrozenEvent RequireShanheS5JixianFrozen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireShanheS5JixianFrozen(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShanheS5Jixian(global::CardGame.Core.ProgramSkillFrame supplied, string stage) => _resolve().ResolveShanheS5Jixian(supplied, stage);
    internal void ReturnShanheS5JixianDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShanheS5JixianDraw(supplied, outcome);
    internal int ShanheS5JixianMask(int owner, global::CardGame.Core.CharacterState target) => _resolve().ShanheS5JixianMask(owner, target);
    internal int ShanheS5JixianSkillCount(global::CardGame.Core.CharacterState p) => _resolve().ShanheS5JixianSkillCount(p);
}
