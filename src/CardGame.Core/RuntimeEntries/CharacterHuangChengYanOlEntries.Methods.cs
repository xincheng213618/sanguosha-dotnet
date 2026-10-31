#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangChengYanOlEntries
{
    internal string GetSkillDisplayName(string skillId) => _resolve().GetSkillDisplayName(skillId);
    internal global::CardGame.Core.SkillProgramStepOutcome JiezhenProgramReplaceSkills(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().JiezhenProgramReplaceSkills(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome JiezhenProgramRestoreSkills(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().JiezhenProgramRestoreSkills(frame, effect);
    internal int JiezhenTakeOneCardFromZones(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().JiezhenTakeOneCardFromZones(frame, targetSeat);
    internal void ObserveRoundTrickUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveRoundTrickUse(payload);
    internal void PurgeExpiredRoundGrants() => _resolve().PurgeExpiredRoundGrants();
    internal void ResolveZecaiChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZecaiChoice(selected);
    internal int UniqueRoundTrickLeaderSeat(int round) => _resolve().UniqueRoundTrickLeaderSeat(round);
    internal global::CardGame.Core.SkillProgramStepOutcome YinshiProgramClaimBaguaJudgmentCard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YinshiProgramClaimBaguaJudgmentCard(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome YinshiProgramPreventSourcelessDamage(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YinshiProgramPreventSourcelessDamage(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ZecaiProgramRoundSettlement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZecaiProgramRoundSettlement(frame, effect);
    internal bool TracksRoundTrickUses { get => _resolve().TracksRoundTrickUses; }
}
