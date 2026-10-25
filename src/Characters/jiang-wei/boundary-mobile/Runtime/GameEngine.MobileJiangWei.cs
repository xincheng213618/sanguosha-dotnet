namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileJiangWeiTiaoxin = "boundary:tiaoxin-mobile";
    private bool IsMobileJiangWeiAssistedFrame(ProgramSkillFrame f, int actor) =>
        f.SkillId == MobileJiangWeiTiaoxin && f.ActivationId == "taunt" && f.TriggerId is null && f.InstructionIndex == 1 &&
        f.SelectedTargetSeats is [var selected] && selected == actor && actor != f.OwnerSeat &&
        GetPausedPrivateOfferEffect(f) is { Op: SkillProgramEffectOp.RequestSlashAgainstChosenTarget, Target: SkillProgramEffectTarget.SelectedTarget,
            ResultBind: "tiaoxin-answer", ChooserRef.Kind: ProgramParticipantRef.SelectedTarget } &&
        _contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;

    // The original assisted-use frame already owns every physical/conversion
    // payment, source invoice and native return. Only its fixed target differs.
    private bool TryBeginMobileJiangWeiTiaoxin(ProgramSkillFrame supplied, int actor, string bind, bool actorChoosesTarget,
        out SkillProgramStepOutcome outcome)
    {
        outcome = SkillProgramStepOutcome.Continue;
        if (supplied.SkillId != MobileJiangWeiTiaoxin) return false;
        var f = GetActiveProgramFrame(supplied.Id);
        if (!IsMobileJiangWeiAssistedFrame(f, actor) || bind != "tiaoxin-answer" || !actorChoosesTarget || f.AssistedSlashRequest is not null)
            throw new InvalidOperationException("Mobile Tiaoxin requires its original once-per-play activation and fixed native Slash recipient.");
        if (!_players[f.OwnerSeat].IsAlive || !_players[actor].IsAlive)
        {
            CancelProgramBindingAndCleanup(f, "挑衅参与者已失效。"); outcome = SkillProgramStepOutcome.AwaitChild; return true;
        }
        if (MobileJiangWeiAssistedTargets(f, actor) is not { Length: > 0 })
        {
            CommitProgramChoiceResult(f.Id, bind, "declined", actor, "不能对挑衅者合法使用【杀】。"); return true;
        }
        ReplaceRuntimeTop(f = f with { AssistedSlashRequest = new(actor, f.OwnerSeat, ActorChoosesTarget: true) });
        PublishAssistedPhysicalSlashPrompt(f);
        if (_pendingDecision is { } p)
            _pendingDecision = p with { Prompt = $"对{_players[f.OwnerSeat].Name}使用一张【杀】，否则其弃置你的一张牌。" };
        outcome = SkillProgramStepOutcome.AwaitChoice; return true;
    }

    private int[]? MobileJiangWeiAssistedTargets(ProgramSkillFrame f, int actor)
    {
        if (f.SkillId != MobileJiangWeiTiaoxin) return null;
        if (!IsMobileJiangWeiAssistedFrame(f, actor) || f.AssistedSlashRequest is { } draft &&
            (draft.ActorSeat != actor || draft.TargetSeat != f.OwnerSeat || !draft.ActorChoosesTarget))
            throw new InvalidOperationException("Mobile Tiaoxin changed the actor or original fixed Slash recipient.");
        return _players[actor].IsAlive && _players[f.OwnerSeat].IsAlive && AssistedPhysicalSlashTargets(actor).Contains(f.OwnerSeat)
            ? [f.OwnerSeat] : [];
    }
}
