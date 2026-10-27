using System.Reflection;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static partial class AnniversaryXiahouHuiChecks
{

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
