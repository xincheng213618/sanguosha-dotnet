#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanShaoMouMobileEntries
{
    internal ProgramCardTriggerWindowFrame AppendMouYuanShaoTargets(ProgramCardTriggerWindowFrame frame) => _resolve().AppendMouYuanShaoTargets(frame);
    internal bool CanMouYuanShaoReward(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().CanMouYuanShaoReward(candidate, c);
    internal bool CanOfferMouYuanShaoReward(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMouYuanShaoReward(candidate, trigger, context);
    internal void CaptureMouYuanShaoDesignation(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().CaptureMouYuanShaoDesignation(action, window);
    internal void CaptureMouYuanShaoTurn(TurnStartedEvent turn) => _resolve().CaptureMouYuanShaoTurn(turn);
    internal int MouYuanShaoActualRewards(int owner, string skill) => _resolve().MouYuanShaoActualRewards(owner, skill);
    internal MouYuanShaoTurnStartedEvent MouYuanShaoTurn() => _resolve().MouYuanShaoTurn();
    internal SkillProgramStepOutcome ReserveMouYuanShaoReward(ProgramSkillFrame supplied) => _resolve().ReserveMouYuanShaoReward(supplied);
    internal bool TryBeginMouYuanShaoLateSlashTarget(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginMouYuanShaoLateSlashTarget(attack);
    internal bool HasMouYuanShaoCapability { get => _resolve().HasMouYuanShaoCapability; }
}
