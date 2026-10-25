# 移动版界张角

核对日期：2026-10-25。独立身份 `boundary:zhang-jiao-mobile`，群、男性、三体力，画像键 `boundary-zhang-jiao-mobile`。雷击、鬼道、黄天完整复用现有同规则的 `classic:leiji`、`classic:guidao`、`classic:huangtian`；技能ID的既有命名不改变本人物的 `boundary-mobile` 版本与 `sanguosha-mobile` 规则集身份。

- [移动版官网详情149](https://www.sanguosha.cn/pc/hero-detail-149.html)：完整身份版HTML中雷击为使用或打出闪后令其他角色判定，黑桃对其造成二点雷电伤害，梅花先自己回复一点体力再对其造成一点雷电伤害；隐藏正文为黑牌鬼道和闪/闪电黄天。
- [移动版Wiki界张角](https://wiki.biligame.com/msgs/%E7%95%8C%E5%BC%A0%E8%A7%92)及[雷击专页](https://wiki.biligame.com/msgs/%E9%9B%B7%E5%87%BB/%E7%95%8C%E5%BC%A0%E8%A7%92)（均2025-12-19更新）给出相同三体力身份与两种黑色判定分支；历史版黑色一伤回血另列，不能混入现行规则。1V1的“对手”范围也不替换身份版的其他角色。
- [鬼道/界张角](https://wiki.biligame.com/msgs/%E9%AC%BC%E9%81%93/%E7%95%8C%E5%BC%A0%E8%A7%92)和[黄天/界张角](https://wiki.biligame.com/msgs/%E9%BB%84%E5%A4%A9/%E7%95%8C%E5%BC%A0%E8%A7%92)补充确认：鬼道没有黑桃2～9的额外摸牌；黄天为其他群角色每实际出牌阶段一次交闪或闪电，不是任意黑桃手牌。
- [官方武将技能调整汇总98](https://www.sanguosha.cn/pc/news-detail-98.html)列出雷击从旧版黑色一伤改为黑桃二伤、梅花回血后一伤的调整。已检索后续官方技能改版及调整公告，未找到取代官网及当前移动版Wiki这套完整身份规则的修订。

本版与现有OL `boundary:*` 三技能有实际差异。OL雷击包含使用闪电及本人其他判定后的独立雷击，OL鬼道替换牌为黑桃2～9时摸牌，OL黄天接受闪或黑桃手牌。本版不授予这些OL节点，也不复制其定义后仅更名。

复用的雷击在本人实际闪响应后选择其他存活角色，让该角色进行原生判定；结算订阅限定判定来源为本技能拥有者及原雷击原因，沿最终判定花色对原目标结算，不把目标判定误作自己的判定。黑桃分支造成二点雷伤；梅花分支先调用本人原生恢复，再向同一目标造成一点雷伤。正常改判、伤害子流程与冷恢复继续由既有原帧和原生判定合同承担。

鬼道复用本人手牌/装备区黑牌替换的付款与旧判定牌入手合同，没有OL点数奖励；黄天复用真实主公资格、其他群势力提供者、实际闪或闪电实体及每个出牌阶段的次数账本。此次仅增加人物配置与来源说明，不新增运行时或复制夹具。

相关已有过滤器：`skill program selected judgment subjects damage and replay`、`skill program judgment replacement commits both old-card destinations and replays`、`skill program contributions bind owner filters, phase ledger and replay`，分别覆盖选定判定对象及原伤害来源、改判实体与两种旧牌去向、跨角色黄天资格及原阶段额度。
