#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanYuHappyGodEntries
{
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> HappyGuanYuHeartSlashDamage(GameEngine.IDamageAttempt attack) => _resolve().HappyGuanYuHeartSlashDamage(attack);
    internal bool IsHappyGuanYuDyingExitCandidate(ProgramTriggerCandidate c, int victim) => _resolve().IsHappyGuanYuDyingExitCandidate(c, victim);
    internal bool IsHappyGuanYuDyingExitContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsHappyGuanYuDyingExitContext(c, context);
    internal bool IsHappyGuanYuDyingExitReturn(ProgramSkillFrame f) => _resolve().IsHappyGuanYuDyingExitReturn(f);
    internal bool HasHappyGuanYuHeartSlashCapability { get => _resolve().HasHappyGuanYuHeartSlashCapability; }
}
