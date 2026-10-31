#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineYaodaoChitongEntries
{
    internal global::CardGame.Core.Card? ActiveYaodaoChitong(int seat, string skillId, string instance) => _resolve().ActiveYaodaoChitong(seat, skillId, instance);
    internal void AssertYaodaoChitongFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertYaodaoChitongFrame(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYaodaoChitong(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginYaodaoChitong(input);
    internal bool CanOfferYaodaoChitong(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferYaodaoChitong(gate);
    internal bool ContinueIssuedYaodaoChitong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedYaodaoChitong(f);
    internal global::CardGame.Core.ProgramSkillFrame? ExactYaodaoChitongJudgmentRoot(global::CardGame.Core.JudgmentFrame judgment) => _resolve().ExactYaodaoChitongJudgmentRoot(judgment);
    internal bool HasExactYaodaoChitongJudgmentSubtree(long judgmentId) => _resolve().HasExactYaodaoChitongJudgmentSubtree(judgmentId);
    internal bool IsExactYaodaoChitongJudgmentFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index) => _resolve().IsExactYaodaoChitongJudgmentFinalizedSubtree(window, judgment, index);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveYaodaoChitong(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ResolveYaodaoChitong(input, effect);
    internal int YaodaoChitongBlackSlashBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().YaodaoChitongBlackSlashBonus(attack);
    internal bool YaodaoChitongTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int target) => _resolve().YaodaoChitongTarget(use, attack, target);
}
