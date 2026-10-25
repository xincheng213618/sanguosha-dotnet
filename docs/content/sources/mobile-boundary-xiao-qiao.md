# 移动版界小乔

角色为 `boundary:xiao-qiao-mobile`，移动版身份规则，吴、女、3体力。核对日期：2026-10-25。

主源为[移动版官网武将311](https://www.sanguosha.cn/index.php/pc/hero-detail-311.html)，复核身份页完整正文与隐藏重复技能段；第二来源为[移动版BWIKI界小乔](https://wiki.biligame.com/msgs/%E7%95%8C%E5%B0%8F%E4%B9%94)，页面标注2026-09-23更新。天香仅可弃置红桃手牌，防止原伤害，再令其他角色受到原伤害来源造成的1点伤害后按其损失体力摸至多5张，或失去1体力后获得原弃牌。红颜只改写实际使用、打出、弃置或交出的黑桃手牌及本人的黑桃判定；任意角色的红桃判定生效前，由小乔指定结果花色。装备区的黑桃牌不在当前移动手牌改写范围。

当前官网正文和BWIKI当前第三条都使用锁定的“将结果改为指定花色”，没有可选措辞；BWIKI的历史第二版才是可选改花色、且黑桃牌范围更宽。因此当前采用必须指定一次花色的原生判定前窗口，四种花色均可选择，包括保持红桃；不误用历史第二版或国战页。检索官方后续调整信息未发现取代官网311当前身份正文的修订。

既有OL `boundary:xiao-qiao` 另有飘零、红桃装备的体力上限手限、装备牌也可支付天香，并把全部黑桃牌改为红桃，不能整人复用。当前移动版不授予飘零或装备手限。`boundary:tianxiang-mobile` 直接复用已有完整原生天香防止、原来源1点新伤害、损失体力上限5摸牌、失去体力及原弃牌赠与机制，只将付款材料限制为手牌。原 `SuitPreventionBenefit` 付款游标、真实移动孩子及typed return负责冷恢复，不复制付款或伤害流水线。

`boundary:hongyan-mobile` 的手牌和本人判定花色查询保留实际来源位置；红桃结果改写直接进入原 `JudgmentBeforeResult` 窗口，并把标量收据存于原 `JudgmentFrame`。物理判定牌、点数和原生清理不变；冷恢复仍返回原判定。与其他生效前改结果并存时，以原生子帧的发行序号尊重后一次真实改写，不让旧红颜收据永久压过后来的紫微或大愿收据。

最少新增Core入口为 `BoundaryXiaoQiaoMobileChecks.OtherHeartJudgmentAndHandOnlyTianxiangPaidCold`：固定场景中，另一角色的真实红桃判定在生效前改为梅花并冷恢复；装备区材料不能支付，而真实黑桃手牌以红桃支付；原弃牌孩子冷恢复后只失去一次体力并获得同一物理弃牌。既有 `BoundaryXiaoQiaoChecks.TianxiangEquipmentPaymentWaitsForRecoveryBeforeNewDamageAndCappedDraw` 保持OL装备费用语义，继续覆盖共用新伤害、恢复、摸牌及上限5机制；当前版本不复制该矩阵。

画像取官网311默认第一张[官方3000立绘](https://www.sanguosha.cn/storage/uploads/images/skins/3000.jpg)，原JPEG、530×750，保存为 `src/CardGame.Wpf/Assets/official-boundary-xiao-qiao-mobile.jpg`，SHA-256为 `ac1249c7e56edc32aab26201c33f9d4e5a48e10f964c82d77427b84b255dc9b0`，未裁剪、未重绘。
