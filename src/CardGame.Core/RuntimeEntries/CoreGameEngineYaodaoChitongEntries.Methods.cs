#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineYaodaoChitongEntries
{
    internal Card? ActiveYaodaoChitong(int seat, string skillId, string instance) => _resolve().ActiveYaodaoChitong(seat, skillId, instance);
    internal SkillProgramStepOutcome BeginYaodaoChitong(ProgramSkillFrame input) => _resolve().BeginYaodaoChitong(input);
    internal bool CanOfferYaodaoChitong(ProgramTriggerLegalityContext gate) => _resolve().CanOfferYaodaoChitong(gate);
    internal bool ContinueIssuedYaodaoChitong(ProgramSkillFrame f) => _resolve().ContinueIssuedYaodaoChitong(f);
    internal SkillProgramStepOutcome ResolveYaodaoChitong(ProgramSkillFrame input, SkillProgramEffect effect) => _resolve().ResolveYaodaoChitong(input, effect);
    internal int YaodaoChitongBlackSlashBonus(GameEngine.IDamageAttempt attack) => _resolve().YaodaoChitongBlackSlashBonus(attack);
    internal bool YaodaoChitongTarget(CardUseFrame use, NativeOwners.CardAttackHandle attack, int target) => _resolve().YaodaoChitongTarget(use, attack, target);
}
