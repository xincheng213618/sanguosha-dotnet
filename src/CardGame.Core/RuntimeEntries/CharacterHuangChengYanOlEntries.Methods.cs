#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangChengYanOlEntries
{
    internal string GetSkillDisplayName(string skillId) => _resolve().GetSkillDisplayName(skillId);
    internal SkillProgramStepOutcome JiezhenProgramReplaceSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().JiezhenProgramReplaceSkills(frame, effect);
    internal SkillProgramStepOutcome JiezhenProgramRestoreSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().JiezhenProgramRestoreSkills(frame, effect);
    internal int JiezhenTakeOneCardFromZones(ProgramSkillFrame frame, int targetSeat) => _resolve().JiezhenTakeOneCardFromZones(frame, targetSeat);
    internal void ObserveRoundTrickUse(IGameEvent payload) => _resolve().ObserveRoundTrickUse(payload);
    internal void PurgeExpiredRoundGrants() => _resolve().PurgeExpiredRoundGrants();
    internal void ResolveZecaiChoice(PromptChoice selected) => _resolve().ResolveZecaiChoice(selected);
    internal int UniqueRoundTrickLeaderSeat(int round) => _resolve().UniqueRoundTrickLeaderSeat(round);
    internal SkillProgramStepOutcome YinshiProgramClaimBaguaJudgmentCard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YinshiProgramClaimBaguaJudgmentCard(frame, effect);
    internal SkillProgramStepOutcome YinshiProgramPreventSourcelessDamage(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YinshiProgramPreventSourcelessDamage(frame, effect);
    internal SkillProgramStepOutcome ZecaiProgramRoundSettlement(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZecaiProgramRoundSettlement(frame, effect);
    internal bool TracksRoundTrickUses { get => _resolve().TracksRoundTrickUses; }
}
