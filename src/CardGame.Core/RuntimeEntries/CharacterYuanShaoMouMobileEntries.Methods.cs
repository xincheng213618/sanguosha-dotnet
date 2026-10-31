#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanShaoMouMobileEntries
{
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame AppendMouYuanShaoTargets(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().AppendMouYuanShaoTargets(frame);
    internal bool CanMouYuanShaoReward(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanMouYuanShaoReward(candidate, c);
    internal bool CanOfferMouYuanShaoReward(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMouYuanShaoReward(candidate, trigger, context);
    internal void CaptureMouYuanShaoDesignation(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().CaptureMouYuanShaoDesignation(action, window);
    internal void CaptureMouYuanShaoTurn(global::CardGame.Core.TurnStartedEvent turn) => _resolve().CaptureMouYuanShaoTurn(turn);
    internal int MouYuanShaoActualRewards(int owner, string skill) => _resolve().MouYuanShaoActualRewards(owner, skill);
    internal global::CardGame.Core.MouYuanShaoTurnStartedEvent MouYuanShaoTurn() => _resolve().MouYuanShaoTurn();
    internal global::CardGame.Core.SkillProgramStepOutcome ReserveMouYuanShaoReward(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ReserveMouYuanShaoReward(supplied);
    internal bool TryBeginMouYuanShaoLateSlashTarget(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginMouYuanShaoLateSlashTarget(attack);
    internal bool HasMouYuanShaoCapability { get => _resolve().HasMouYuanShaoCapability; }
}
