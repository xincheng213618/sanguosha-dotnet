#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineYaodaoChitongEntries
{
    internal global::CardGame.Core.Card? ActiveYaodaoChitong(int seat, string skillId, string instance) => _resolve().ActiveYaodaoChitong(seat, skillId, instance);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYaodaoChitong(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginYaodaoChitong(input);
    internal bool CanOfferYaodaoChitong(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferYaodaoChitong(gate);
    internal bool ContinueIssuedYaodaoChitong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedYaodaoChitong(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveYaodaoChitong(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ResolveYaodaoChitong(input, effect);
    internal int YaodaoChitongBlackSlashBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().YaodaoChitongBlackSlashBonus(attack);
    internal bool YaodaoChitongTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NativeOwners.CardAttackHandle attack, int target) => _resolve().YaodaoChitongTarget(use, attack, target);
}
