using CardGame.Core;

namespace CardGame.Modes.Activities;

/// <summary>Installs optional activity rule services without registering inactive rule callbacks.</summary>
public sealed class ActivityBattleRulesFeatureFactory : IGameModeFeatureFactory
{
    public string Id => "activity:battle-rules";
    public string ModeId => ModeFeatureTargets.AllModes;

    public IGameModeFeature Create(ModeFeatureCreationContext context)
    {
        var mode = context.ModeDefinition ?? throw new InvalidOperationException("Activity rules require the current mode.");
        var options = context.Options ?? throw new InvalidOperationException("Activity rules require immutable match options.");
        var features = new List<IGameModeFeature>();
        if (mode.ModeKind == ContentModeKind.Survival)
            features.Add(new WenheBattleRulesFeature(mode, options, context.GetHost<IModeBattleRulesHost>(), context.GetHost<IWenheRuleFlowHost>()));
        if (mode.GuanduActivity is not null)
            features.Add(new GuanduBattleRulesFeature(mode, context.GetHost<IModeBattleRulesHost>(), context.GetHost<IGuanduEventFlowHost>()));
        if (mode.DragonBoatActivity is not null)
            features.Add(new DragonBoatBattleRulesFeature(mode, context.GetHost<IModeBattleRulesHost>()));
        return new ModeFeatureBundle(features);
    }
}
