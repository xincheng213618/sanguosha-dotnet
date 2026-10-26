using System.Reflection;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static partial class AnniversaryXiahouHuiChecks
{
    public static void NativeAiDrawnGiftAndJikunUsePublishedRecipientsCold()
    {
        var s = Scene.Start();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Dujun && s.P.Choices.Any(c => Key(c) == "select-target" && c.Targets.SequenceEqual([1])));
        s.Answer(c => c.Targets.SequenceEqual([1])); s.Play();
        var crossbow = s.V(0).Hand.Single(card => card.Kind == CardKind.Crossbow).Id;
        var slash = s.V(0).Hand.Single(card => card.Kind == CardKind.Slash).Id;
        s.Card(crossbow); s.Play(); s.Card(slash, 1);
        var skippedEarly = s.PreparePair("self-first-caused", "partner-first-received");
        s.Menu(); s.Cold();
        var frame = s.Root!; var id = frame.Id; var drawn = frame.XiahouHui!.Drawn.ToArray();
        s.Need(drawn.Length == 2 && drawn.All(card => s.Zone(card) == CardLocation.Hand(0)) &&
            s.P!.Choices.Any(choice => choice.Targets.SequenceEqual([0])),
            "The genuine paid first-damage Draw2 publishes its whole drawn set and legal self-recipient before the AI selector is consulted.");
        var choice = ChooseNativeXiahouHui(s.G, s.P!, frame);
        s.Need(s.V(0).TeamId == s.V(1).TeamId && s.V(0).TeamId != s.V(2).TeamId && s.V(1).HandCount == 0 &&
            s.P!.Choices.Any(candidate => candidate.Id == choice.Id) && choice.Targets.SequenceEqual([1]),
            "The registered native Give selector chooses the real low-hand ally from its original published menu, rather than gifting an enemy or inventing an action.");
        s.Answer(candidate => candidate.Id == choice.Id);
        s.GiftChild();
        s.Need(s.Root!.XiahouHui is { Stage: XiahouHuiStage.MovementChildren, Recipient: 1 } paid &&
            paid.Moves.Count == 2 && paid.Moves.All(move => drawn.Contains(move.CardId)),
            "The AI's original recipient issues the actual whole-drawn-card payment before its real movement child.");
        s.Cold(); s.Continue(); s.CompletePair("partner-first-received", skippedEarly); s.Play(); s.Cold();
        s.Need(drawn.All(card => s.Zone(card) == CardLocation.Hand(1)) &&
            s.E<NativeDrawInvocationRecordedEvent>().Count(draw => draw.ParentFrameId == id && draw.Requested == 2) == 1 &&
            s.E<XiahouHuiStartedEvent>().Count(start => start.FrameId == id) == 1 &&
            s.E<XiahouHuiCommittedEvent>().Count(commit => commit.FrameId == id) == 1 &&
            s.E<XiahouHuiMovedEvent>().Single(move => move.FrameId == id) is { Recipient: 1, Donor: 0, Count: 2 } &&
            !s.G.ResolutionStack.Any(root => root.Id == id),
            "The AI whole-gift moves only the original Draw2 to its selected ally and closes the original frame once, without replaying the draw or payment after cold recovery.");

        var discard = s.V(0).Hand.Take(1).Select(card => card.Id).ToArray();
        s.Need(discard.Length == 1 && s.E<XiahouHuiLossRecordedEvent>().Last().Count == 4,
            "One further real hand departure reaches the existing fifth-loss opportunity after the original equipment, Slash and whole-gift costs.");
        foreach (var card in discard) s.Use("discard-one", [card]);
        s.Offer(Jikun, "five-hand-loss"); s.Activate(); s.Menu(); s.Cold();
        var threshold = s.Root!.Id;
        choice = ChooseNativeXiahouHui(s.G, s.P!, s.Root!);
        var receiver = choice.Targets[0]; var donor = choice.Targets[1];
        s.Need(receiver != 0 && receiver != donor && choice.Targets.Count == 2 &&
            s.V(donor).HandCount == Enumerable.Range(0, 4).Max(seat => s.V(seat).HandCount) &&
            s.P!.Choices.Any(candidate => candidate.Id == choice.Id),
            "The registered Jikun selector uses the original public recipient/donor ordering and actual maximum-hand donor menu.");
        s.Answer(candidate => candidate.Id == choice.Id); s.Play(); s.Cold();
        s.Need(s.E<XiahouHuiMovedEvent>().Single(move => move.FrameId == threshold) is { Count: 1 } moved &&
            moved.Recipient == receiver && moved.Donor == donor &&
            s.E<XiahouHuiCommittedEvent>().Count(commit => commit.FrameId == threshold) == 1,
            "The selected Jikun pair pays one real native random obtain on its exact original fifth-loss frame after cold restore.");
    }

    private static PromptChoice ChooseNativeXiahouHui(GameEngine game, PendingDecision decision, ProgramSkillFrame frame)
    {
        // Attach a brain to the externally controlled fixture seat only. Player,
        // frame, issued prompt, native payment and rule state remain untouched.
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var brains = (Dictionary<int, SimpleAiBrain>)typeof(GameEngine).GetField("_aiBrains", flags)!.GetValue(game)!;
        brains[decision.PlayerSeat] = new SimpleAiBrain(decision.PlayerSeat, 79, AiDecisionPolicy.CurrentVersion);
        var router = typeof(GameEngine).GetProperty("ProgramInteractions", flags)!.GetValue(game)!;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, game.ContentRegistry.GetSkill(frame.SkillId).Program!)
            .GetPausedInstruction(frame.InstructionIndex).Effect;
        return (PromptChoice)router.GetType().GetMethod("SelectAi", flags)!.Invoke(router, [decision, frame, effect])!;
    }
}
