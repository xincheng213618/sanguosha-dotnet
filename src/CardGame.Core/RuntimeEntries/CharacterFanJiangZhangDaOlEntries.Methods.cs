#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFanJiangZhangDaOlEntries
{
    internal bool AdvanceFanJiangZhangDaSubtree() => _resolve().AdvanceFanJiangZhangDaSubtree();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginJuesheng(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginJuesheng(supplied);
    internal bool CanActivateJuesheng(global::CardGame.Core.CharacterState owner) => _resolve().CanActivateJuesheng(owner);
    internal bool CanIssueJueshengDuel(int actor, int target) => _resolve().CanIssueJueshengDuel(actor, target);
    internal void ContinueJuesheng(long id) => _resolve().ContinueJuesheng(id);
    internal bool ContinuePaidJuesheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidJuesheng(f);
    internal void ExpireJueshengSkills(global::CardGame.Core.ActualTurnCursorReturnedEvent returned) => _resolve().ExpireJueshengSkills(returned);
    internal bool FanJiangZhangDaFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().FanJiangZhangDaFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? FanJiangZhangDaObserverRoot() => _resolve().FanJiangZhangDaObserverRoot();
    internal bool HasYuanchouArmorBypass(long useId, int targetSeat) => _resolve().HasYuanchouArmorBypass(useId, targetSeat);
    internal bool IsJueshengDuelUse(long id) => _resolve().IsJueshengDuelUse(id);
    internal bool JueshengActorMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.JueshengDuelOrigin origin) => _resolve().JueshengActorMatches(use, origin);
    internal long? JueshengDuelProgramParent(long id) => _resolve().JueshengDuelProgramParent(id);
    internal bool JueshengSpent(int seat) => _resolve().JueshengSpent(seat);
    internal int PrepareJueshengDuelDamage(long useId, int targetSeat) => _resolve().PrepareJueshengDuelDamage(useId, targetSeat);
    internal bool ResumeJuesheng(long id) => _resolve().ResumeJuesheng(id);
    internal bool ReturnJueshengAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnJueshengAttack(completion);
    internal void ReturnJueshengNative(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnJueshengNative(use);
    internal void SettleJueshengNative(global::CardGame.Core.JueshengDuelOrigin origin) => _resolve().SettleJueshengNative(origin);
    internal void ValidateJueshengNative(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateJueshengNative(use);
    internal bool HasFanJiangZhangDaCapability { get => _resolve().HasFanJiangZhangDaCapability; }
}
