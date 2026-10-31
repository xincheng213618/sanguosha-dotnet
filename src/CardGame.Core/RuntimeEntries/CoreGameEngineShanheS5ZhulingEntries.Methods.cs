#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ZhulingEntries
{
    internal bool CanContinueIssuedShanheS5Jixian(ProgramSkillFrame f) => _resolve().CanContinueIssuedShanheS5Jixian(f);
    internal bool CanOfferShanheS5Jixian(ProgramTriggerLegalityContext c) => _resolve().CanOfferShanheS5Jixian(c);
    internal bool IsShanheS5JixianTarget(int owner, CharacterState target) => _resolve().IsShanheS5JixianTarget(owner, target);
    internal void IssueShanheS5JixianSlashUse(long useId, ProgramSkillFrame f, CardActionContext action) => _resolve().IssueShanheS5JixianSlashUse(useId, f, action);
    internal ShanheS5JixianFrozenEvent RequireShanheS5JixianFrozen(ProgramSkillFrame f) => _resolve().RequireShanheS5JixianFrozen(f);
    internal SkillProgramStepOutcome ResolveShanheS5Jixian(ProgramSkillFrame supplied, string stage) => _resolve().ResolveShanheS5Jixian(supplied, stage);
    internal void ReturnShanheS5JixianDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShanheS5JixianDraw(supplied, outcome);
    internal int ShanheS5JixianMask(int owner, CharacterState target) => _resolve().ShanheS5JixianMask(owner, target);
    internal int ShanheS5JixianSkillCount(CharacterState p) => _resolve().ShanheS5JixianSkillCount(p);
}
