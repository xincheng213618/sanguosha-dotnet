using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheRewardUiChecks
{

    private static void PublishPrimaryRewardStageForUi(ShanheCampaignSession session, ShanheNodeDefinition node, int gold)
    {
        // The real Move supplies the locked node. Seed this UI fixture's reward
        // boundary on a detached record and use the unchanged production planner.
        var type = typeof(ShanheCampaignSession);
        var fields = BindingFlags.Instance | BindingFlags.NonPublic;
        var stateField = type.GetField("_state", fields)!;
        var original = stateField.GetValue(session)!;
        var state = original.GetType().GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public)!
            .Invoke(original, null)!;
        state.GetType().GetProperty("BattleSetup")!.SetValue(state, null);
        state.GetType().GetProperty("Gold")!.SetValue(state, gold);
        var next = type.GetMethod("BeginBattlePrimaryRewards", fields)!.Invoke(session, [state, node])!;
        var snapshot = type.GetMethod("CreateSnapshot", fields)!.Invoke(session, [next, session.Revision]);
        stateField.SetValue(session, next);
        type.GetField("_snapshot", fields)!.SetValue(session, snapshot);
    }
}
