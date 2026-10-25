# 界张辽（移动版）规则来源

核对日期：2026-10-25。角色为魏势力、男性、4 体力，身份场移动版。

- [《三国杀移动版》官网武将页：界张辽](https://www.sanguosha.cn/index.php/pc/hero-detail-167.html)。
- [移动版 Wiki：界张辽](https://wiki.biligame.com/msgs/%E7%95%8C%E5%BC%A0%E8%BE%BD)。页面现行突袭与官网一致；历史版本另列目标手牌数限制。
- 定向查找官网关于界张辽、突袭的调整公告，未检索到晚于上述现行规则且与之冲突的官方修订。

## 完整技能

**突袭**：摸牌阶段，你可以少摸任意张牌并获得等量的其他角色各一张手牌。

当前规则不比较目标与张辽的手牌数。目标必须为有手牌的其他存活角色，每名目标取得一张真实手牌；目标数不能超过该次摸牌阶段实际尚可摸的普通牌数。放弃发动保留全部普通摸牌，发动后逐名目标取得一张手牌并相应减少普通摸牌数。

## 实现边界

- 武将 `boundary:zhang-liao-mobile` 使用独立的 `boundary:tuxi-mobile`，只把既有旧版突袭的目标集合改为 `otherLivingWithHand`。仓库原 `boundary:tuxi` 的 `otherLivingHandAtLeastOwner` 保留其既有版本行为；不据此把该旧配置称为 OL 当前规则。
- 继续使用既有 `plannedNormalDrawCount`、`adjustNormalDraw(selectedTargetCount)` 和 `takeRandomHandCardFromSelectedTargets`。普通摸牌计划在选择前确定，原生目标选择保存所选座位，取牌沿既有实体移动、随机数及重放路径完成，不增加人物专属运行时、状态容器或付款阶段。
- 现有目标选择器已允许 `otherLivingWithHand` 配合动态普通摸牌数及最多八名角色；它不引入手牌数比较。旧版突袭特有的冻结比较分支不会作用于移动版。
- 共用取牌与摸牌组合已有 `DrawPhaseCompositionChecks` 和 `ShanheHuangkongChecks` 的真实结算、计划数量与冷恢复覆盖。本批保留该共用覆盖，不复制按人物重复的固定场景矩阵。

注册顺序：武将及当前牌池 59800，技能 59801。版本标识为 `character:zhang-liao` / `boundary-mobile` / `sanguosha-mobile`。
